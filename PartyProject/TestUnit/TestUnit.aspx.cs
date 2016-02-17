using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PartyProject.TestUnit
{
    public partial class TestUnit : System.Web.UI.Page
    {
        private string ServerPage = "http://192.168.10.53:18876/pservice.aspx";
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_MD5_Click(object sender, EventArgs e)
        {
            string md5Str = GetMd5(txt_MD5.Text.Trim(), "utf-8");
            txt_Response.Text = md5Str;
        }

        protected void btn_SendTest_Click(object sender, EventArgs e)
        {
            try
            {
                string requestStr = txt_Request.Text.Trim();
                string responseStr = RequestAndResponse(requestStr); ;
                txt_Response.Text = responseStr;
            }
            catch (Exception ex)
            {
                string returnString = "EX|" + ex.Message;
                txt_Response.Text = returnString;
            }
        }

        /// <summary>
        /// 获取MD5加密串
        /// </summary>
        /// <param name="str">待加密串</param>
        /// <param name="inputCharset">编码</param>
        /// <returns>返回加密串</returns>
        private string GetMd5(string str, string inputCharset)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding(inputCharset).GetBytes(str));
            var md5str = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                md5str.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return md5str.ToString();
        }

        /// <summary>
        /// 组织Http请求
        /// </summary>
        /// <param name="requestStr">请求串</param>
        /// <returns>响应串</returns>
        private string RequestAndResponse(string requestStr)
        {
            byte[] dataArray = Encoding.UTF8.GetBytes(requestStr);
            //创建请求
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(ServerPage);
            request.Method = "POST";
            request.ContentLength = dataArray.Length;
            //request.ContentType = "application/x-www-form-urlencoded";
            //创建输入流
            Stream dataStream = null;
            try
            {
                dataStream = request.GetRequestStream();
            }
            catch (Exception ex)
            {
                return ex.Message;//连接服务器失败
            }
            //发送请求
            dataStream.Write(dataArray, 0, dataArray.Length);
            dataStream.Close();
            //读取返回消息
            string res = string.Empty;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                res = reader.ReadToEnd();
                reader.Close();
            }
            catch (Exception ex)
            {
                return ex.Message;//连接服务器失败
            }
            return res;
        }
    }
}