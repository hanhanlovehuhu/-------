using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebDal;
using WebModel;
using WebUtility;

namespace WebBll.PartyRequest
{
    class GetOrderStateBll : JsonCommand
    {
        public string execute(string request)
        {
            //1208|1订单号|2合作商标识|3MD5串-MD5（订单号+aidaijia）
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
        /// <returns>返回订单的状态（文字描述，非状态编号）</returns>
        private string InvokeGetOrderStateService(string orderId)
        {
            string orderState = string.Empty;

            try
            {
                object[] args = new object[1];
                args[0] = orderId;
                string orderService = ConfigHelper.AdjOrderService;
                object obj = DlWebService.InvokeWebService(orderService + "/AidaijiaServer.asmx"
                    , "GetOrderStatesByOrderIds"
                    , args);

                GetOrderStateResponseEntity[] orderStateResponse = null;
                orderStateResponse = JsonHelper.GetObject<GetOrderStateResponseEntity[]>(obj.ToString());
                if (orderStateResponse[0].Status == null )
                {
                    throw new Exception();
                }

                orderState = ConvertOrderState(orderStateResponse[0].Status);
            }
            catch
            {
                throw new Exception("查询订单服务调用失败!");
            }
            return orderState;
        }

        /// <summary>
        /// 转换StateType Code为文字状态的描述
        ///     10,订单已提交;11,司机已接单;
        ///     20,司机已到达;21,开始代驾;22,结束代驾;23,开始等待
        ///     30,订单已完成;
        ///     40,客户取消订单;41，司机订单取消;45，客服订单取消;
        ///     46,第三方取消订单
        /// </summary>
        /// <param name="stateCode"></param>
        /// <returns></returns>
        private string ConvertOrderState(string stateCode)
        {
            string stateDes = string.Empty;
            switch (stateCode)
            {
                case "1":
                    stateDes = "订单未分配";
                    break;
                case "4":
                    stateDes = "司机无响应";
                    break;
                case "5":
                    stateDes = "无司机可派";
                    break;
                case "10":
                    stateDes = "订单已提交";
                    break;
                case "11":
                    stateDes = "司机已接单";
                    break;
                case "20":
                    stateDes = "司机已到达";
                    break;
                case "21":
                    stateDes = "开始代驾";
                    break;
                case "22":
                    stateDes = "结束代驾";
                    break;
                case "23":
                    stateDes = "开始等待";
                    break;
                case "30":
                    stateDes = "订单已完成";
                    break;
                case "40":
                    stateDes = "客户取消订单";
                    break;
                case "41":
                    stateDes = "司机订单取消";
                    break;
                case "45":
                    stateDes = "客服订单取消";
                    break;
                case "46":
                    stateDes = "第三方取消订单";
                    break;
                default:
                    stateDes = "未知";
                    break;
            }
            return stateDes;
        }

        class GetOrderStateResponseEntity
        {
            public string Status { get; set; }          //订单状态码
            public string OrderId { get; set; }         // 订单号
        }
    }
}
