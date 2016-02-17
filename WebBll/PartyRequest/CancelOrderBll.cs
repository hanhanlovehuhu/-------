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
    class CancelOrderBll : JsonCommand
    {
        //1209|1订单号|2sign|3IMEI|4MD5串
        public string execute(string request)
        {
            string[] req = request.Split('|');
            var sign = EncodingUtil.GetMd5(req[1] + "aidaijia", "utf-8");
            
            if (sign.ToLower() == req[3].ToLower())
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
        /// <param name="orderId">订单编号</param>
        /// <returns>返回订单的状态（状态编号）</returns>
        private string InvokeGetOrderStateService(string orderId)
        {
            string result = string.Empty;
            try
            {
                object[] args = new object[1];
                args[0] = orderId;
                string orderService = ConfigHelper.AdjOrderService;
                var obj = DlWebService.InvokeWebService(orderService + "/AidaijiaServer.asmx"
                    , "CancelOrder"
                    , args);
                if (obj == null)
                {
                    result = "0";
                }
                else
                {
                    result = obj as string;
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception("查询订单服务调用失败!");
            }
            return result;
        }
    }
}
