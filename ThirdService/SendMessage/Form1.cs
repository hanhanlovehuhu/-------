using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace SendMessage
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public string aTCPServerHost = "172.18.2.121";
        public int aTCPPort = 8931;
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("请录入发送内容!");
                return;
            }
            Msg msg = new Msg();
            msg.MsgId = 0;
            msg.MsgContent = textBox1.Text;
           // msg.MsgContent = "2011-07-07 一嗨租车“代驾事件”或为IPO狙击 初步材料已证实";
            if (radioButton1.Checked)
            {
                msg.Status = 1;
            }
            else if (radioButton2.Checked)
            {
                msg.Status = 3;
            }
            else if (radioButton3.Checked)
            {
                msg.Status = 2;
            }
            TcpClient aClient = new TcpClient();
            aClient.ReceiveTimeout = 10 * 1000;
            Byte[] data = System.Text.Encoding.UTF8.GetBytes(Aidaijia.API.Common.JSONHelper.GetJSON<Msg>(msg));

            Byte[] SendData = new byte[data.Length + 8 + 1]; //头尾共8位 多一位传送01 用于绑定客户端协议
            SendData[0] = 0x04;
            SendData[1] = 0xFF;
            SendData[2] = 0x04;
            int packetLength = data.Length;
            packetLength = System.Net.IPAddress.NetworkToHostOrder(packetLength);
            //长度
            Array.Copy(BitConverter.GetBytes(packetLength), 0, SendData, 3, 4);
            // 包体
            Array.Copy(data, 0, SendData, 7, data.Length);
            aClient.Connect(aTCPServerHost, aTCPPort);
            NetworkStream stream = aClient.GetStream();
            stream.ReadTimeout = 1000 * 60 * 5;
            stream.Write(SendData, 0, SendData.Length);
        }
    }
}
