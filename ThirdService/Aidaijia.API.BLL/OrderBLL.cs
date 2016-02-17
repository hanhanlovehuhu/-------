
using Aidaijia.API.DAL;
using Aidaijia.API.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aidaijia.API.BLL
{
   public class OrderBLL
    {
       static OrderDal dal = new OrderDal();
       public static List<Order> GetOrderByOrderIds(string orderIds, out List<string> lastIds)
       {
           return dal.GetOrderByOrderIds(orderIds, out lastIds);
       }

       public static bool SendMessage(Message msg)
       {
           return dal.SendMessage(msg);
       }
       public static string ServiceLogin(string username, string pwd)
       {
           return dal.ServiceLogin(username,pwd);
       }

       public static List<OrderStatus> GetOrderStatusByOrderIds(string orderIds,out List<string> lastIds)
       {
           return dal.GetOrderStatusByOrderIds(orderIds,out lastIds);
       }
       public static List<OrderStatus> GetOrderStatusByParterid(string parterid)
       {
           return dal.GetOrderStatusByParterid(parterid);
       }

       public static List<Order> GetOrderByParterid(string parterid)
       {
           return dal.GetOrderByParterid(parterid);
       }
       public static bool InsertCallCenterOrderInfo(Order entity, out string error)
       {
          return dal.InsertCallCenterOrderInfo(entity,out error);
       }

       public static bool InsertCallRecords(CallRecords entity)
       {
           return dal.InsertCallRecords(entity);
       }
       public static OrderDriverEntity GetOrderByOrderId(string orderId, string ucode)
       {
           return dal.GetOrderByOrderId(orderId, ucode);
       }

       public static OrderDriverEntity GetUnFinishedOrderByUcode(string ucode)
       {
           return dal.GetUnFinishedOrderByUcode(ucode);
       }
       public static List<Order> GetOrderByCellPhone(string cellPhone)
       {
           return dal.GetOrderByCellPhone(cellPhone);
       }

       public static int GetParterIdBySign(string sign)
       {
           return dal.GetParterIdBySign(sign);
       }
       public static bool InsertOrder(Order entity, out string error)
       {
           return dal.InsertOrder(entity,out error);
       }
       public static List<CommentEntity> GetCommentOrderByUcode(string ucode,string cellPhone)
       {
           return dal.GetCommentOrderByUcode(ucode,cellPhone);
       }
       public static bool InsertComment(CommentEntity entity)
       {
           return dal.InsertComment(entity);
       }
       public static OrderPrice GetOrderPrice(string orderId)
       {
           return dal.GetOrderPrice(orderId);
       }

       public static OrderTimeLine GetOrderTimeLine(string orderId)
       {
           return dal.GetOrderTimeLine(orderId);
       }

       public static List<CityPrice> GetCityPrice(string cityId)
       {
           return dal.GetCityPrice(cityId);
       }
       public static HistoryOrderPage GetHistoryOrders(HistoryConditions conditions)
       {
           return dal.GetHistoryOrders(conditions);
       }

       public static HistoryOrderDetail GetHistoryDetail(string orderId)
       {
           return dal.GetHistoryDetail(orderId);
       }

       public static List<Html5UnfinishedOrder> GetUnFinishedOrderByCellPhone(string cellPhone)
       {
           return dal.GetUnFinishedOrderByCellPhone(cellPhone);
       }

       public static FavorableInfoEntity getFavorableInfo(string cellPhone, string id)
       {
           return dal.getFavorableInfo(cellPhone, id);
       }
       public static FavorableInfoEntity getFavorableInfoById(string id)
       {
           return dal.getFavorableInfoById(id);
       }

       public static bool getIsFirstOrder(string cellPhone)
       {
           return dal.getIsFirstOrder(cellPhone);
       }
       public static bool UpdateOrderStatus(string orderId, int status, out string msg)
       {
           return dal.UpdateOrderStatus(orderId,status,out msg);
       }
    }
}
