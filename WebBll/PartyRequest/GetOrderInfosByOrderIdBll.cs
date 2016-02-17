using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDal;
using WebModel;
using WebUtility;

namespace WebBll.PartyRequest
{
    class GetOrderInfosByOrderIdBll : JsonCommand
    {
        /// <summary>
        /// 按合作方查询订单信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string execute(string request)
        {
            //1213|1orderid|2合作商标识|3IMEI串|4MD5（订单号+aidaijia）
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
        /// <returns>返回订单的状态（文字描述，非状态编号）</returns>
        private string InvokeGetOrderStateService(string orderId)
        {
            string orderInfos = string.Empty;

            try
            {
                object[] args = new object[1];
                args[0] = orderId;
                string orderService = ConfigHelper.AdjOrderService;
                object obj = DlWebService.InvokeWebService(orderService + "/AidaijiaServer.asmx"
                    , "GetOrderInfosByOrderIds"
                    , args);

                orderInfos = obj.ToString();
            }
            catch
            {
                throw new Exception("查询订单服务调用失败!");
            }
            return orderInfos;
        }
    }
}
