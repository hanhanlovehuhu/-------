
using System.Net.Sockets;
using AdjSocketService.Entity;
using AdjSocketService.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AdjSocketService.AsyncSocketService
{
    //异步Socket调用对象，所有的协议处理都从本类继承     
    public class AsyncSocketInvokeElement
    {
        #region 初始化
        protected AsyncSocketServer m_asyncSocketServer;
        protected AsyncSocketUserToken m_asyncSocketUserToken;
        public AsyncSocketUserToken AsyncSocketUserToken { get { return m_asyncSocketUserToken; } }

        private bool m_netByteOrder;
        public bool NetByteOrder { get { return m_netByteOrder; } set { m_netByteOrder = value; } } //长度是否使用网络字节顺序

        protected IncomingDataParser m_incomingDataParser; //协议解析器，用来解析客户端接收到的命令
        protected OutgoingDataAssembler m_outgoingDataAssembler; //协议组装器，用来组织服务端返回的命令

        protected bool m_sendAsync; //标识是否有发送异步事件

        protected DateTime m_connectDT;
        public DateTime ConnectDT { get { return m_connectDT; } }
        protected DateTime m_activeDT;
        public DateTime ActiveDT { get { return m_activeDT; } }


        public AsyncSocketInvokeElement(AsyncSocketServer asyncSocketServer, AsyncSocketUserToken asyncSocketUserToken)
        {
            m_asyncSocketServer = asyncSocketServer;
            m_asyncSocketUserToken = asyncSocketUserToken;

            m_netByteOrder = true;

            m_incomingDataParser = new IncomingDataParser();
            m_outgoingDataAssembler = new OutgoingDataAssembler();


            m_sendAsync = false;

            m_connectDT = DateTime.UtcNow;
            m_activeDT = DateTime.UtcNow;


        }


        public virtual void Close()
        {
        }
        #endregion

        #region 接收数据
        /// <summary>
        /// 接收异步事件返回的数据，用于对数据进行缓存和分包  modified by zhangyang  20140724
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public virtual bool ProcessReceive(AsyncSocketUserToken userToken, int offset, int count)
        // public virtual bool ProcessReceive(byte[] buffer, int offset, int count) 
        {

            m_activeDT = DateTime.UtcNow;

            //拼接数据 
            DynamicBufferManager receiveBuffer = m_asyncSocketUserToken.ReceiveBuffer;

            // 写入缓存
            receiveBuffer.WriteBuffer(userToken.ReceiveEventArgs.Buffer, 0, count);
            userToken.DataReceived += count; //增加已读
            bool result = true;

            // 解析包数据 开始符 功能码 包体长度 包体（JSON） 校验码

            //处理数据  // 丢掉脏数据
            userToken.DataReceived = receiveBuffer.DataCount;
            



            while (receiveBuffer.DataCount > 6) // 长于包头 FF 0a 
            {
                while ((receiveBuffer.DataCount > 0) && (receiveBuffer.Buffer[0] != ProtocolConst.StartCharacter))
                {
                    receiveBuffer.Clear(1);
                }
                //按照长度分包
                int packetLength = BitConverter.ToInt32(receiveBuffer.Buffer, 2); //获取包长度
                if (NetByteOrder)
                    packetLength = System.Net.IPAddress.NetworkToHostOrder(packetLength); //把网络字节顺序转为本地字节顺序


                if ((packetLength > 10 * 1024 * 1024) | (receiveBuffer.DataCount > 10 * 1024 * 1024)) //最大Buffer异常保护
                {
                    userToken.DataReceived = 0;
                    return false;
                }
                if ((receiveBuffer.DataCount - 6 - 2) >= packetLength) //收到的数据达到包长度
                {

                    result = ProcessPacket(userToken, receiveBuffer.Buffer, 0, packetLength); //处理分出来的包 
                    if (result)
                    {
                        receiveBuffer.Clear(packetLength + 6 + 2); //从缓存中清理
                        userToken.DataReceived -= packetLength + 6 + 2;
                    }
                    else
                        return result;
                }
                else
                {
                    return true;
                }
            }
            return true;
        }

        void AsynCallBack(IAsyncResult result)
        {
            try
            {
                Socket sock = result.AsyncState as Socket;

                if (sock != null)
                {
                    sock.EndSend(result);
                }
            }
            catch
            {

            }
        }

        public void SendData(Socket sock, byte[] data)
        {
            sock.BeginSend(data, 0, data.Length, SocketFlags.None, AsynCallBack, sock);

        }



        /// <summary>
        /// 处理分完包后的数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public virtual bool ProcessPacket(AsyncSocketUserToken userToken, byte[] buffer, int offset, int count)
        {
            //zhangyang  解析包 生成实体
            byte[] tmpbuffer = new byte[count + 6 + 2];
            try
            {

                Array.Copy(buffer, offset, tmpbuffer, 0, count + 8);
                if (tmpbuffer[0] != ProtocolConst.StartCharacter)
                {
                    //标识该包错误 丢弃并关闭连接
                    return false; //
                }
                Byte iFlag = tmpbuffer[1];//功能包
                switch (iFlag)
                {
                    case (byte)ProtocolFlag.Customer:
                        {
                            break;
                        };
                    case (byte)ProtocolFlag.Driver:
                        {
                            break;
                        }

                }
                // 校验数据的准确性
#if DEBUG
                LogService.LogControl.WriteError(string.Format(" ReceiveData : %s ", BitConverter.ToString(tmpbuffer)));
#endif
                SendData(userToken.ConnectSocket, tmpbuffer);



                return true;
            }
            catch (Exception ex)
            {
                // 报错记录日志并关闭连接
                LogService.LogControl.WriteError(string.Format("Error Occured in package[ %s  ] because of  : % s ", BitConverter.ToString(tmpbuffer), ex.Message));
                return false;

            }

        }

        public virtual bool ProcessCommand()
        {
            return true;
        }
        #endregion

        #region 回发数据
        public virtual bool SendCompleted()
        {
            m_activeDT = DateTime.UtcNow;
            m_sendAsync = false;

            AsyncSendBufferManager asyncSendBufferManager = m_asyncSocketUserToken.SendBuffer;
            asyncSendBufferManager.ClearFirstPacket(); //清除已发送的包
            int offset = 0;
            int count = 0;
            if (asyncSendBufferManager.GetFirstPacket(ref offset, ref count))
            {
                m_sendAsync = true;
                return m_asyncSocketServer.SendAsyncEvent(m_asyncSocketUserToken.ConnectSocket, m_asyncSocketUserToken.SendEventArgs,
                    asyncSendBufferManager.DynamicBufferManager.Buffer, offset, count);
            }
            else
                return SendCallback();
        }

        //发送回调函数，用于连续下发数据
        public virtual bool SendCallback()
        {
            return true;
        }

        public bool DoSendResult()
        {
            string commandText = m_outgoingDataAssembler.GetProtocolText();
            byte[] bufferUTF8 = Encoding.UTF8.GetBytes(commandText);
            int totalLength = sizeof(int) + bufferUTF8.Length; //获取总大小
            AsyncSendBufferManager asyncSendBufferManager = m_asyncSocketUserToken.SendBuffer;
            asyncSendBufferManager.StartPacket();
            asyncSendBufferManager.DynamicBufferManager.WriteInt(totalLength, false); //写入总大小
            asyncSendBufferManager.DynamicBufferManager.WriteInt(bufferUTF8.Length, false); //写入命令大小
            asyncSendBufferManager.DynamicBufferManager.WriteBuffer(bufferUTF8); //写入命令内容
            asyncSendBufferManager.EndPacket();

            bool result = true;
            if (!m_sendAsync)
            {
                int packetOffset = 0;
                int packetCount = 0;
                if (asyncSendBufferManager.GetFirstPacket(ref packetOffset, ref packetCount))
                {
                    m_sendAsync = true;
                    result = m_asyncSocketServer.SendAsyncEvent(m_asyncSocketUserToken.ConnectSocket, m_asyncSocketUserToken.SendEventArgs,
                        asyncSendBufferManager.DynamicBufferManager.Buffer, packetOffset, packetCount);
                }
            }
            return result;
        }

        public bool DoSendResult(AsyncSocketUserToken token)
        {
            string commandText = m_outgoingDataAssembler.GetProtocolText();
            byte[] bufferUTF8 = Encoding.UTF8.GetBytes(commandText);
            int totalLength = sizeof(int) + bufferUTF8.Length; //获取总大小
            AsyncSendBufferManager asyncSendBufferManager = token.SendBuffer;
            asyncSendBufferManager.StartPacket();
            asyncSendBufferManager.DynamicBufferManager.WriteInt(totalLength, false); //写入总大小
            asyncSendBufferManager.DynamicBufferManager.WriteInt(bufferUTF8.Length, false); //写入命令大小
            asyncSendBufferManager.DynamicBufferManager.WriteBuffer(bufferUTF8); //写入命令内容
            asyncSendBufferManager.EndPacket();

            bool result = true;
            if (!m_sendAsync)
            {
                int packetOffset = 0;
                int packetCount = 0;
                if (asyncSendBufferManager.GetFirstPacket(ref packetOffset, ref packetCount))
                {
                    m_sendAsync = true;
                    result = m_asyncSocketServer.SendAsyncEvent(token.ConnectSocket, token.SendEventArgs,
                        asyncSendBufferManager.DynamicBufferManager.Buffer, packetOffset, packetCount);
                }
            }
            return result;
        }
        public bool DoSendResult(byte[] buffer, int offset, int count)
        {
            string commandText = m_outgoingDataAssembler.GetProtocolText();
            byte[] bufferUTF8 = Encoding.UTF8.GetBytes(commandText);
            int totalLength = sizeof(int) + bufferUTF8.Length + count; //获取总大小
            AsyncSendBufferManager asyncSendBufferManager = m_asyncSocketUserToken.SendBuffer;
            asyncSendBufferManager.StartPacket();
            asyncSendBufferManager.DynamicBufferManager.WriteInt(totalLength, false); //写入总大小
            asyncSendBufferManager.DynamicBufferManager.WriteInt(bufferUTF8.Length, false); //写入命令大小
            asyncSendBufferManager.DynamicBufferManager.WriteBuffer(bufferUTF8); //写入命令内容
            asyncSendBufferManager.DynamicBufferManager.WriteBuffer(buffer, offset, count); //写入二进制数据
            asyncSendBufferManager.EndPacket();

            bool result = true;
            if (!m_sendAsync)
            {
                int packetOffset = 0;
                int packetCount = 0;
                if (asyncSendBufferManager.GetFirstPacket(ref packetOffset, ref packetCount))
                {
                    m_sendAsync = true;
                    result = m_asyncSocketServer.SendAsyncEvent(m_asyncSocketUserToken.ConnectSocket, m_asyncSocketUserToken.SendEventArgs,
                        asyncSendBufferManager.DynamicBufferManager.Buffer, packetOffset, packetCount);
                }
            }
            return result;
        }
        /// <summary>
        ///不是按包格式下发一个内存块，用于日志这类下发协议
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool DoSendBuffer(byte[] buffer, int offset, int count)
        {
            AsyncSendBufferManager asyncSendBufferManager = m_asyncSocketUserToken.SendBuffer;
            asyncSendBufferManager.StartPacket();
            asyncSendBufferManager.DynamicBufferManager.WriteBuffer(buffer, offset, count);
            asyncSendBufferManager.EndPacket();

            bool result = true;
            if (!m_sendAsync)
            {
                int packetOffset = 0;
                int packetCount = 0;
                if (asyncSendBufferManager.GetFirstPacket(ref packetOffset, ref packetCount))
                {
                    m_sendAsync = true;
                    result = m_asyncSocketServer.SendAsyncEvent(m_asyncSocketUserToken.ConnectSocket, m_asyncSocketUserToken.SendEventArgs,
                        asyncSendBufferManager.DynamicBufferManager.Buffer, packetOffset, packetCount);
                }
            }
            return result;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 字符连接
        /// </summary>
        /// <param name="text"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string LinkText(string text, string value)
        {
            return text + ProtocolKey.ReturnWrap + value;
        }
        #endregion
    }
}
