using Aidaijia.API.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aidaijia.API.Entity;
namespace Aidaijia.API
{

    /// <summary>
    /// HTML5专用获取订单价格，城市价格，订单时间线
    /// </summary>
    public class OrderServer : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (!string.IsNullOrEmpty(context.Request.QueryString["method"]))
            {
                switch (context.Request.QueryString["method"])
                {
                    case "GetOrderPrice":
                        GetOrderPrice(context);
                        break;
                    case "GetCityPrice":
                        GetCityPrice(context);
                        break;
                    case "GetOrderTimeLine":
                        GetOrderTimeLine(context);
                        break;
                    case "GetHistoryOrder":
                        GetHistoryOrder(context);
                        break;
                    case "GetHistoryOrderDetail":
                        GetHistoryOrderDetail(context);
                        break;
                    default:
                        break;
                }
            }           
        }

        private void GetOrderPrice(HttpContext context)
        {
            try
            {
                if (!string.IsNullOrEmpty(context.Request.QueryString["orderId"]))
                {
                    var entity = OrderBLL.GetOrderPrice(context.Request.QueryString["orderId"]);
                    if (entity != null)
                    {
                        context.Response.Write(JSONHelper.GetJSON<OrderPrice>(entity));
                    }
                    else
                    {
                        context.Response.Write("");
                    }
                }          
            }
            catch (Exception)
            {
                context.Response.Write("");
            }           
        }
        private void GetOrderTimeLine(HttpContext context)
        {
            try
            {
                if (!string.IsNullOrEmpty(context.Request.QueryString["orderId"]))
                {
                    var entity = OrderBLL.GetOrderTimeLine(context.Request.QueryString["orderId"]);
                    if (entity != null)
                    {
                        context.Response.Write(JSONHelper.GetJSON<OrderTimeLine>(entity));
                    }
                    else
                    {
                        context.Response.Write("");
                    }
                }
            }
            catch (Exception)
            {
                context.Response.Write("");
            }       
        }

        private void GetCityPrice(HttpContext context)
        {
            try
            {
                if (!string.IsNullOrEmpty(context.Request.QueryString["cityId"]))
                {
                    var List = OrderBLL.GetCityPrice(context.Request.QueryString["cityId"]);
                    if (List != null && List.Count>0)
                    {
                        context.Response.Write(JSONHelper.GetJSON<List<CityPrice>>(List));
                    }
                    else
                    {
                        context.Response.Write("[]");
                    }
                }
            }
            catch (Exception)
            {
                context.Response.Write("[]");
            }
        }

        private void GetHistoryOrder(HttpContext context)
        {
            try
            {
                if (!string.IsNullOrEmpty(context.Request.QueryString["conditions"]))
                {
                    var entity = JSONHelper.ParseFormByJson<HistoryConditions>(context.Request.QueryString["conditions"]);
                    if (entity!=null)
                    {
                        var List = OrderBLL.GetHistoryOrders(entity);
                        if (List != null && List.PageCount > 0)
                        {
                            context.Response.Write(JSONHelper.GetJSON<HistoryOrderPage>(List));
                        }
                        else
                        {
                            context.Response.Write("[]");
                        }
                    }                  
                }
            }
            catch (Exception)
            {
                context.Response.Write("[]");
            }
        }
        private void GetHistoryOrderDetail(HttpContext context)
        {
            try
            {
                if (!string.IsNullOrEmpty(context.Request.QueryString["orderId"]))
                {
                    var entity = OrderBLL.GetHistoryDetail(context.Request.QueryString["orderId"]);
                    if (entity != null)
                    {
                        context.Response.Write(JSONHelper.GetJSON<HistoryOrderDetail>(entity));
                    }
                    else
                    {
                        context.Response.Write("");
                    }
                }
            }
            catch (Exception)
            {
                context.Response.Write("");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}