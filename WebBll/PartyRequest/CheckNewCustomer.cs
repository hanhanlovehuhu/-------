using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebDal;
using WebModel;
using WebUtility;

namespace WebBll.PartyRequest
{
    class CheckNewCustomer : JsonCommand
    {
        //1217|1用户手机号|2sign|3IMEI|4MD5串
        public string execute(string request)
        {
            string[] req = request.Split('|');
            var sign = EncodingUtil.GetMd5(req[1] + "aidaijia", "utf-8");

            if (sign.ToLower() == req[4].ToLower())
            {
                T_ParterEntity parter = new T_PartyDal().GetParterModelBySign(req[2].ToLower());
                if (parter != null)
                {

                    return InvokeGetOrderStateService(req[1]);
                }
                else
                {
                    throw new Exception("商户标识错误");
                }
            }
            else
            {
                throw new Exception("签名错误。");
            }
        }

        /// <summary>
        /// 调用web service查询订单状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns>返回订单的状态（状态编号）</returns>
        private string InvokeGetOrderStateService(string cellPhone)
        {
            string orderInfos = string.Empty;
            try
            {
                WebRequest request = null;
                WebResponse response = null;
                string url = ConfigHelper.AdjOrderService + "/AidaijiaServer.asmx/getIsFirstOrder?cellPhone={0}";
                request = WebRequest.Create(string.Format(url, cellPhone));
                response = request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string strMsg = reader.ReadToEnd();
                    return strMsg;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("查询订单服务调用失败!");
            }
        }
    }
}
