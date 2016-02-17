using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUtility
{
    public class LogHelper
    {
        public void log(string str,string path)
        {

            string newFile = path.Substring(0, path.LastIndexOf('.')) + "_" + DateTime.Now.ToString("yyyy-MM-dd") + path.Substring(path.LastIndexOf('.'));
            formatStr(ref str);
            try
            {
                File.AppendAllText(newFile, str, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void formatStr(ref string str)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("=======================================" + DateTime.Now + "====================================");
            sb.AppendLine(str);
            //sb.AppendLine("================================================================================================");
            sb.AppendLine();
            sb.AppendLine();
            str = sb.ToString();
        }
    }
}
