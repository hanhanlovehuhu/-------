using System.Web.Configuration;
using AdjSocketService.Entity;
using AdjSocketService.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace AdjSocketService.AsyncSocketService
{
    public class AsyncSocketServer
    {
        #region 初始化
        private Socket listenSocket;

        private int m_numConnections; //最大支持连接个数
        private int m_receiveBufferSize; //每个连接接收缓存大小
        private Semaphore m_maxNumberAcceptedClients; //限制访问接收连接的线程数，用来控制最大并发数

        private int m_socketTimeOutMS;
        /// <summary>
        /// Socket最大超时时间，单位为MS
        /// </summary>
        public int SocketTimeOutMS { get { return m_socketTimeOutMS; } set { m_socketTimeOutMS = value; } }

        private AsyncSocketUserTokenPool m_asyncSocketUserTokenPool;
        private AsyncSocketUserTokenList m_asyncSocketUserTokenList;
        public AsyncSocketUserTokenList AsyncSocketUserTokenList { get { return m_asyncSocketUserTokenList; } }

        public static AsyncSocketServer SocketServer { get; set; }
        private DaemonThread m_daemonThread;

        public AsyncSocketServer(int numConnections)
        {
            m_numConnections = numConnections;
            m_receiveBufferSize = ProtocolConst.ReceiveBufferSize;

            m_asyncSocketUserTokenPool = new AsyncSocketUserTokenPool(numConnections);
            m_asyncSocketUserTokenList = new AsyncSocketUserTokenList();
            m_maxNumberAcceptedClients = new Semaphore(numConnections, numConnections);

        }

        public void Init()
        {
            AsyncSocketUserToken userToken;
            //按照连接数建立读写对象
            for (int i = 0; i < m_numConnections; i++)
            {
                userToken = new AsyncSocketUserToken(m_receiveBufferSize);
                userToken.ReceiveEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                userToken.SendEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                m_asyncSocketUserTokenPool.Push(userToken);
            }
        }
        #endregion

        #region 开始接收
        public void Start(IPEndPoint localEndPoint)
        {
            listenSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listenSocket.Bind(localEndPoint);
            listenSocket.Listen(m_numConnections);
            StartAccept(null);
            m_daemonThread = new DaemonThread(this);
            RedisService redis = new RedisService();
            SocketServer = this;
        }

        public void StartAccept(SocketAsyncEventArgs acceptEventArgs)
        {
            if (acceptEventArgs == null)
            {
                acceptEventArgs = new SocketAsyncEventArgs();
                acceptEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(AcceptEventArg_Completed);
            }
            else
            {
                //释放上次绑定的Socket，等待下一个Socket连接
                acceptEventArgs.AcceptSocket = null;
            }
            //获取信号量
            m_maxNumberAcceptedClients.WaitOne();
            bool willRaiseEvent = listenSocket.AcceptAsync(acceptEventArgs);
            if (!willRaiseEvent)
            {
                ProcessAccept(acceptEventArgs);
            }
        }
        #endregion
        void AcceptEventArg_Completed(object sender, SocketAsyncEventArgs acceptEventArgs)
        {
            try
            {
                ProcessAccept(acceptEventArgs);
            }
            catch (Exception E)
            {
                LogService.LogControl.WriteError(string.Format("Accept client {0} error, message: {1}", acceptEventArgs.AcceptSocket, E.Message));
            }
        }

        private void ProcessAccept(SocketAsyncEventArgs acceptEventArgs)
        {
            AsyncSocketUserToken userToken = m_asyncSocketUserTokenPool.Pop();
            m_asyncSocketUserTokenList.Add(userToken); //添加到正在连接列表
            userToken.ConnectSocket = acceptEventArgs.AcceptSocket;
            userToken.ConnectDateTime = DateTime.Now;
            try
            {
                bool willRaiseEvent = userToken.ConnectSocket.ReceiveAsync(userToken.ReceiveEventArgs); //投递接收请求
                if (!willRaiseEvent)
                {
                    lock (userToken)
                    {
                        ProcessReceive(userToken.ReceiveEventArgs);
                    }
                }
            }
            catch (Exception E)
            {
                LogService.LogControl.WriteError(string.Format("Accept client {0} error, message: {1}", userToken.ConnectSocket, E.Message));
            }

            StartAccept(acceptEventArgs); //把当前异步事件释放，等待下次连接
        }

        void IO_Completed(object sender, SocketAsyncEventArgs asyncEventArgs)
        {
            AsyncSocketUserToken userToken = asyncEventArgs.UserToken as AsyncSocketUserToken;
            userToken.ActiveDateTime = DateTime.Now;
            try
            {
                lock (userToken)
                {
                    if (asyncEventArgs.LastOperation == SocketAsyncOperation.Receive)
                        ProcessReceive(asyncEventArgs);
                    else if (asyncEventArgs.LastOperation == SocketAsyncOperation.Send)
                        ProcessSend(asyncEventArgs);
                    else
                        throw new ArgumentException("The last operation completed on the socket was not a receive or send");
                }
            }
            catch (Exception E)
            {
                LogService.LogControl.WriteError(string.Format("IO_Completed {0} error, message: {1}", userToken.ConnectSocket, E.Message));
            }
        }

        private void SubBuffer(SocketAsyncEventArgs aBuf, Int32 aStartIndex)
        {
            Int32 aLength = Math.Min(aBuf.Count, aBuf.BytesTransferred);
            if (aStartIndex > aLength) return;
            Byte[] aTmpBuffer = new byte[aLength - aStartIndex];
            Array.Copy(aBuf.Buffer, aStartIndex, aTmpBuffer, 0, aLength - aStartIndex);
            aBuf.SetBuffer(aTmpBuffer, 0, aLength - aStartIndex);

        }

        private void ProcessReceive(SocketAsyncEventArgs receiveEventArgs)
        {

            // zhangyang
            AsyncSocketUserToken userToken = receiveEventArgs.UserToken as AsyncSocketUserToken;
            if (userToken.ConnectSocket == null)
                return;
            userToken.ActiveDateTime = DateTime.Now;
            int offset = userToken.DataReceived; // 已读数据
            int count = userToken.ReceiveEventArgs.BytesTransferred;
            if (userToken.ReceiveEventArgs.BytesTransferred > 0 && userToken.ReceiveEventArgs.SocketError == SocketError.Success)
            {
                if (count > 0) //处理接收数据
                {
                    // 绑定协议
                    userToken.AsyncSocketInvokeElement = new DriverSocketProtocol(this, userToken);
                    //第二个字节为协议

                    if (!userToken.AsyncSocketInvokeElement.ProcessReceive(userToken, offset, count))
                    { //如果处理数据返回失败，则断开连接
                        CloseClientSocket(userToken);
                    }
                    else //否则投递下次介绍数据请求
                    {
                        bool willRaiseEvent = userToken.ConnectSocket.ReceiveAsync(userToken.ReceiveEventArgs); //投递接收请求
                        if (!willRaiseEvent)
                            ProcessReceive(userToken.ReceiveEventArgs);
                    }
                }
                else
                {
                    bool willRaiseEvent = userToken.ConnectSocket.ReceiveAsync(userToken.ReceiveEventArgs); //投递接收请求
                    if (!willRaiseEvent)
                        ProcessReceive(userToken.ReceiveEventArgs);
                }
            }
            else
            {
                CloseClientSocket(userToken);
            }
        }

        private void BuildingSocketInvokeElement(AsyncSocketUserToken userToken)
        {
            /*
             *  modified by zhangyang  20140724
             byte caStart = userToken.ReceiveEventArgs.Buffer[0]; 
             byte flag = userToken.ReceiveEventArgs.Buffer[userToken.ReceiveEventArgs.Offset];
             if (flag == (byte)ProtocolFlag.Driver)
                 userToken.AsyncSocketInvokeElement = new DriverSocketProtocol (this, userToken);
             if (flag == (byte)ProtocolFlag.Customer)
                 userToken.AsyncSocketInvokeElement = new ClientSocketProtocol(this, userToken);
             if (userToken.AsyncSocketInvokeElement != null)
             {               
                 LogService.LogControl.WriteError(string.Format("Building socket invoke element {0}.Local Address: {1}, Remote Address: {2}",
                     userToken.AsyncSocketInvokeElement, userToken.ConnectSocket.LocalEndPoint, userToken.ConnectSocket.RemoteEndPoint)); 
             }
             * */
        }

        private bool ProcessSend(SocketAsyncEventArgs sendEventArgs)
        {
            AsyncSocketUserToken userToken = sendEventArgs.UserToken as AsyncSocketUserToken;
            if (userToken.AsyncSocketInvokeElement == null)
                return false;
            userToken.ActiveDateTime = DateTime.Now;
            if (sendEventArgs.SocketError == SocketError.Success)
                return userToken.AsyncSocketInvokeElement.SendCompleted(); //调用子类回调函数
            else
            {
                CloseClientSocket(userToken);
                return false;
            }
        }

        public bool SendAsyncEvent(Socket connectSocket, SocketAsyncEventArgs sendEventArgs, byte[] buffer, int offset, int count)
        {
            if (connectSocket == null)
                return false;
            sendEventArgs.SetBuffer(buffer, offset, count);
            bool willRaiseEvent = connectSocket.SendAsync(sendEventArgs);
            if (!willRaiseEvent)
            {
                return ProcessSend(sendEventArgs);
            }
            else
                return true;
        }

        public void CloseClientSocket(AsyncSocketUserToken userToken)
        {
            if (userToken.ConnectSocket == null)
                return;
            string socketInfo = string.Format("Local Address: {0} Remote Address: {1}", userToken.ConnectSocket.LocalEndPoint,
                userToken.ConnectSocket.RemoteEndPoint);
            LogService.LogControl.WriteError(string.Format("Client connection disconnected. {0}", socketInfo));
            try
            {
                userToken.ConnectSocket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception E)
            {
                LogService.LogControl.WriteError(string.Format("CloseClientSocket Disconnect client {0} error, message: {1}", socketInfo, E.Message));
            }
            userToken.ConnectSocket.Close();
            userToken.ConnectSocket = null; //释放引用，并清理缓存，包括释放协议对象等资源

            m_maxNumberAcceptedClients.Release();
            m_asyncSocketUserTokenPool.Push(userToken);
            m_asyncSocketUserTokenList.Remove(userToken);
        }
    }
}
