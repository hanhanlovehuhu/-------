using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Aidaijia.API
{
    public class ProtocolConfig
    {
        public static string APIServer = ConfigurationManager.AppSettings["apiServer"];
        public static int APIPort = Convert.ToInt32(ConfigurationManager.AppSettings["apiPort"]);      
    }
}