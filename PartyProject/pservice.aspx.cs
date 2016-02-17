using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebBll;
using WebModel;
using WebUtility;

namespace PartyProject
{
    public partial class pservice : System.Web.UI.Page
    {
        LogHelper log = new LogHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            string logStr = "";
            try
            {
                logStr = "From:" + getIp() + "\r\n";
                string requestXML = "";
                Stream StreamXML = Request.InputStream;
                if (StreamXML == null)
                {
                    requestXML = Encoding.UTF8.GetString(Request.BinaryRead(Request.TotalBytes));
                }
                else
                {
                    var streamReader = new StreamReader(StreamXML, Encoding.UTF8);
                    requestXML = streamReader.ReadToEnd();
                }

                logStr += "Input:\r\n" + requestXML + "\r\n";
                if (requestXML.IndexOf("|") == 0)
                {
                    throw new Exception("服务请求失败");
                }
                string returnString = JsonBusinessFactory.Execute(requestXML);
                logStr += "Result:\r\n" + returnString + "\r\n";
                Send(returnString);
            }
            catch (Exception ex)
            {
                string returnString = "EX|" + ex.Message;
                logStr += "\r\nResult:\r\n" + returnString + "\r\n";
                Send(returnString);
            }
            finally
            {
                log.log(logStr, Server.MapPath("Log/log.txt"));
            }

        }
        private void Send(string str)
        {
            byte[] b = Encoding.UTF8.GetBytes(str);
            Response.OutputStream.Write(b, 0, b.Length);
            Response.Flush();
        }
        private string getIp()
        {
            try
            {
                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                {
                    string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (!string.IsNullOrEmpty(ip))
                        return ip.Split(new char[] { ',' })[0];
                }
                return System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            catch
            {
                return "Ip=0.0.0.0";
            }
        }
    }
}