using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebUtility;

namespace PartyProject
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            byte[] bytes = Encoding.UTF8.GetBytes("1210|SSWXNCUV5A4T|3fe5d0205970e8a732686c7cc2e74745|a4ec42535670f556511fe41c170be26b");
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://172.18.2.126:9991/pservice.aspx");  // localhost
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://192.168.10.50:18876/pservice.aspx");   // 192.168.10.50
            request.Method = "POST";
            request.ContentLength = bytes.Length;
            request.ContentType = "text/xml";
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(bytes, 0, bytes.Length);
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                string message = String.Format("POST failed. Received HTTP {0}",
                response.StatusCode);
                //throw new ApplicationException(message);
                Response.Write(message);
            }
            else
            {
                Stream responseStream= response.GetResponseStream();
                var streamReader = new StreamReader(responseStream, Encoding.UTF8);
                string output = streamReader.ReadToEnd();
                Response.Write(output);
            }

        }
    }
}
