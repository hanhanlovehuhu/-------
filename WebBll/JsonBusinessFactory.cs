using System;
using System.Runtime.Remoting.Contexts;
using System.Web;
using WebBll;
using WebBll.PartyRequest;

namespace WebBll
{
    public class JsonBusinessFactory
    {
        public static string Execute(string request)
        {
            JsonCommand Cmd = null;
            string[] req = request.Split('|');

            switch (req[0])
            {

                case "0001":
                    Cmd = new VerJsonBll();   //版本升级
                    break;
                //合作商户协议
                //获取周边司机
                case "1201":
                     Cmd = new GetNearSjListBll();              
                    break;
                //预约司机
                case "1202": 
                     //Cmd = new YuyueRequestBll();                //预约
                    Cmd = new YuyueRequestServiceBll();                //预约
                    break;
                //上传通话记录
                case "1203":
                    Cmd = new ParterCallRequestBll();          //针对现有的设计，不再适用，服务号无作用
                    break;
                //评价服务
                case "1204":
                     Cmd = new PingJiaBll();                    //针对现有的设计，不再适用，服务号无作用
                    break;
                case "1205":
                    Cmd = new GetCityPriceBll();
                    break;
                case "1206":
                    //Cmd = new CreateOrderByUcodeBll();       //新增   //立即预约 预约指定司机
                    Cmd = new CreateOrderByUcodeServiceBll();       //新增   //立即预约 预约指定司机
                    break;
                case "1207":
                    Cmd = new GetPingjiaBll();                //新增    //获取评论
                    break;
                case "1208":
                    Cmd = new GetOrderStateBll();                //新增    //查询订单状态
                    break;
                case "1209":
                    Cmd = new CancelOrderBll(); //2014-8-6 sxy新增取消订单
                    break;
                case "1210":
                    Cmd = new GetOrderStateBll_new(); //2014-8-6 sxy新增获得订单状态数字
                    break;
                case "1211":
                    Cmd = new YuyueRequestServiceBll_new(); //2014-8-6 sxy新增预约订单返回订单号
                    break;
                case "1212":
                    Cmd = new CreateOrderByUcodeServiceBll_new(); //2014-8-6 sxy新增立即预约订单返回订单号
                    break;
                case "1213":
                    Cmd = new GetOrderInfosByOrderIdBll(); //2014-8-7 sxy新增按合作方查询订单
                    break;
                case "1214":
                    Cmd = new ComplainBll(); //2015-1-12 sxy新增投诉接口
                    break;
                case "1215":
                    Cmd = new OrderHistoryBll(); //2015-1-30 sxy新增订单历史接口
                    break;
                case "1216":
                    Cmd = new DriverInfoBll(); //2015-1-30 sxy新增司机信息接口
                    break;
                case "1217":
                    Cmd = new CheckNewCustomer(); //2015-2-2 sxy新增查询是否新用户
                    break;
                //case "1217":
                //    Cmd = new CreateOrderByUcodeService_OnlyCheLun(); //2015-1-30 sxy新增首单优惠接口
                //    break;
                //case "1218":
                //    Cmd = new YuyueRequest_OnlyCheLun(); //2015-1-30 sxy新增首单优惠接口
                //    break;
                case "1219":
                    Cmd = new CreateBusinessOrderBll(); //2015-5-4 ytb新增创建商务订单
                    break;
                case "1220":
                    Cmd = new CustomerRecharge();//2015-7-10 ytb新增第三方调用支付接口
                    break;
                default:
                    throw new Exception("找不到该服务");
            }
            return Cmd.execute(request);
        }
    }

}