using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebUtility;

namespace WebBll
{
    public class CreateBusinessOrder
    {
        /// <summary>
        /// 通过Redis接口下商务订单
        /// </summary>
        /// <param name="url"></param>
        /// <param name="createOrderEntity"></param>
        /// <returns></returns>
        public string InvokeCreateOrderService(string url, object createOrderEntity)
        {
            string json = JsonConvert.SerializeObject(createOrderEntity);
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

            if (request != null)
            {
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                byte[] data = Encoding.UTF8.GetBytes(json);

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                if (response != null)
                {
                    StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    string ret = sr.ReadToEnd(); //成功 {"Result":"1","OrderId": "KFWPE8NBG3WZ", "Status": "5"，"ErrorMsg":"创建订单成功"}失败 {"Result":"0","OrderId": NULL", "Status":NULL，"ErrorMsg":"创建失败"}
                    sr.Close();
                    response.Close();

                    if (ret.Length > 0)
                    {
                        return ret;
                    }
                    else
                    {
                        return string.Empty;
                    }

                }
                else
                {
                    throw new Exception("创建订单服务调用失败!");
                }
            }
            else
            {
                throw new Exception("创建订单服务调用失败!");
            }
        }
    }
}
