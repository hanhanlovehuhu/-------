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
    public class CreateOrder
    {
        /// <summary>
        /// 通过Redis接口下订单
        /// </summary>
        /// <param name="url"></param>
        /// <param name="createOrderEntity"></param>
        /// <returns></returns>
        public string InvokeCreateOrderService(string url, CreateOrderServiceRequestEntity createOrderEntity)
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
                    string ret = sr.ReadToEnd(); // ["SS38AKHTQO5Z"]
                    sr.Close();
                    response.Close();
                    string[] result = JsonHelper.GetObject<string[]>(ret);
                    if (result.Length > 0)
                    {
                        return result[0];
                    }
                    else
                    {
                        return null;
                    }
                    
                }
                else
                {
                    throw new Exception("预约订单服务调用失败!");
                }
            }
            else
            {
                throw new Exception("预约订单服务调用失败!");
            }
        }

        /// <summary>
        /// 调用web service创建订单
        /// </summary>
        /// <param name="createOrderEntity">创建订单参数</param>
        /// <returns>0 指定司机订单创建不成功；1 - 指定司机订单创建成功</returns>
        private string InvokeCreateOrderService(CreateOrderServiceRequestEntity createOrderEntity)
        {
            string json = JsonConvert.SerializeObject(createOrderEntity);
            string result = null;
            try
            {
                object[] args = new object[1];
                args[0] = json;
                string createOrderService = ConfigHelper.AdjOrderService;
                object obj = DlWebService.InvokeWebService(createOrderService + "/AidaijiaServer.asmx"
                    , "CreateOrder"
                    , args);
                string[] orderIDs = JsonHelper.GetObject<string[]>(obj.ToString());
                result = orderIDs[0];
            }
            catch
            {
                throw new Exception("预约订单服务调用失败!");
            }

            return result;
        }

    }
}
