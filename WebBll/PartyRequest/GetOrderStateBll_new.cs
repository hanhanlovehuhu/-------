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
    class GetOrderStateBll_new : JsonCommand
    {
        /// <summary>
        /// 返回订单状态编号
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string execute(string request)
        {
            //1210|1订单号|2合作商标识|3MD5串-MD5（订单号+aidaijia）
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
        /// <param name="orderId"></param>
        /// <returns>返回订单的状态（状态编号）</returns>
        private string InvokeGetOrderStateService(string orderId)
        {
            string orderState = string.Empty;
            try
            {
                object[] args = new object[1];
                args[0] = orderId;
                string orderService = ConfigHelper.AdjOrderService;
                var obj = DlWebService.InvokeWebService(orderService + "/AidaijiaServer.asmx"
                    , "GetOrderStatesByOrderIds"
                    , args);

                GetOrderStateResponseEntity[] orderStateResponse = new GetOrderStateResponseEntity[]{};
                orderStateResponse = JsonHelper.GetObject<GetOrderStateResponseEntity[]>(obj.ToString());
                if (orderStateResponse[0].Status == null)
                {
                    throw new Exception();
                }

                orderState = JsonConvert.SerializeObject(new { status = orderStateResponse[0].Status });
            }
            catch(Exception ex)
            {
                throw new Exception("查询订单服务调用失败!");
            }
            return orderState;
        }

        class GetOrderStateResponseEntity
        {
            public string OrderId { get; set; }             //订单号
            public string Status { get; set; }           //订单状态
        }
    }
}
