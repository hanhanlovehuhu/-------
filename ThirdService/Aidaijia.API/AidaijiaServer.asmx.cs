using Aidaijia.API.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json.Linq;
using ServiceStack.Redis;
using Newtonsoft.Json;
using System.Data;
using Aidaijia.API.BLL;
using System.Web.Security;
using System.Web.Configuration;
using System.Text;
using System.IO;
using System.Collections;
using Aidaijia.API.Common;
namespace Aidaijia.API
{
    /// <summary>
    /// AidaijiaOrder 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class AidaijiaServer : System.Web.Services.WebService
    {
        delegate string UpdateEventHandler(string Ucode, double Lat, double Lng);

        #region 创建订单
        /// <summary>
        /// 创建订单 传入json串
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [WebMethod]
        public string CreateOrder(string order)
        {
            try
            {
                string url = string.Format("http://{0}:{1}/api/createOrder", ProtocolConfig.APIServer, ProtocolConfig.APIPort);
                JSONHelper.ParseFormByJson<Order>(order);
                var response = HttpWebResponseUtility.CreatePostHttpResponse(url, order);
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                var ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                return JArray.Parse(ret).ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }
        #endregion

        #region 根据多个orderid从redis与数据库获取订单
        /// <summary>
        /// 根据订单编号查询订单信息
        /// </summary>
        /// <param name="orderIds"></param>
        /// <returns></returns>
        [WebMethod]
        public string GetOrderInfosByOrderIds(string orderIds)
        {
            try
            {
                List<Order> result = new List<Order>();
                List<string> lastIds = new List<string>();
                var sqlData = OrderBLL.GetOrderByOrderIds(orderIds, out lastIds);
                if (sqlData.Count > 0)
                {
                    result.AddRange(sqlData);
                }
                if (lastIds.Count > 0)
                {
                    string sIds = "";
                    for (int i = 0; i < lastIds.Count; i++)
                    {
                        sIds += lastIds[i] + ",";
                    }
                    sIds = sIds.Substring(0, sIds.Length - 1);
                    string url = string.Format("http://{0}:{1}/api/getOrderInfo", ProtocolConfig.APIServer, ProtocolConfig.APIPort);
                    IDictionary<string, string> paras = new Dictionary<string, string>();
                    paras.Add("orders", sIds);
                    var request = HttpWebResponseUtility.CreateGetHttpResponse(url, paras);
                    StreamReader sr = new StreamReader(request.GetResponseStream(), Encoding.UTF8);
                    var ret = sr.ReadToEnd();
                    sr.Close();
                    request.Close();
                    var statusList = JSONHelper.ParseFormByJson<List<Order>>(ret);
                    if (statusList.Count > 0)
                    {
                        result.AddRange(statusList);
                    }
                }
                result.ForEach(x =>
                {
                    double lat = x.Lat;
                    double lng = x.Lng;

                    CoordinateHelper.ScottToBaidu(ref lng, ref lat);
                    x.Lat = lat;
                    x.Lng = lng;
                });
                return JSONHelper.GetJSON<List<Order>>(result);
            }
            catch (Exception)
            {
                return "";
            }
        }
        #endregion

        #region 根据多个orderid从数据库获取订单
        /// <summary>
        /// 根据订单编号查询订单信息
        /// </summary>
        /// <param name="orderIds"></param>
        /// <returns></returns>
        [WebMethod]
        public void GetDBOrderInfosByOrderIds(string orderIds)
        {
            try
            {
                List<Order> result = new List<Order>();
                List<string> lastIds = new List<string>();
                var sqlData = OrderBLL.GetOrderByOrderIds(orderIds, out lastIds);
                if (sqlData.Count > 0)
                {
                    result.AddRange(sqlData);
                }
                Context.Response.Write(JSONHelper.GetJSON<List<Order>>(result));
            }
            catch (Exception)
            {
                Context.Response.Write("");
            }
        }

        #endregion

        #region 根据来电号码与客户号码获取12小时内订单信息
        [WebMethod]
        public void GetOrderInfosByCellPhone(string cellPhone)
        {
            try
            {
                List<Order> result = OrderBLL.GetOrderByCellPhone(cellPhone);
                Context.Response.Write(JSONHelper.GetJSON<List<Order>>(result));
            }
            catch (Exception)
            {
                Context.Response.Write("");
            }
        }
        #endregion

        #region 根据第三方编号从redis中获取订单信息
        /// <summary>
        /// 根据第三方编号查询订单信息
        /// </summary>
        /// <param name="parterid"></param>
        /// <returns></returns>
        [WebMethod]
        public string GetOrderInfosByParterid(string parterid)
        {
            try
            {
                List<Order> result = new List<Order>();
                var sqlData = OrderBLL.GetOrderByParterid(parterid);
                if (sqlData.Count > 0)
                {
                    result.AddRange(sqlData);
                }
                string url = string.Format("http://{0}:{1}/api/getOrderInfo", ProtocolConfig.APIServer, ProtocolConfig.APIPort);
                IDictionary<string, string> paras = new Dictionary<string, string>();
                paras.Add("parter", parterid);
                var request = HttpWebResponseUtility.CreateGetHttpResponse(url, paras);
                StreamReader sr = new StreamReader(request.GetResponseStream(), Encoding.UTF8);
                var ret = sr.ReadToEnd();
                sr.Close();
                request.Close();
                var statusList = JSONHelper.ParseFormByJson<List<Order>>(ret);
                if (statusList.Count > 0)
                {
                    result.AddRange(statusList);
                }
                return JSONHelper.GetJSON<List<Order>>(result);
            }
            catch (Exception)
            {
                return "";
            }


        }
        #endregion

        #region 根据多个orderid从redis中获取订单信息
        /// <summary>
        /// 根据订单编号查询订单状态
        /// </summary>
        /// <param name="orderIds"></param>
        /// <returns></returns>
        [WebMethod]
        public string GetOrderStatesByOrderIds(string orderIds)
        {
            try
            {
                List<OrderStatus> result = new List<OrderStatus>();
                List<string> lastIds = new List<string>();
                var sqlData = OrderBLL.GetOrderStatusByOrderIds(orderIds, out lastIds);
                if (sqlData.Count > 0)
                {
                    result.AddRange(sqlData);
                }
                if (lastIds.Count > 0)
                {
                    string sIds = "";
                    for (int i = 0; i < lastIds.Count; i++)
                    {
                        sIds += lastIds[i] + ",";
                    }
                    sIds = sIds.Substring(0, sIds.Length - 1);
                    string url = string.Format("http://{0}:{1}/api/getOrderStatus", ProtocolConfig.APIServer, ProtocolConfig.APIPort);
                    IDictionary<string, string> paras = new Dictionary<string, string>();
                    paras.Add("orders", sIds);
                    var request = HttpWebResponseUtility.CreateGetHttpResponse(url, paras);
                    StreamReader sr = new StreamReader(request.GetResponseStream(), Encoding.UTF8);
                    var ret = sr.ReadToEnd();
                    sr.Close();
                    request.Close();
                    var statusList = JSONHelper.ParseFormByJson<List<OrderStatus>>(ret);
                    if (statusList.Count > 0)
                    {
                        result.AddRange(statusList);
                    }
                }
                return JSONHelper.GetJSON<List<OrderStatus>>(result);
            }
            catch (Exception)
            {
                return "";
            }

        }
        #endregion

        #region 根据多个orderid从redis中获取订单状态
        /// <summary>
        /// 根据第三方编号查询订单状态
        /// </summary>
        /// <param name="parterid"></param>
        /// <returns></returns>

        [WebMethod]
        public string GetOrderStatesByParterid(string parterid)
        {
            try
            {
                List<OrderStatus> result = new List<OrderStatus>();
                var sqlData = OrderBLL.GetOrderStatusByParterid(parterid);
                if (sqlData.Count > 0)
                {
                    result.AddRange(sqlData);
                }
                string url = string.Format("http://{0}:{1}/api/getOrderStatus", ProtocolConfig.APIServer, ProtocolConfig.APIPort);
                IDictionary<string, string> paras = new Dictionary<string, string>();
                paras.Add("parter", parterid);
                var request = HttpWebResponseUtility.CreateGetHttpResponse(url, paras);
                StreamReader sr = new StreamReader(request.GetResponseStream(), Encoding.UTF8);
                var ret = sr.ReadToEnd();
                sr.Close();
                request.Close();
                var statusList = JSONHelper.ParseFormByJson<List<OrderStatus>>(ret);
                if (statusList.Count > 0)
                {
                    result.AddRange(statusList);
                }
                return JSONHelper.GetJSON<List<OrderStatus>>(result);
            }
            catch (Exception)
            {
                return "";
            }


        }
        #endregion

        #region 根据客户手机获取客户信息
        /// <summary>
        /// 根据用户id获取用户信息
        /// </summary>
        /// <param name="customerId"></param>
        [WebMethod]
        public void GetCustomerInfoByCellPhone(string cellPhone)
        {
            try
            {
                var entity = CustomerBLL.GetCustomerByCellPhone(cellPhone);
                Context.Response.Write(JSONHelper.GetJSON<Customer>(entity));

            }
            catch (Exception)
            {
                Context.Response.Write("");

            }

        }
        #endregion

        #region 根据司机工号获取司机信息
        /// <summary>
        /// 根据司机工号获取司机信息
        /// </summary>
        /// <param name="ucode"></param>

        [WebMethod]
        public void GetDriverInfo(string ucode)
        {
            try
            {
                var entity = DriverBLL.GetDriverByUcode(ucode);
                if (entity != null && !string.IsNullOrEmpty(entity.Ucode))
                {
                    Context.Response.Write(JSONHelper.GetJSON<Driver>(entity));
                }
                else
                {
                    Context.Response.Write("");
                }

            }
            catch (Exception)
            {
                Context.Response.Write("");

            }

        }
        #endregion

        #region 根据多个司机工号获取司机信息
        /// <summary>
        /// 根据司机工号获取司机信息列表
        /// </summary>
        /// <param name="ucodes">以逗号分隔</param>

        [WebMethod]
        public void GetDriverInfoByUcodes(string ucodes)
        {
            try
            {
                var list = DriverBLL.GetDriverByUcodes(ucodes);
                if (list.Count > 0)
                {
                    Context.Response.Write(JSONHelper.GetJSON<List<Driver>>(list));
                }
                else
                {
                    Context.Response.Write("");
                }
            }
            catch (Exception)
            {
                Context.Response.Write("");

            }

        }
        #endregion

        #region 根据城市ID获取司机数量
        [WebMethod]
        public void GetDriverCountByCityId(string cityId)
        {
            try
            {
                var entity = DriverBLL.GetDriverCountByCity(cityId);
                if (entity != null)
                {
                    Context.Response.Write(JSONHelper.GetJSON<DriverCount>(entity));
                }
                else
                {
                    Context.Response.Write("");
                }

            }
            catch (Exception)
            {
                Context.Response.Write("");

            }

        }
        #endregion

        #region 根据经纬度与范围取前N位司机信息(html5专用)
        [WebMethod]
        public void GetDriverInfoByRange(double lat, double lng, double range, int top = 50)
        {
            try
            {
                var list = DriverBLL.GetDriverInfoByRange(lat, lng, range, top);
                if (list.Count > 0)
                {
                    Context.Response.Write(JSONHelper.GetJSON<List<Driver>>(list));
                }
                else
                {
                    Context.Response.Write("");
                }
            }
            catch (Exception)
            {
                Context.Response.Write("");

            }

        }
        #endregion

        #region 根据经纬度与范围与车型取前100位司机
        [WebMethod]
        public void GetDriverByRange(double lat, double lng, double range, string driverCarType)
        {
            try
            {
                var list = DriverBLL.GetDriverByRange(lat, lng, range, driverCarType);
                if (list.Count > 0)
                {
                    Context.Response.Write(JSONHelper.GetJSON<List<DriverRange>>(list));
                }
                else
                {
                    Context.Response.Write("[]");
                }
            }
            catch (Exception)
            {
                Context.Response.Write("[]");
            }

        }

        #endregion

        #region 发送短信
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="msg"></param>

        [WebMethod]
        public void SendMessage(string msg)
        {
            try
            {
                var entity = JSONHelper.ParseFormByJson<Message>(msg);
                bool isSend = OrderBLL.SendMessage(entity);
                if (isSend)
                {
                    Context.Response.Write("{\"Status\":True}");
                }
                else
                {
                    Context.Response.Write("{\"Status\":False}");
                }
            }
            catch (Exception)
            {
                Context.Response.Write("{\"Status\":False}");
            }
        }
        #endregion

        #region 客服登录
        /// <summary>
        /// 客服登录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pwd"></param>

        [WebMethod]
        public void ServiceLogin(string username, string pwd)
        {
            try
            {
                pwd = FormsAuthentication.HashPasswordForStoringInConfigFile(pwd, FormsAuthPasswordFormat.MD5.ToString());
                var ret = OrderBLL.ServiceLogin(username, pwd);
                Context.Response.Write(ret);
            }
            catch (Exception)
            {
                Context.Response.Write("0");
            }

        }
        #endregion

        #region 从redis中取消订单,成功后插入调度表
        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        [WebMethod]
        public string CancelOrder(string orderid)
        {
            try
            {
                string error = "";

                bool isUpdate = OrderBLL.UpdateOrderStatus(orderid, 46, out error);
                if (!isUpdate)
                {
                    string url = string.Format("http://{0}:{1}/api/cancelOrder", ProtocolConfig.APIServer, ProtocolConfig.APIPort);
                    IDictionary<string, string> paras = new Dictionary<string, string>();
                    paras.Add("order", orderid);
                    var response = HttpWebResponseUtility.CreatePostHttpResponse(url, paras, null, "", Encoding.UTF8, null);
                    StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    var ret = sr.ReadToEnd();
                    sr.Close();
                    response.Close();
                    var status = JSONHelper.ParseFormByJson<CancelStatus>(ret);
                    if (status.success == "true")
                    {
                        string Error = "";
                        bool isInsert = OrderBLL.InsertCallCenterOrderInfo(status.order, out Error);
                        if (isInsert)
                        {
                            return "1";
                        }
                        else
                        {
                            return "0";
                        }
                    }
                    else
                    {
                        return "0";
                    }
                }
                else
                {
                    return "1";
                }

            }
            catch (Exception)
            {
                return "0";
            }
        }
        #endregion

        #region 插入通话记录
        /// <summary>
        /// 插入通话记录表
        /// </summary>
        /// <param name="callRecords"></param>
        [WebMethod]

        public void InsertCallRecords(string callRecords)
        {
            try
            {
                var entity = JSONHelper.ParseFormByJson<CallRecords>(callRecords);
                bool isInsert = OrderBLL.InsertCallRecords(entity);
                if (isInsert)
                {
                    Context.Response.Write("1");
                }
                else
                {
                    Context.Response.Write("0");
                }
            }
            catch (Exception)
            {

                Context.Response.Write("0");
            }
        }

        #endregion

        #region 插入问题记录

        /// <summary>
        /// 插入问题记录表
        /// </summary>
        /// <param name="problemRecord"></param>
        [WebMethod]

        public void InsertProblemRecord(string problemRecord)
        {
            try
            {
                var entity = JSONHelper.ParseFormByJson<ProblemRecord>(problemRecord);
                bool isInsert = CustomerBLL.InsertProblemRecord(entity);
                if (isInsert)
                {
                    Context.Response.Write("1");
                }
                else
                {
                    Context.Response.Write("0");
                }
            }
            catch (Exception)
            {

                Context.Response.Write("0");
            }
        }
        #endregion

        #region 根据订单号与司机工号获取未完成订单

        [WebMethod]

        public void GetDriverOrderInfoByOrderId(string orderId, string ucode)
        {
            var entity = new OrderDriver();
            try
            {
                entity.Result = OrderBLL.GetOrderByOrderId(orderId, ucode);
                if (entity.Result != null && !string.IsNullOrEmpty(entity.Result.OrderId))
                {
                    switch (entity.Result.Status)
                    {
                        case 11:
                            entity.Result.Status = 3;
                            break;
                        case 20:
                            entity.Result.Status = 9;
                            break;
                        case 21:
                            entity.Result.Status = 10;
                            break;
                        case 22:
                            entity.Result.Status = 11;
                            break;
                        case 30:
                            entity.Result.Status = 12;
                            break;
                        default:
                            break;
                    }
                    entity.Result.Error = "1";
                    entity.ErrorMsg = "";
                    entity.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                entity.IsSuccess = false;
                entity.ErrorMsg = ex.Message;
                entity.Result.Error = ex.Message;
            }
            entity.ErrorCode = 0;
            Context.Response.Write(JSONHelper.GetJSON<OrderDriver>(entity));
        }

        #endregion

        #region 根据司机工号获取未完成订单

        [WebMethod]

        public void GetDriverUnFinishedOrderByUcode(string ucode)
        {

            var entity = new OrderDriver();
            try
            {
                entity.Result = OrderBLL.GetUnFinishedOrderByUcode(ucode);
                if (entity.Result != null && !string.IsNullOrEmpty(entity.Result.OrderId))
                {
                    switch (entity.Result.Status)
                    {
                        case 11:
                            entity.Result.Status = 3;
                            break;
                        case 20:
                            entity.Result.Status = 9;
                            break;
                        case 21:
                            entity.Result.Status = 10;
                            break;
                        case 22:
                            entity.Result.Status = 11;
                            break;
                        default:
                            break;
                    }
                    entity.Result.Error = "1";
                    entity.ErrorMsg = "";
                    entity.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                entity.IsSuccess = false;
                entity.ErrorMsg = ex.Message;
                entity.Result.Error = ex.Message;
            }
            entity.ErrorCode = 0;
            Context.Response.Write(JSONHelper.GetJSON<OrderDriver>(entity));
        }
        #endregion

        #region 根据手机号号获取未完成订单(Html5专用)

        [WebMethod]

        public void GetUnFinishedOrderByCellPhone(string cellPhone)
        {
            string ret = "[]";
            try
            {
                var entity = OrderBLL.GetUnFinishedOrderByCellPhone(cellPhone);
                if (entity != null)
                {
                    ret = JSONHelper.GetJSON<List<Html5UnfinishedOrder>>(entity);
                }
            }
            catch (Exception ex)
            {
                LogControl.WriteError("根据电话获取未完成订单异常:" + ex.Message);
            }
            Context.Response.Write(ret);
        }
        #endregion

        #region 根据第三方sign获取第三方id

        [WebMethod]

        public void GetParterIdBySign(string sign)
        {
            try
            {
                Context.Response.Write(OrderBLL.GetParterIdBySign(sign));
            }
            catch (Exception)
            {
                Context.Response.Write(0);
            }
        }

        #endregion

        #region 获取优惠信息

        [WebMethod]

        public void getFavorable(string id, string cellPhone = "")
        {
            string ret = "{}";
            try
            {
                FavorableInfoEntity entity = new FavorableInfoEntity();
                if (!string.IsNullOrEmpty(cellPhone))
                {
                    entity = OrderBLL.getFavorableInfo(cellPhone, id);
                }
                else
                {
                    entity = OrderBLL.getFavorableInfoById(id);
                }
                if (entity != null)
                {
                    ret = JSONHelper.GetJSON<FavorableInfoEntity>(entity);
                }
            }
            catch (Exception ex)
            {
                LogControl.WriteError("根据Id获取优惠信息失败:" + ex.Message);
            }
            Context.Response.Write(ret);
        }
        #endregion

        #region 获取是否第一次使用爱代驾

        [WebMethod]

        public void getIsFirstOrder(string cellPhone = "")
        {
            string ret = "0";
            try
            {
                bool isFirst = false;
                if (!string.IsNullOrEmpty(cellPhone))
                {
                    isFirst = OrderBLL.getIsFirstOrder(cellPhone);
                }
                if (isFirst)
                {
                    ret = "1";
                }
            }
            catch (Exception ex)
            {
                LogControl.WriteError("获取是否第一次使用爱代驾失败:" + ex.Message);
            }
            Context.Response.Write(ret);
        }
        #endregion

        #region 更新司机经纬度
        [WebMethod]
        public void UpdateDriverLatLng(string Ucode, double Lat, double Lng)
        {
            try
            {
                var entity = new DriverLatLng();
                try
                {
                    // bool ret = DriverLatLngBLL.UpdateDriverLatLng(Ucode, Lat, Lng);
                    bool ret = true;
                    if (ret)
                    {
                        entity.IsSuccess = true;
                        entity.ErrorMsg = "";
                        entity.ErrorCode = 0;
                    }
                    else
                    {
                        entity.IsSuccess = false;
                        entity.ErrorMsg = "更新失败";
                        entity.ErrorCode = 0;
                    }
                }
                catch (Exception ex)
                {
                    entity.IsSuccess = false;
                    entity.ErrorMsg = ex.Message;
                    entity.ErrorCode = 0;
                }
                Context.Response.Write(JSONHelper.GetJSON<DriverLatLng>(entity));
            }
            catch (Exception ex)
            {
                LogControl.WriteError("UpdateDriverLatLng Error:" + ex.Message);
            }

        }
        #endregion

    }
}
