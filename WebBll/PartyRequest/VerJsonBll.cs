using System.Xml;
using System.Web;
using Newtonsoft.Json;
using WebModel;
using WebUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using Newtonsoft.Json;

namespace WebBll
{
    public class VerJsonBll : JsonCommand
    {
        public string execute(string request)
        {
            string[] req = request.Split('|');
            string requestXML = string.Empty;
            if (req[1] == "iPhone")
            {
                requestXML = HttpContext.Current.Server.MapPath("~/Iphoneversion.xml");
            }
            else if (req[1] == "WinPhone")
            {
                requestXML = HttpContext.Current.Server.MapPath("~/WinPhoneversion.xml");
            }
            else
            {
                requestXML = HttpContext.Current.Server.MapPath("~/version.xml");
            }
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(requestXML);
            UpVerResponse response = (UpVerResponse)XmlSerialize.Deserialize(xmlDoc.InnerXml, typeof(UpVerResponse));
            return JsonConvert.SerializeObject(response);
        }
    }
}
