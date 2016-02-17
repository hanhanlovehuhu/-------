using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;

namespace WebUtility
{
    public class GetData
    {
        public string GetWebContent(string Url, string pagenum, string cat)
        {
            string strResult = "";
            try
            {
                WebClient WebClientObj = new WebClient();
                System.Collections.Specialized.NameValueCollection PostVars = new System.Collections.Specialized.NameValueCollection();
                PostVars.Add("Cat", cat);
                PostVars.Add("mnonly", "0");
                PostVars.Add("newproducts", "0");
                PostVars.Add("ColumnSort", "0");
                PostVars.Add("page", pagenum);
                PostVars.Add("stock", "0");
                PostVars.Add("pbfree", "0");
                PostVars.Add("rohs", "0");
                byte[] byRemoteInfo = WebClientObj.UploadValues(Url, "POST", PostVars);
                //StreamReader streamRead = new StreamReader(byRemoteInfo.ToString(), Encoding.Default);
                //FileStream fs = new FileStream(@"D:\\gethtml.txt", FileMode.Create);
                //BinaryWriter sr = new BinaryWriter(fs);
                //sr.Write(byRemoteInfo, 0, byRemoteInfo.Length);
                //sr.Close();
                //fs.Close();
                strResult = Encoding.Default.GetString(byRemoteInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strResult;
        }
        public string GetWebContent(string Url, Encoding encoding)
        {
            string strResult = "";
            //try
            //{
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            //声明一个HttpWebRequest请求
            request.Timeout = 30000;
            //设置连接超时时间
            request.Headers.Set("Pragma", "no-cache");
            // request.Headers.Set("KeepAlive", "true");
            request.CookieContainer = new CookieContainer();
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Referer = Url;

            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.0; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream streamReceive = response.GetResponseStream();

            StreamReader streamReader = new StreamReader(streamReceive, encoding);
            strResult = streamReader.ReadToEnd();
            streamReceive.Close();
            streamReader.Close();
            streamReceive = null;
            streamReader = null;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            return strResult;
        }
        public string GetWebContent(string Url, Encoding encoding, ref CookieContainer Cookies)
        {
            string strResult = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                //声明一个HttpWebRequest请求
                request.Timeout = 30000;
                //设置连接超时时间
                request.Headers.Set("Pragma", "no-cache");
                // request.Headers.Set("KeepAlive", "true");
                request.CookieContainer = new CookieContainer();
                request.Credentials = CredentialCache.DefaultCredentials;
                request.Referer = Url;

                request.CookieContainer = Cookies;
                request.CookieContainer.Add(new Uri(Url),
                new Cookie("PreferredCurrency_www", "USDu"));
                request.CookieContainer.Add(new Uri(Url),
               new Cookie("PreferredSubdomain", "www"));
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.0; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream streamReceive = response.GetResponseStream();

                StreamReader streamReader = new StreamReader(streamReceive, encoding);
                strResult = streamReader.ReadToEnd();
                streamReceive.Close();
                streamReader.Close();
                streamReceive = null;
                streamReader = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strResult;
        }

        public bool GetImg(string url, string filename)
        {
            bool issusess = true;
            if (url == null || url.Trim() == "")
            { return false; }
            else
            {
                try
                {

                    WebClient wc = new WebClient(); //定义
                    wc.Credentials = CredentialCache.DefaultCredentials;
                    wc.DownloadFile(url, System.Web.HttpContext.Current.Server.MapPath("~\\upimage") + "\\" + filename);
                    return issusess; //.ASCII.GetString 
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
