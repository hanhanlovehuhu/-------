using Aidaijia.API.Common;
using Aidaijia.API.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Aidaijia.API.DAL
{
    public class OrderDal
    {
        private SqlHelper helper;

        public OrderDal()
        {
            helper = new SqlHelper("AiDaiJiaConStr");
        }
        public List<Order> GetOrderByOrderIds(string orderIds, out List<string> lastIds)
        {
            List<Order> result = new List<Order>();
            lastIds = new List<string>();
            try
            {
                string[] sIds = orderIds.Split(',');
                string newIds = "";

                for (int i = 0; i < sIds.Length; i++)
                {
                    newIds += "'" + sIds[i] + "',";
                    lastIds.Add(sIds[i].ToUpper());
                }
                newIds = newIds.Substring(0, newIds.Length - 1);
                string sSql = string.Format(@"
                   DECLARE @TotalPrice DECIMAL(18,2)=0,
		                   @PreferentialPrice DECIMAL(18,2)=0,
				           @AllMoney DECIMAL(18,2)=0,
				           @MileageMoney DECIMAL(18,2)=0,
				           @WaitMoney DECIMAL(18,2)=0,
				           @NewMileage FLOAT=0,
				           @NewWaitTime INT=0
				   SELECT TOP(1) @TotalPrice=TotalPrice,@PreferentialPrice=PreferentialPrice,@MileageMoney=NewMileagePrice,@WaitMoney=NewWaitPrice,@NewWaitTime=NewWaitTime,@NewMileage=NewMileage 
                   FROM dbo.D_PaymentRecord(NOLOCK) WHERE OrderId in({0}) ORDER BY InsertTime ASC
				   IF @TotalPrice-@PreferentialPrice>=0
				     SET @AllMoney=@TotalPrice-@PreferentialPrice
                       Select a.OrderId, OrderFrom, a.Address, AppointTime, OrderTime, a.BusinessType, FromCellphone, States,
                       CellPhone, a.Remark, a.Ucode, CustomerName, AllUserCount, a.Create_User, SendUserName, a.CarCode, 0 as Mileage,
                       0 as MileageMoney, 0 as WaitTime, 0 as WaitMoney, 0 as AllMoney, 0 as DiscountMoney, '' as driverPhone, 
                       di.DriverName, dod.BeginDriveTime, dod.EndDriveTime, dod.ArrivedTime
                       From D_CallCenterOrderInfo a with(nolock)
                       join D_DriverInfo di with(nolock) on a.Ucode=di.Ucode
                       join D_OrderDetail dod with(nolock) on a.OrderId=dod.OrderId
                       where states>41 and a.OrderId in({0})
                       union
                       Select d.OrderId, OrderFrom, BeginAddress, AppointmentTime, OrderTime, d.BusinessType, FromCellphone, d.State,
                       CellPhone, CustomerRemark, d.Ucode, CustomerName, 0, d.Create_User, '', '',
					   @NewMileage,
                       @MileageMoney,
					   @NewWaitTime,
					   @WaitMoney,
					   @AllMoney,
					   @PreferentialPrice,
                       case when d.state<30 then dd.phone else '' end as driverPhone, 
                       dd.DriverName, do.BeginDriveTime, do.EndDriveTime, do.ArrivedTime
                       From D_OrderInfo d with(nolock)
                       join D_OrderDetail do with(nolock) on d.OrderId=do.OrderId
                       join D_DriverInfo dd with(nolock) on d.Ucode=dd.Ucode 
                       where d.OrderId in({0})", newIds);               
                var dt = helper.GetDataTable(sSql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Order entity = new Order();
                        entity.OrderId = dt.Rows[0]["OrderId"] == DBNull.Value ? "" : dt.Rows[0]["OrderId"].ToString();
                        entity.Address = dt.Rows[0]["Address"] == DBNull.Value ? "" : dt.Rows[0]["Address"].ToString();
                        entity.AppointTime = dt.Rows[0]["AppointTime"] == DBNull.Value ? "" : Convert.ToDateTime(dt.Rows[0]["AppointTime"]).ToString("yyyy-MM-dd HH:mm");
                        entity.OrderTime = dt.Rows[0]["OrderTime"] == DBNull.Value ? "" : Convert.ToDateTime(dt.Rows[0]["OrderTime"]).ToString("yyyy-MM-dd HH:mm");
                        entity.AllUserCount = 1;
                        entity.BusinessType = Convert.ToInt32(dt.Rows[0]["BusinessType"] == DBNull.Value ? 0 : dt.Rows[0]["BusinessType"]);
                        entity.FromCellPhone = dt.Rows[0]["FromCellphone"] == DBNull.Value ? "" : dt.Rows[0]["FromCellphone"].ToString();
                        entity.Status = Convert.ToInt32(dt.Rows[0]["States"] == DBNull.Value ? 0 : dt.Rows[0]["States"]);
                        entity.CellPhone = dt.Rows[0]["CellPhone"] == DBNull.Value ? "" : dt.Rows[0]["CellPhone"].ToString();
                        entity.Remark = dt.Rows[0]["Remark"] == DBNull.Value ? "" : dt.Rows[0]["Remark"].ToString();
                        entity.Ucode = dt.Rows[0]["Ucode"] == DBNull.Value ? "" : dt.Rows[0]["Ucode"].ToString();
                        entity.Status = dt.Rows[0]["States"] == DBNull.Value ? 0 :Convert.ToInt32(dt.Rows[0]["States"]);
                        entity.AllMoney = dt.Rows[0]["AllMoney"] == DBNull.Value ? 0 : Convert.ToDecimal(dt.Rows[0]["AllMoney"]);
                        entity.MileageMoney = dt.Rows[0]["MileageMoney"] == DBNull.Value ? 0 : Convert.ToDecimal(dt.Rows[0]["MileageMoney"]);
                        entity.WaitMoney = dt.Rows[0]["WaitMoney"] == DBNull.Value ? 0 : Convert.ToDecimal(dt.Rows[0]["WaitMoney"]);
                        entity.WaitTime = dt.Rows[0]["WaitTime"] == DBNull.Value ? 0 : Convert.ToDouble(dt.Rows[0]["WaitTime"]);
                        entity.Mileage = dt.Rows[0]["Mileage"] == DBNull.Value ? 0 : Convert.ToDouble(dt.Rows[0]["Mileage"]);
                        entity.DiscountMoney = dt.Rows[0]["DiscountMoney"] == DBNull.Value ? 0 : Convert.ToDecimal(dt.Rows[0]["DiscountMoney"]);
                        entity.DriverPhone = dt.Rows[0]["driverPhone"] == DBNull.Value ? "" : dt.Rows[0]["driverPhone"].ToString();
                        entity.CustomerName = dt.Rows[0]["CustomerName"] == DBNull.Value ? "" : dt.Rows[0]["CustomerName"].ToString();

                        entity.DriverName = dt.Rows[0]["DriverName"] == DBNull.Value ? "" : dt.Rows[0]["DriverName"].ToString();
                        entity.BeginDriveTime = dt.Rows[0]["BeginDriveTime"] == DBNull.Value ? "" : Convert.ToDateTime(dt.Rows[0]["BeginDriveTime"]).ToString("yyyy-MM-dd HH:mm");
                        entity.EndDriveTime = dt.Rows[0]["EndDriveTime"] == DBNull.Value ? "" : Convert.ToDateTime(dt.Rows[0]["EndDriveTime"]).ToString("yyyy-MM-dd HH:mm");
                        entity.ArrivedTime = dt.Rows[0]["ArrivedTime"] == DBNull.Value ? "" : Convert.ToDateTime(dt.Rows[0]["ArrivedTime"]).ToString("yyyy-MM-dd HH:mm");
                        
                        result.Add(entity);
                        lastIds.Remove(entity.OrderId.ToUpper());
                    }                   
                }
            }
            catch (Exception ex)
            {
                LogControl.WriteError("GetOrderByOrderIds查询订单失败|Error:" + ex.Message);
            }
            return result;

        }

        public List<Order> GetOrderByCellPhone(string cellPhone)
        {
            List<Order> result = new List<Order>();
            try
            {              
                string   sSql = string.Format(@"Select OrderId,OrderFrom, Address,AppointTime,OrderTime,BusinessType,FromCellphone,States,
                      CellPhone,Remark,Ucode,CustomerName,AllUserCount,Create_User,SendUserName,CarCode From D_CallCenterOrderInfo(nolock) 
                  where (create_time between dateadd(hour,-12,getdate()) and getdate()) and  (FromCellPhone='{0}' or CellPhone='{0}')",
                      cellPhone);               
                var dt = helper.GetDataTable(sSql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Order entity = new Order();

                        entity.OrderId = dt.Rows[i]["OrderId"] == DBNull.Value ? "" : dt.Rows[i]["OrderId"].ToString();
                        entity.Address = dt.Rows[i]["Address"] == DBNull.Value ? "" : dt.Rows[i]["Address"].ToString();
                        entity.AppointTime = dt.Rows[i]["AppointTime"] == DBNull.Value ? "" : Convert.ToDateTime(dt.Rows[i]["AppointTime"]).ToString("yyyy-MM-dd HH:mm");
                        entity.OrderTime = dt.Rows[i]["OrderTime"] == DBNull.Value ? "" : Convert.ToDateTime(dt.Rows[i]["OrderTime"]).ToString("yyyy-MM-dd HH:mm");
                        entity.AllUserCount = dt.Rows[i]["AllUserCount"] == DBNull.Value ? 0 :Convert.ToInt32(dt.Rows[i]["AllUserCount"]);
                        entity.BusinessType = Convert.ToInt32(dt.Rows[i]["BusinessType"] == DBNull.Value ? 0 : dt.Rows[i]["BusinessType"]);
                        entity.FromCellPhone = dt.Rows[i]["FromCellphone"] == DBNull.Value ? "" : dt.Rows[i]["FromCellphone"].ToString();
                        int Status = Convert.ToInt32(dt.Rows[i]["States"] == DBNull.Value ? 0 : dt.Rows[i]["States"]);
                        switch (Status)
                        {
                                //接单
                            case 11:
                                entity.Status = 3;
                                break;
                            case 20:
                                entity.Status = 9;
                                break;
                            case 21:
                                entity.Status = 10;
                                break;
                            case 22:
                                entity.Status = 11;
                                break;
                            case 30:
                                entity.Status = 12;
                                break;
                                //用户取消
                            case 40:
                                entity.Status = 7;
                                break;
                            //第三方 司机取消
                            case 41:
                                entity.Status = 8;
                                break;
                             //客服取消
                            case 45:
                                entity.Status = 6;
                                break;
                                //第三方取消
                            case 46:
                                entity.Status = 9;
                                break;
                            default:
                                break;
                        }
                        entity.CellPhone = dt.Rows[i]["CellPhone"] == DBNull.Value ? "" : dt.Rows[i]["CellPhone"].ToString();
                        entity.Remark = dt.Rows[i]["Remark"] == DBNull.Value ? "" : dt.Rows[i]["Remark"].ToString();
                        entity.Ucode = dt.Rows[i]["Ucode"] == DBNull.Value ? "" : dt.Rows[i]["Ucode"].ToString();
                        entity.CustomerName = dt.Rows[i]["CustomerName"] == DBNull.Value ? "" : dt.Rows[i]["CustomerName"].ToString();
                        entity.OrderFrom = dt.Rows[i]["OrderFrom"] == DBNull.Value ? 0: Convert.ToInt32(dt.Rows[i]["OrderFrom"]);
                        entity.CreateUser = dt.Rows[i]["Create_User"] == DBNull.Value ? "" : dt.Rows[i]["Create_User"].ToString();
                        entity.SendUserName = dt.Rows[i]["SendUserName"] == DBNull.Value ? "" : dt.Rows[i]["SendUserName"].ToString();
                        entity.CarNumber = dt.Rows[i]["CarCode"] == DBNull.Value ? "" : dt.Rows[i]["CarCode"].ToString();
                        result.Add(entity); 
                    }
                   
                }
            }
            catch (Exception ex)
            {
                LogControl.WriteError("GetOrderByCellPhone查询订单失败|Error:" + ex.Message);
            }
            return result;
        }

        public OrderDriverEntity GetOrderByOrderId(string orderId, string ucode)
        {
            try
            {
                var paras = new SqlParameter[] {
                new SqlParameter("@OrderId",orderId),           
                 };
                DataTable dt = helper.GetDataTableStoredProcedure("gsp_GetStartDriverOrderInfo", paras);
                if (dt != null && dt.Rows.Count > 0)
                {
                    OrderDriverEntity entity = new OrderDriverEntity()
                    {
                        OrderId = dt.Rows[0]["OrderId"] == DBNull.Value ? "" : dt.Rows[0]["OrderId"].ToString(),
                        Ucode = dt.Rows[0]["Ucode"].ToString(),
                        CellPhone = dt.Rows[0]["CellPhone"].ToString(),
                        OrderTime = dt.Rows[0]["OrderTime"] == DBNull.Value ? "" : Convert.ToDateTime(dt.Rows[0]["OrderTime"]).ToString("yyyy-MM-dd HH:mm"),
                        ArrivedTime = dt.Rows[0]["ArrivedTime"] == DBNull.Value ? "" : Convert.ToDateTime(dt.Rows[0]["ArrivedTime"]).ToString("yyyy-MM-dd HH:mm"),
                        BeginTime = dt.Rows[0]["BeginDriveTime"] == DBNull.Value ? "" : Convert.ToDateTime(dt.Rows[0]["BeginDriveTime"]).ToString("yyyy-MM-dd HH:mm"),
                        VipPrice = Convert.ToDecimal(dt.Rows[0]["Amount"] == DBNull.Value ? 0 : dt.Rows[0]["Amount"]),
                        StartPrice = Convert.ToDecimal(dt.Rows[0]["StartPrice"]),
                        StartMileage = Convert.ToInt32(dt.Rows[0]["StartMileage"]),
                        UnitMileage = Convert.ToInt32(dt.Rows[0]["UnitMileage"]),
                        UnitPrice = Convert.ToDecimal(dt.Rows[0]["UnitPrice"]),
                        WaitUnitTime = Convert.ToInt32(dt.Rows[0]["WaitUnitTime"]),
                        WaitUnitPrice = Convert.ToDecimal(dt.Rows[0]["WaitUnitPrice"]),
                        IsFixed = Convert.ToBoolean(dt.Rows[0]["CustomerBasicType"] == DBNull.Value ? 0 : dt.Rows[0]["CustomerBasicType"]) ? 1 : 0,
                        FixedPrice = Convert.ToDecimal(dt.Rows[0]["CustomerBasicFee"] == DBNull.Value ? 0 : dt.Rows[0]["CustomerBasicFee"]),
                        Address = dt.Rows[0]["BeginAddress"] == DBNull.Value ? "" : dt.Rows[0]["BeginAddress"].ToString(),
                        AppointTime = dt.Rows[0]["AppointmentTime"] == DBNull.Value ? "" : Convert.ToDateTime(dt.Rows[0]["AppointmentTime"]).ToString("yyyy-MM-dd HH:mm"),
                        CusSubsidiesMoney = Convert.ToDecimal(dt.Rows[0]["CusSubsidiesMoney"]),
                        AppSubsidiesMoney = Convert.ToDecimal(dt.Rows[0]["AppSubsidiesMoney"]),
                        BusinessType = Convert.ToInt32(dt.Rows[0]["BusinessType"] == DBNull.Value ? 0 : dt.Rows[0]["BusinessType"]),
                        Lat = Convert.ToInt32(dt.Rows[0]["DriverLat"]),
                        Lng = Convert.ToInt32(dt.Rows[0]["DriverLng"]),
                        Remark = dt.Rows[0]["CustomerRemark"] == DBNull.Value ? "" : dt.Rows[0]["CustomerRemark"].ToString(),
                        Status = Convert.ToInt32(dt.Rows[0]["State"]),
                    };
                    return entity;
                }
            }
            catch (Exception ex)
            {
                LogControl.WriteError("GetOrderByOrderId 查询订单失败|Error:" + ex.Message);
            }
            return null;
        }

        public OrderDriverEntity GetUnFinishedOrderByUcode(string ucode)
        {
            try
            {
                var paras = new SqlParameter[] {
                new SqlParameter("@Ucode",ucode),           
                 };
                DataTable dt = helper.GetDataTableStoredProcedure("gsp_GetUnFinishedDriverOrderInfo", paras);
                if (dt != null && dt.Rows.Count > 0)
                {
                    OrderDriverEntity entity = new OrderDriverEntity()
                    {
                        OrderId = dt.Rows[0]["OrderId"] == DBNull.Value ? "" : dt.Rows[0]["OrderId"].ToString(),
                        Ucode = dt.Rows[0]["Ucode"].ToString(),
                        CellPhone = dt.Rows[0]["CellPhone"].ToString(),
                        OrderTime = dt.Rows[0]["OrderTime"] == DBNull.Value ? "" : Convert.ToDateTime(dt.Rows[0]["OrderTime"]).ToString("yyyy-MM-dd HH:mm"),
                        ArrivedTime = dt.Rows[0]["ArrivedTime"] == DBNull.Value ? "" : Convert.ToDateTime(dt.Rows[0]["ArrivedTime"]).ToString("yyyy-MM-dd HH:mm"),
                        BeginTime = dt.Rows[0]["BeginDriveTime"] == DBNull.Value ? "" : Convert.ToDateTime(dt.Rows[0]["BeginDriveTime"]).ToString("yyyy-MM-dd HH:mm"),
                        VipPrice = Convert.ToDecimal(dt.Rows[0]["Amount"] == DBNull.Value ? 0 : dt.Rows[0]["Amount"]),
                        StartPrice = Convert.ToDecimal(dt.Rows[0]["StartPrice"]),
                        StartMileage = Convert.ToInt32(dt.Rows[0]["StartMileage"]),
                        UnitMileage = Convert.ToInt32(dt.Rows[0]["UnitMileage"]),
                        UnitPrice = Convert.ToDecimal(dt.Rows[0]["UnitPrice"]),
                        WaitUnitTime = Convert.ToInt32(dt.Rows[0]["WaitUnitTime"]),
                        WaitUnitPrice = Convert.ToDecimal(dt.Rows[0]["WaitUnitPrice"]),
                        IsFixed = Convert.ToBoolean(dt.Rows[0]["CustomerBasicType"] == DBNull.Value ? 0 : dt.Rows[0]["CustomerBasicType"]) ? 1 : 0,
                        FixedPrice = Convert.ToDecimal(dt.Rows[0]["CustomerBasicFee"] == DBNull.Value ? 0 : dt.Rows[0]["CustomerBasicFee"]),
                        Address = dt.Rows[0]["BeginAddress"] == DBNull.Value ? "" : dt.Rows[0]["BeginAddress"].ToString(),
                        AppointTime = dt.Rows[0]["AppointmentTime"] == DBNull.Value ? "" : Convert.ToDateTime(dt.Rows[0]["AppointmentTime"]).ToString("yyyy-MM-dd HH:mm"),
                        CusSubsidiesMoney = Convert.ToDecimal(dt.Rows[0]["CusSubsidiesMoney"]),
                        AppSubsidiesMoney = Convert.ToDecimal(dt.Rows[0]["AppSubsidiesMoney"]),
                        BusinessType = Convert.ToInt32(dt.Rows[0]["BusinessType"] == DBNull.Value ? 0 : dt.Rows[0]["BusinessType"]),
                        Lat = Convert.ToInt32(dt.Rows[0]["Lat"]),
                        Lng = Convert.ToInt32(dt.Rows[0]["Lng"]),
                        Remark = dt.Rows[0]["CustomerRemark"] == DBNull.Value ? "" : dt.Rows[0]["CustomerRemark"].ToString(),
                        Status = Convert.ToInt32(dt.Rows[0]["State"]),
                    };
                    return entity;
                }
            }
            catch (Exception ex)
            {
                LogControl.WriteError("GetOrderByOrderId 查询订单失败|Error:" + ex.Message);
            }
            return null;
        }

        public List<Order> GetOrderByParterid(string parterid)
        {
            List<Order> result = new List<Order>();
            try
            {
                string sSql = string.Format(@"Select OrderId,Address,AppointTime,OrderTime,BusinessType,FromCellphone,States,CellPhone,Remark,Ucode where D.Parterid='{0}'", parterid);
                //插入订单
                var dt = helper.GetDataTable(sSql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Order entity = new Order();
                        entity.OrderId = dt.Rows[0]["OrderId"] == DBNull.Value ? "" : dt.Rows[0]["OrderId"].ToString();
                        entity.Address = dt.Rows[0]["Address"] == DBNull.Value ? "" : dt.Rows[0]["Address"].ToString();
                        entity.AppointTime = dt.Rows[0]["AppointTime"] == DBNull.Value ? "" : Convert.ToDateTime(dt.Rows[0]["AppointTime"]).ToString("yyyy-MM-dd HH:mm");
                        entity.OrderTime = dt.Rows[0]["OrderTime"] == DBNull.Value ? "" : Convert.ToDateTime(dt.Rows[0]["OrderTime"]).ToString("yyyy-MM-dd HH:mm");
                        entity.AllUserCount = 1;
                        entity.BusinessType = Convert.ToInt32(dt.Rows[0]["BusinessType"] == DBNull.Value ? 0 : dt.Rows[0]["BusinessType"]);
                        entity.FromCellPhone = dt.Rows[0]["FromCellphone"] == DBNull.Value ? "" : dt.Rows[0]["FromCellphone"].ToString();
                        entity.Status = Convert.ToInt32(dt.Rows[0]["States"] == DBNull.Value ? 0 : dt.Rows[0]["States"]);
                        entity.CellPhone = dt.Rows[0]["CellPhone"] == DBNull.Value ? "" : dt.Rows[0]["CellPhone"].ToString();
                        entity.Remark = dt.Rows[0]["Remark"] == DBNull.Value ? "" : dt.Rows[0]["Remark"].ToString();
                        entity.Ucode = dt.Rows[0]["Ucode"] == DBNull.Value ? "" : dt.Rows[0]["Ucode"].ToString();
                        result.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                LogControl.WriteError("GetOrderByParterid查询订单失败|Error:" + ex.Message);
            }
            return result;
        }

        public List<OrderStatus> GetOrderStatusByParterid(string parterid)
        {
            List<OrderStatus> result = new List<OrderStatus>();
            try
            {
                string sSql = string.Format("Select * From D_CallCenterOrderInfo (nolock) where Parterid='{0}'", parterid);
                //插入订单
                var dt = helper.GetDataTable(sSql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        OrderStatus entity = new OrderStatus();
                        entity.Status = dt.Rows[i]["State"].ToString();
                        entity.OrderId = dt.Rows[i]["OrderId"].ToString();
                        result.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                LogControl.WriteError("GetOrderStatusByParterid查询订单失败|Error:" + ex.Message);
            }
            return result;
        }

        public List<OrderStatus> GetOrderStatusByOrderIds(string orderIds, out List<string> lastIds)
        {
            List<OrderStatus> result = new List<OrderStatus>();
            lastIds = new List<string>();
            try
            {
                string[] sIds = orderIds.Split(',');
                string newIds = "";

                for (int i = 0; i < sIds.Length; i++)
                {
                    newIds += "'" + sIds[i] + "',";
                    lastIds.Add(sIds[i].ToUpper());
                }
                newIds = newIds.Substring(0, newIds.Length - 1);
                string sSql = string.Format(@"Select OrderId,States From D_CallCenterOrderInfo (nolock) where States>41 and OrderId in({0}) 
                                              union Select Orderid,State From D_OrderInfo(nolock) where OrderId in({0})", newIds);
                var dt = helper.GetDataTable(sSql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        OrderStatus entity = new OrderStatus();
                        entity.Status = dt.Rows[i]["States"].ToString();
                        entity.OrderId = dt.Rows[i]["OrderId"].ToString();
                        result.Add(entity);
                        lastIds.Remove(entity.OrderId.ToUpper());
                    }
                }
            }
            catch (Exception ex)
            {
                LogControl.WriteError("GetOrderStatusByOrderIds查询订单失败|Error:" + ex.Message);
            }
            return result;

        }

        public bool SendMessage(Message msg)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"insert into [dbo].[D_SendMsg] (
                  TypeId, ReceivePhone, MsgContent, State, Created,FromType,create_time, create_user)
                  values (@TypeId, @ReceivePhone, @MsgContent, @State, @Created,@FromType, getdate(), @create_user);");
                SqlParameter[] paras = new SqlParameter[] {
                    new SqlParameter("@TypeId",msg.TypeId),
                    new SqlParameter("@ReceivePhone",msg.ReceivePhone),
                    new SqlParameter("@MsgContent",msg.MsgContent),
                    new SqlParameter("@State",msg.State),
                    new SqlParameter("@Created",msg.Created),
                    new SqlParameter("@FromType",msg.FromType),
                     new SqlParameter("@create_user","socketApi"),
                };
                var value = helper.ExecuteCommand(strSql.ToString(), paras);
                if (value > 0)
                {
                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogControl.WriteError(string.Format("发送短信失败|Error:{0}", ex.Message));
                return false;
            }
        }
        public string ServiceLogin(string username, string pwd)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(@"select Id,RealName From [dbo].[T_Admin](nolock) where UserName='{0}' and PWD='{1}';", username, pwd);
                var dt = helper.GetDataTable(strSql.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["Id"].ToString() + "_" + dt.Rows[0]["RealName"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex)
            {
                LogControl.WriteError(string.Format("客服登录异常|Error:{0}", ex.Message));
                return "0";
            }
        }

        public bool InsertCallCenterOrderInfo(Order entity, out string error)
        {
            error = "";
            try
            {
                var paras = new SqlParameter[] { new SqlParameter("@OrderId",SqlDbType.NVarChar,30),           
              new SqlParameter("@Address", SqlDbType.NVarChar,200),            
              new SqlParameter("@AppointTime", SqlDbType.DateTime),           
              new SqlParameter("@BusinessType", SqlDbType.Int),    
              new SqlParameter("@Cellphone", SqlDbType.NVarChar,20),
              new SqlParameter("@FromCellphone", SqlDbType.NVarChar,20),
              new SqlParameter("@Lat", SqlDbType.Float),
              new SqlParameter("@Lng", SqlDbType.Float),             
              new SqlParameter("@OrderFrom", SqlDbType.Int),
              new SqlParameter("@Parterid", SqlDbType.Int),                 
              new SqlParameter("@Ucode", SqlDbType.NVarChar,20), 
              new SqlParameter("@States",SqlDbType.VarChar,50),
              new SqlParameter("@Remark", SqlDbType.NVarChar,400),   
              new SqlParameter("@CarCode", SqlDbType.VarChar,100),
              new SqlParameter("@CustomerName",SqlDbType.VarChar,100),
              new SqlParameter("@AllUserCount", SqlDbType.Int),
              new SqlParameter("@PhoneCallTime",SqlDbType.Int),
              new SqlParameter("@SendUserId",SqlDbType.Int),
              new SqlParameter("@SendUserName",SqlDbType.VarChar,50),             
              new SqlParameter("@CreateUser",SqlDbType.VarChar,50),
              new SqlParameter("@OrderTime",SqlDbType.VarChar,50),
              new SqlParameter("@ErrorMsg", SqlDbType.NVarChar,100)
            };
                paras[0].Value = entity.OrderId;
                paras[1].Value =string.IsNullOrEmpty(entity.Address)?"" : entity.Address;
                paras[2].Value = string.IsNullOrEmpty(entity.AppointTime) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm") : entity.AppointTime;
                paras[3].Value = entity.BusinessType;
                paras[4].Value =string.IsNullOrEmpty(entity.CellPhone) ? "" : entity.CellPhone;
                paras[5].Value =string.IsNullOrEmpty(entity.FromCellPhone)? "" : entity.FromCellPhone;
                paras[6].Value = entity.Lat;
                paras[7].Value = entity.Lng;              
                paras[8].Value = entity.OrderFrom;
                paras[9].Value = entity.Parterid;
                paras[10].Value =string.IsNullOrEmpty(entity.Ucode)?"":entity.Ucode;
                switch (entity.Status)
                {
                        //客服取消
                    case 6:
                        entity.Status = 45;
                        break;
                        //第三方取消
                    case 13:
                        entity.Status=46;
                        break;
                    default:
                        break;
                }
                paras[11].Value = entity.Status;  
                paras[12].Value =string.IsNullOrEmpty(entity.Remark)? "" : entity.Remark;
                paras[13].Value =string.IsNullOrEmpty(entity.CarNumber)? "" : entity.CarNumber;
                paras[14].Value =string.IsNullOrEmpty(entity.CustomerName) ? "" : entity.CustomerName;
                paras[15].Value = entity.AllUserCount;
                paras[16].Value = entity.PhoneCallTime;
                paras[17].Value = entity.SendUserId;
                paras[18].Value =string.IsNullOrEmpty(entity.SendUserName)?"":entity.SendUserName;
                paras[19].Value = string.IsNullOrEmpty(entity.CreateUser) ? "sysSocketApi" : entity.CreateUser;
                paras[20].Value = string.IsNullOrEmpty(entity.OrderTime) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm") : entity.OrderTime;
                paras[21].Direction = ParameterDirection.Output;
                //插入订单                 
                var value = helper.ExecuteCommandProc("gsp_InsertCallCenterOrderInfo", paras);
                if (value >= 0)
                {
                    return true;
                }
                else
                {
                    error = paras[21] == null ? "" : paras[21].Value.ToString();
                    return false;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                LogControl.WriteError("InsertOrder插入调度中心失败|Error:" + ex.Message);
                return false;
            }
        }

        public bool InsertCallRecords(CallRecords entity)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"
                  Insert Into [D_CallRecords]
                                       ([CallType] ,[CallAddress],[CallNo],[CalledNo],[Seat] ,[CustomerService],[RingTime],
                                        [StartTime],[EndTime],[CallTime],[SoundRecord],[InCallType])
                  Values(@CallType,@CallAddress,@CallNo,@CalledNo,@Seat,@CustomerService,@RingTime,@StartTime,@EndTime,@CallTime,@SoundRecord,@InCallType)               
                ");
                SqlParameter[] para = new SqlParameter[] {
                new SqlParameter("@CallType",entity.CallType),
                new SqlParameter("@CallAddress",entity.CallAddress),
                new SqlParameter("@CallNo",entity.CallNo),               
                new SqlParameter("@CalledNo",entity.CalledNo),
                new SqlParameter("@Seat",entity.Seat),
                new SqlParameter("@CustomerService",entity.CustomerService),
                new SqlParameter("@RingTime",entity.RingTime),
                new SqlParameter("@StartTime",entity.StartTime),
                new SqlParameter("@EndTime",entity.EndTime),
                new SqlParameter("@CallTime",entity.CallTime),
                new SqlParameter("@SoundRecord",entity.SoundRecord),
                 new SqlParameter("@InCallType",entity.InCallType)
                };
                var value = helper.ExecuteCommand(strSql.ToString(), para);
                return value > 0;
            }
            catch (Exception ex)
            {
                LogControl.WriteError(string.Format("插入电话记录失败|Error:{0}", ex.Message));
                return false;
            }
        }

        public int GetParterIdBySign(string sign)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(@"Select * from   [dbo].[T_Parter]  where sign='{0}';",sign);               
                var dt = helper.GetDataTable(strSql.ToString());
                if (dt != null && dt.Rows.Count>0)
                {
                    return dt.Rows[0]["Id"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[0]["Id"]);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                LogControl.WriteError(string.Format("GetParterIdBySign获取失败|Error:{0}", ex.Message));
                return 0;
            }
 
        }

        public bool InsertOrder(Order entity, out string error)
        {
            error = "";
            try
            {
              var paras = new SqlParameter[] { new SqlParameter("@OrderId",SqlDbType.NVarChar,30),           
              new SqlParameter("@Address", SqlDbType.NVarChar,200),            
              new SqlParameter("@AppointmentTime", SqlDbType.DateTime),           
              new SqlParameter("@BusinessType", SqlDbType.Int),    
              new SqlParameter("@Cellphone", SqlDbType.NVarChar,20),
              new SqlParameter("@FromCellphone", SqlDbType.NVarChar,20),
              new SqlParameter("@Lat", SqlDbType.Float),
              new SqlParameter("@Lng", SqlDbType.Float),
              new SqlParameter("@DriverLat", SqlDbType.Float),
              new SqlParameter("@DriverLng", SqlDbType.Float),
              new SqlParameter("@OrderFrom", SqlDbType.Int),
              new SqlParameter("@Parterid", SqlDbType.Int),            
              new SqlParameter("@Ucode", SqlDbType.NVarChar,20),
              new SqlParameter("@RecommendCode", SqlDbType.NVarChar,100),
              new SqlParameter("@CustomerRemark", SqlDbType.NVarChar,400),   
              new SqlParameter("@CarCode", SqlDbType.VarChar,20),
              new SqlParameter("@CustomerName",SqlDbType.VarChar,100),                      
              new SqlParameter("@AllUserCount", SqlDbType.Int),
              new SqlParameter("@PhoneCallTime",SqlDbType.Int),
              new SqlParameter("@SendUserId",SqlDbType.Int),
              new SqlParameter("@SendUserName",SqlDbType.VarChar,50),             
              new SqlParameter("@CreateUser",SqlDbType.VarChar,50),
              new SqlParameter("@OrderTime",SqlDbType.VarChar,50),
              new SqlParameter("@ErrorMsg", SqlDbType.NVarChar,100)
            };
                paras[0].Value = entity.OrderId;
                paras[1].Value = string.IsNullOrEmpty(entity.Address )? "" : entity.Address;
                paras[2].Value = string.IsNullOrEmpty(entity.AppointTime) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm") : entity.AppointTime;
                paras[3].Value = entity.BusinessType;
                paras[4].Value = string.IsNullOrEmpty(entity.CellPhone)? "" : entity.CellPhone;
                paras[5].Value = string.IsNullOrEmpty(entity.FromCellPhone) ? "" : entity.FromCellPhone;
                paras[6].Value = entity.Lat;
                paras[7].Value = entity.Lng;
                paras[8].Value = entity.DriverLat;
                paras[9].Value = entity.DriverLng;
                paras[10].Value = entity.OrderFrom;
                paras[11].Value = entity.Parterid;
                paras[12].Value = entity.Ucode;
                paras[13].Value = string.IsNullOrEmpty(entity.RecommendCode) ? "" : entity.RecommendCode;
                paras[14].Value = string.IsNullOrEmpty(entity.Remark)? "" : entity.Remark;
                paras[15].Value = string.IsNullOrEmpty(entity.CarNumber)? "" : entity.CarNumber;
                paras[16].Value = string.IsNullOrEmpty(entity.CustomerName)? "" : entity.CustomerName;
                paras[17].Value = entity.AllUserCount;
                paras[18].Value = entity.PhoneCallTime;
                paras[19].Value = entity.SendUserId;
                paras[20].Value = string.IsNullOrEmpty(entity.SendUserName)?"":entity.SendUserName;
                paras[21].Value = string.IsNullOrEmpty(entity.CreateUser)?"sysSocketApi":entity.CreateUser;
                paras[22].Value = string.IsNullOrEmpty(entity.OrderTime) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm") : entity.OrderTime;
                paras[23].Direction = ParameterDirection.Output;
                //插入订单                 
                var value = helper.ExecuteCommandProc("gsp_InsertOrder", paras);
                if (value >= 0)
                {
                    return true;
                }
                else
                {
                    error = paras[23] == null ? "" :paras[23].Value==null?"":paras[23].Value.ToString();
                    return false;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                LogControl.WriteError("InsertOrder插入订单失败|Error:" + ex.Message);
                return false;
            }

        }
      
        public List<CommentEntity> GetCommentOrderByUcode(string ucode,string cellPhone)
        {
            List<CommentEntity> result = new List<CommentEntity>();
            try
            {
                StringBuilder strSql = new StringBuilder();
                string sCellPhone = "";
                if (!string.IsNullOrEmpty(sCellPhone))
                {
                    sCellPhone =string.Format(" and CellPhone='{0}'",cellPhone);
                }
                strSql.AppendFormat(@"Select top 20 * from   [dbo].[D_DriverComment](nolock) 
                  where ucode='{0}' and Comment!='' {1} order by create_time desc;", ucode, sCellPhone);
                var dt = helper.GetDataTable(strSql.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        CommentEntity entity = new CommentEntity();                       
                        entity.CellPhone = dt.Rows[i]["Cellphone"]==DBNull.Value?"":dt.Rows[i]["Cellphone"].ToString();
                        entity.Comment = dt.Rows[i]["Comment"] == DBNull.Value ? "" : dt.Rows[i]["Comment"].ToString();
                        entity.CreateTime = dt.Rows[i]["create_time"] == DBNull.Value ? "" : Convert.ToDateTime(dt.Rows[i]["create_time"]).ToString("yyyy-MM-dd");
                        entity.Evaluate = dt.Rows[i]["Evaluate"] == DBNull.Value ? 0 :Convert.ToInt32(dt.Rows[i]["Evaluate"]);
                        entity.Ucode = dt.Rows[i]["ucode"] == DBNull.Value ? "" : dt.Rows[i]["ucode"].ToString();
                        result.Add(entity);
                    }
                }               
            }
            catch (Exception ex)
            {
                LogControl.WriteError(string.Format("{0}|GetCommentOrderByUcode获取失败|Error:{1}",DateTime.Now, ex.Message));               
            }
            return result;
        }

        public bool InsertComment(CommentEntity entity)
        {
            try
            {
                var paras = new SqlParameter[] { new SqlParameter("@Ucode",SqlDbType.NVarChar,30),           
              new SqlParameter("@Evaluate", SqlDbType.Int),            
              new SqlParameter("@CellPhone", SqlDbType.NVarChar,50),           
              new SqlParameter("@Comment", SqlDbType.NVarChar,2000),                    
              new SqlParameter("@CreateTime", SqlDbType.DateTime),
              new SqlParameter("@Status", SqlDbType.Int),
              new SqlParameter("@CreateUser", SqlDbType.NVarChar,20),           
                };
                paras[0].Value = entity.Ucode;
                paras[1].Value = entity.Evaluate;
                paras[2].Value = entity.CellPhone;
                paras[3].Value = entity.Comment;
                paras[4].Value = DateTime.Now;
                paras[5].Value = 1;
                paras[6].Value = "Aidaijia.API.CommentOrder.ashx";
                string sql = " Insert into D_DriverComment(Ucode,Evaluate,CellPhone,Comment,Create_Time,Create_User,Status)Values(@Ucode,@Evaluate,@CellPhone,@Comment,@CreateTime,@CreateUser,@Status)";
                //插入订单                 
                var value = helper.ExecuteCommand(sql, paras);
                if (value >= 0)
                {
                    return true;
                }
                else
                {                   
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogControl.WriteError("InsertComment插入评论失败|Error:" + ex.Message);
                return false;
            }
        }

        public OrderPrice GetOrderPrice(string orderId)
        {
           
            try
            {
                OrderPrice result = new OrderPrice();
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(@"Select O.SSMoney,O.MileageMoney,O.WaitMoney,O.WaitTime,O.State,D.Mileage 
                      from  D_OrderInfo(nolock) O join D_OrderDetail(nolock)D on O.OrderId=D.OrderId where O.OrderId='{0}'", orderId);
                var dt = helper.GetDataTable(strSql.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    result.SSMoney = dt.Rows[0]["SSMoney"] == DBNull.Value ? 0 :Convert.ToDouble(dt.Rows[0]["SSMoney"]);
                    result.MileageMoney = dt.Rows[0]["MileageMoney"] == DBNull.Value ? 0 : Convert.ToDouble(dt.Rows[0]["MileageMoney"]);
                    result.WaitMoney = dt.Rows[0]["WaitMoney"] == DBNull.Value ? 0 : Convert.ToDouble(dt.Rows[0]["WaitMoney"]);
                    result.WaitTime = dt.Rows[0]["WaitTime"] == DBNull.Value ? 0 : Convert.ToDouble(dt.Rows[0]["WaitTime"]);
                    result.State = dt.Rows[0]["State"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[0]["State"]);
                    result.Mileage = dt.Rows[0]["Mileage"] == DBNull.Value ? 0 : Convert.ToDouble(dt.Rows[0]["Mileage"]);                    
                }
                return result;
            }
            catch (Exception ex)
            {
                LogControl.WriteError(string.Format("{0}|GetOrderPrice获取失败|Error:{1}", DateTime.Now, ex.Message));
                return null;
            }
            
        }

        public List<CityPrice> GetCityPrice(string cityId)
        {

            try
            {
                List<CityPrice> result = new List<CityPrice>();
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(@"Select StartTime, EndTime, StartMileage, StartPrice, PMoney,DCD.UnitMileage, DCD.UnitPrice, DCD.WaitUnitTime, DCD.WaitUnitPrice 
                      from  D_CityPrice(nolock) DC join D_CityPriceDetail(nolock)DCD on DCd.CityPriceId=DC.CityPriceId 
                      where DC.NationalCityId='{0}'", cityId);
                var dt = helper.GetDataTable(strSql.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        CityPrice entity = new CityPrice();
                        entity.StartTime = dt.Rows[i]["StartTime"] == DBNull.Value ? "" : Convert.ToString(dt.Rows[i]["StartTime"]);
                        entity.EndTime = dt.Rows[i]["EndTime"] == DBNull.Value ? "" : Convert.ToString(dt.Rows[i]["EndTime"]);
                        entity.StartMileage = dt.Rows[i]["StartMileage"] == DBNull.Value ? 0 : Convert.ToDouble(dt.Rows[i]["StartMileage"]);
                        entity.StartPrice = dt.Rows[i]["StartPrice"] == DBNull.Value ? 0 : Convert.ToDouble(dt.Rows[i]["StartPrice"]);
                        entity.PMoney = dt.Rows[i]["PMoney"] == DBNull.Value ? 0 : Convert.ToDouble(dt.Rows[i]["PMoney"]);
                        entity.UnitMileage = dt.Rows[i]["UnitMileage"] == DBNull.Value ? 0 : Convert.ToDouble(dt.Rows[i]["UnitMileage"]);
                        entity.UnitPrice = dt.Rows[i]["UnitPrice"] == DBNull.Value ? 0 : Convert.ToDouble(dt.Rows[i]["UnitPrice"]);
                        entity.WaitUnitTime = dt.Rows[i]["WaitUnitTime"] == DBNull.Value ? 0 : Convert.ToDouble(dt.Rows[i]["WaitUnitTime"]);
                        entity.WaitUnitPrice = dt.Rows[i]["WaitUnitPrice"] == DBNull.Value ? 0 : Convert.ToDouble(dt.Rows[i]["WaitUnitPrice"]);  
                        result.Add(entity);
                    }                                    
                }
                return result;
            }
            catch (Exception ex)
            {
                LogControl.WriteError(string.Format("{0}|GetCityPrice获取失败|Error:{1}", DateTime.Now, ex.Message));
                return null;
            }

        }

        public OrderTimeLine GetOrderTimeLine(string OrderId)
        {
            OrderTimeLine result = new OrderTimeLine();
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(@"Select O.OrderTime,D.ArrivedTime,D.BeginDriveTime,D.EndDriveTime,O.State
                      from  D_OrderInfo(nolock) O join D_OrderDetail(nolock)D on O.OrderId=D.OrderId where O.OrderId='{0}'", OrderId);
                var dt = helper.GetDataTable(strSql.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    result.ArriveTime = dt.Rows[0]["ArrivedTime"] == DBNull.Value ? "" : Convert.ToDateTime(dt.Rows[0]["ArrivedTime"]).ToString("yyyy-MM-dd HH:mm");
                    result.OrderTime = dt.Rows[0]["OrderTime"] == DBNull.Value ? "" : Convert.ToDateTime(dt.Rows[0]["OrderTime"]).ToString("yyyy-MM-dd HH:mm");
                    result.BeginTime = dt.Rows[0]["BeginDriveTime"] == DBNull.Value ? "" : Convert.ToDateTime(dt.Rows[0]["BeginDriveTime"]).ToString("yyyy-MM-dd HH:mm");
                    result.EndTime = dt.Rows[0]["EndDriveTime"] == DBNull.Value ? "" : Convert.ToDateTime(dt.Rows[0]["EndDriveTime"]).ToString("yyyy-MM-dd HH:mm");
                    result.Status = dt.Rows[0]["State"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[0]["State"]);
                }
            }
            catch (Exception ex)
            {
                LogControl.WriteError(string.Format("{0}|GetOrderPrice获取失败|Error:{1}", DateTime.Now, ex.Message));
            }
            return result;
        }

        public HistoryOrderPage GetHistoryOrders(HistoryConditions conditions)
        {
            try
            {
                HistoryOrderPage result = new HistoryOrderPage();
                result.OrderList = new List<HistoryOrderEntity>();
                StringBuilder sWhere=new StringBuilder();
                if (!string.IsNullOrEmpty(conditions.OrderId))
                {
                    sWhere.AppendFormat(" and OrderId='{0}'", conditions.OrderId);
                }
                if (!string.IsNullOrEmpty(conditions.BeginTime))
                {
                    sWhere.AppendFormat(" and Create_Time>='{0}'",conditions.BeginTime);
                }
                if (conditions.BusinessType>0)
                {
                    sWhere.AppendFormat(" and BusinessType='{0}'", conditions.BusinessType);
                }
                if (!string.IsNullOrEmpty(conditions.CellPhone))
                {
                    sWhere.AppendFormat(" and CellPhone='{0}'", conditions.CellPhone);
                }
                if (!string.IsNullOrEmpty(conditions.EndTime))
                {
                    sWhere.AppendFormat(" and Create_Time<='{0}'", conditions.EndTime);
                }
                if (!string.IsNullOrEmpty(conditions.FromCellPhone))
                {
                    sWhere.AppendFormat(" and FromCellPhone='{0}'", conditions.FromCellPhone);
                }
                if (conditions.OrderFrom>0)
                {
                    sWhere.AppendFormat(" and OrderFrom='{0}'", conditions.OrderFrom);
                }
                if (conditions.Status>0)
                {
                    sWhere.AppendFormat(" and States='{0}'", conditions.Status);
                }
                if (!string.IsNullOrEmpty(conditions.Ucode))
                {
                    sWhere.AppendFormat(" and Ucode='{0}'", conditions.Ucode);
                }
                if (conditions.PageSize>50)
                {
                    conditions.PageSize = 50;
                }
                if (conditions.StartIndex<1)
                {
                    conditions.StartIndex = 1;
                }
                int pageStart = (conditions.StartIndex -1)* conditions.PageSize;
                int pageEnd = conditions.StartIndex * conditions.PageSize;
                string sqlCount =string.Format(@"Select count(1) as count
                      from D_CallCenterOrderInfo(nolock)
                      where 1=1 {0}", sWhere.ToString());
                 var dtCount= helper.GetDataTable(sqlCount);
                 if (dtCount != null && dtCount.Rows.Count > 0)
                 {
                     result.PageCount = dtCount.Rows[0][0] == DBNull.Value ? 0 : Convert.ToInt32(dtCount.Rows[0][0]);

                     StringBuilder strSql = new StringBuilder();
                     strSql.AppendFormat(@"Select * From (
                      Select Row_Number() over(order by Create_Time desc) as rowIndex, OrderId, OrderFrom, BusinessType,CustomerName,
                      CellPhone,FromCellPhone,Ucode,Address,
                    States,Create_Time,AppointTime,Remark
                      from D_CallCenterOrderInfo(nolock) 
                      where 1=1 {0})t where t.rowIndex between {1} and {2}", sWhere.ToString(), pageStart, pageEnd);
                     var dt = helper.GetDataTable(strSql.ToString());
                     if (dt != null && dt.Rows.Count > 0)
                     {
                         for (int i = 0; i < dt.Rows.Count; i++)
                         {
                             int State = dt.Rows[i]["States"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["States"]);
                             switch (State)
                             {
                                 //接单
                                 case 11:
                                     State = 3;
                                     break;
                                 case 20:
                                     State = 9;
                                     break;
                                 case 21:
                                     State = 10;
                                     break;
                                 case 22:
                                     State = 11;
                                     break;
                                 case 30:
                                     State = 12;
                                     break;
                                 //用户取消
                                 case 40:
                                     State = 7;
                                     break;
                                 // 司机取消
                                 case 41:
                                     State = 8;
                                     break;
                                 //客服取消
                                 case 45:
                                     State = 6;
                                     break;
                                 //第三方取消
                                 case 46:
                                     State = 13;
                                     break;
                             }
                             HistoryOrderEntity entity = new HistoryOrderEntity()
                             {
                                 AppointTime = dt.Rows[i]["AppointTime"] == DBNull.Value ? "" : Convert.ToDateTime(dt.Rows[i]["AppointTime"]).ToString("yyyy-MM-dd HH:mm"),
                                 Address = dt.Rows[i]["Address"] == DBNull.Value ? "" : Convert.ToString(dt.Rows[i]["Address"]),                               
                                 BusinessType = dt.Rows[i]["BusinessType"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["BusinessType"]),                              
                                 CellPhone = dt.Rows[i]["CellPhone"] == DBNull.Value ? "" : Convert.ToString(dt.Rows[i]["CellPhone"]),
                                 CreateTime = dt.Rows[i]["Create_Time"] == DBNull.Value ? "" : Convert.ToDateTime(dt.Rows[i]["Create_Time"]).ToString("yyyy-MM-dd HH:mm"),
                                 CustomerName = dt.Rows[i]["CustomerName"] == DBNull.Value ? "" : Convert.ToString(dt.Rows[i]["CustomerName"]),                               
                                 FromCellPhone = dt.Rows[i]["FromCellPhone"] == DBNull.Value ? "" : Convert.ToString(dt.Rows[i]["FromCellPhone"]),                               
                                 OrderFrom = dt.Rows[i]["OrderFrom"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["OrderFrom"]),
                                 OrderId = dt.Rows[i]["OrderId"] == DBNull.Value ? "" : Convert.ToString(dt.Rows[i]["OrderId"]),                                
                                 Status = State,
                                 Ucode = dt.Rows[i]["Ucode"] == DBNull.Value ? "" : Convert.ToString(dt.Rows[i]["Ucode"]),                               
                             };
                             result.OrderList.Add(entity);
                         }
                     }

                 }
                return result;
            }
            catch (Exception ex)
            {
                LogControl.WriteError(string.Format("{0}|GetHistoryOrders获取历史订单失败|Error:{1}", DateTime.Now, ex.Message));
                return null;
            }

        }


        public HistoryOrderDetail GetHistoryDetail( string orderId)
        {
            try
            {
                HistoryOrderDetail result = new HistoryOrderDetail();
              
                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendFormat(@"Select D.OrderId,OrderFrom,BusinessType,C.CustomerName,D.CellPhone,FromCellPhone,
                      Ucode,State,D.Create_Time,AppointMentTime,case when ISNULL(Amount,0)>0 THEN 1 ELSE 0 END AS AccountType,SSMoney,MileageMoney,WaitMoney,OtherMoney,
                      WaitTime,BeginDriveTime,EndDriveTime,Mileage,Money,BalanceAmount,CashAmount,BeginAddress,BeginDriveAddress,EndDriveAddress                       
                      from D_OrderInfo(nolock) D
                      Join D_OrderDetail(nolock) DD on D.OrderId=DD.OrderId
                      Join D_CustomerInfo(nolock) C on D.CellPhone=C.CellPhone
                      left Join D_OrderPayInfo DP on D.OrderId=DP.OrderId
                      where D.OrderId='{0}'", orderId);
                    var dt = helper.GetDataTable(strSql.ToString());
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            int State = dt.Rows[i]["State"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["State"]);
                            switch (State)
                            {
                                //接单
                                case 11:
                                    State = 3;
                                    break;
                                case 20:
                                    State = 9;
                                    break;
                                case 21:
                                    State = 10;
                                    break;
                                case 22:
                                    State = 11;
                                    break;
                                case 30:
                                    State = 12;
                                    break;
                                //用户取消
                                case 40:
                                    State = 7;
                                    break;
                                // 司机取消
                                case 41:
                                    State = 8;
                                    break;                              
                            }
                            HistoryOrderDetail entity = new HistoryOrderDetail()
                            {
                                AppointTime = dt.Rows[i]["AppointMentTime"] == DBNull.Value ? "" : Convert.ToDateTime(dt.Rows[i]["AppointMentTime"]).ToString("yyyy-MM-dd HH:mm"),
                                Address = dt.Rows[i]["BeginAddress"] == DBNull.Value ? "" : Convert.ToString(dt.Rows[i]["BeginAddress"]),
                                BeginDriveAddress = dt.Rows[i]["BeginDriveAddress"] == DBNull.Value ? "" : Convert.ToString(dt.Rows[i]["BeginDriveAddress"]),
                                EndDriveAddress = dt.Rows[i]["EndDriveAddress"] == DBNull.Value ? "" : Convert.ToString(dt.Rows[i]["EndDriveAddress"]),
                                BusinessType = dt.Rows[i]["BusinessType"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["BusinessType"]),
                                CellPhone = dt.Rows[i]["CellPhone"] == DBNull.Value ? "" : Convert.ToString(dt.Rows[i]["CellPhone"]),
                                CreateTime = dt.Rows[i]["Create_Time"] == DBNull.Value ? "" : Convert.ToDateTime(dt.Rows[i]["Create_Time"]).ToString("yyyy-MM-dd HH:mm"),
                                CustomerName = dt.Rows[i]["CustomerName"] == DBNull.Value ? "" : Convert.ToString(dt.Rows[i]["CustomerName"]),
                                FromCellPhone = dt.Rows[i]["FromCellPhone"] == DBNull.Value ? "" : Convert.ToString(dt.Rows[i]["FromCellPhone"]),
                                OrderFrom = dt.Rows[i]["OrderFrom"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["OrderFrom"]),
                                OrderId = dt.Rows[i]["OrderId"] == DBNull.Value ? "" : Convert.ToString(dt.Rows[i]["OrderId"]),
                                Status = State,
                                Ucode = dt.Rows[i]["Ucode"] == DBNull.Value ? "" : Convert.ToString(dt.Rows[i]["Ucode"]),
                                AccountType = dt.Rows[i]["AccountType"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["AccountType"]),
                                BalanceAmount = dt.Rows[i]["BalanceAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dt.Rows[i]["BalanceAmount"]),
                                BeginDriveTime = dt.Rows[i]["BeginDriveTime"] == DBNull.Value ? "" : Convert.ToDateTime(dt.Rows[i]["BeginDriveTime"]).ToString("yyyy-MM-dd HH:mm"),
                                CashAmount = dt.Rows[i]["CashAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dt.Rows[i]["CashAmount"]),
                                EndDriveTime = dt.Rows[i]["EndDriveTime"] == DBNull.Value ? "" : Convert.ToDateTime(dt.Rows[i]["EndDriveTime"]).ToString("yyyy-MM-dd HH:mm"),
                                Mileage = dt.Rows[i]["Mileage"] == DBNull.Value ? 0 : Convert.ToDouble(dt.Rows[i]["Mileage"]),
                                MileageMoney = dt.Rows[i]["MileageMoney"] == DBNull.Value ? 0 : Convert.ToDecimal(dt.Rows[i]["MileageMoney"]),
                                Money = dt.Rows[i]["Money"] == DBNull.Value ? 0 : Convert.ToDecimal(dt.Rows[i]["Money"]),
                                OtherMoney = dt.Rows[i]["OtherMoney"] == DBNull.Value ? 0 : Convert.ToDecimal(dt.Rows[i]["OtherMoney"]),
                                SSMoney = dt.Rows[i]["SSMoney"] == DBNull.Value ? 0 : Convert.ToDecimal(dt.Rows[i]["SSMoney"]),
                                WaitMoney = dt.Rows[i]["WaitMoney"] == DBNull.Value ? 0 : Convert.ToDecimal(dt.Rows[i]["WaitMoney"]),
                                WaitTime = dt.Rows[i]["WaitTime"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["WaitTime"]),                                
                            };
                            result = entity;
                        }

                }
                return result;
            }
            catch (Exception ex)
            {
                LogControl.WriteError(string.Format("{0}|GetHistoryDetail获取历史订单详情失败|Error:{1}", DateTime.Now, ex.Message));
                return null;
            }
        }


        public List<Html5UnfinishedOrder> GetUnFinishedOrderByCellPhone(string cellPhone)
        {
            List<Html5UnfinishedOrder> ret = new List<Html5UnfinishedOrder>();
            try
            {
                var paras = new SqlParameter[] {
                new SqlParameter("@CellPhone",cellPhone),           
                 };
                string sql = string.Format(@"Select OrderId,O.Ucode,CellPhone,OrderTime,FromCellPhone,AppointmentTime,O.State,Photo,Tel,DriverName,DriveCount,Deposit,Phone
                           From D_OrderInfo O join D_DriverInfo D on O.Ucode=D.Ucode where CellPhone='{0}' and O.State<30", cellPhone);

                DataTable dt = helper.GetDataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        int State = dt.Rows[i]["State"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["State"]);
                        switch (State)
                        {
                            //接单
                            case 11:
                                State = 3;
                                break;
                            case 20:
                                State = 9;
                                break;
                            case 21:
                                State = 10;
                                break;
                            case 22:
                                State = 11;
                                break;
                            case 30:
                                State = 12;
                                break;
                            //用户取消
                            case 40:
                                State = 7;
                                break;
                            // 司机取消
                            case 41:
                                State = 8;
                                break;
                        }
                        Html5UnfinishedOrder entity = new Html5UnfinishedOrder()
                        {
                            OrderId = dt.Rows[i]["OrderId"] == DBNull.Value ? "" : dt.Rows[i]["OrderId"].ToString(),
                            Ucode = dt.Rows[i]["Ucode"].ToString(),
                            CellPhone = dt.Rows[i]["CellPhone"].ToString(),
                            Deposit = Convert.ToDecimal(dt.Rows[i]["Deposit"]),
                            DriveCount = Convert.ToInt32(dt.Rows[i]["DriveCount"]),
                            DriverName = dt.Rows[i]["DriverName"] == DBNull.Value ? "" : dt.Rows[i]["DriverName"].ToString(),
                            AppointTime = dt.Rows[i]["AppointmentTime"] == DBNull.Value ? "" : Convert.ToDateTime(dt.Rows[i]["AppointmentTime"]).ToString("yyyy-MM-dd HH:mm"),
                            FromCellPhone = dt.Rows[i]["FromCellPhone"] == DBNull.Value ? "" : dt.Rows[i]["FromCellPhone"].ToString(),
                            Phone = dt.Rows[i]["Phone"] == DBNull.Value ? "" : dt.Rows[i]["Phone"].ToString(),
                            Photo = dt.Rows[i]["Photo"] == DBNull.Value ? "" : dt.Rows[i]["Photo"].ToString(),
                            Tel = dt.Rows[i]["Tel"] == DBNull.Value ? "" : dt.Rows[i]["Tel"].ToString(),
                            Status = State
                        };
                        ret.Add(entity);                       
                    }
                  
                }
            }
            catch (Exception ex)
            {
                LogControl.WriteError("GetUnFinishedOrderByCellPhone 查询订单失败|Error:" + ex.Message);
            }
            return ret;
        }


        public FavorableInfoEntity getFavorableInfo(string cellPhone, string id)
        {
            try
            {
                FavorableInfoEntity result = null;
                SqlParameter[] para = new SqlParameter[] { new SqlParameter("@CellPhone", cellPhone), new SqlParameter("@Id", id) };
                var dt = helper.GetDataTableStoredProcedure("gsp_GetFavorableInfo", para);
                if (dt != null && dt.Rows.Count > 0)
                {                  
                    result = new FavorableInfoEntity()
                    {
                        CouponCode = dt.Rows[0]["youhui"] == DBNull.Value ? "" : Convert.ToString(dt.Rows[0]["youhui"]),
                        ImgAlt = dt.Rows[0]["imgAlt"] == DBNull.Value ? "" : Convert.ToString(dt.Rows[0]["imgAlt"]),
                        ImgBackColor = dt.Rows[0]["imgBackColor"] == DBNull.Value ? "" : Convert.ToString(dt.Rows[0]["imgBackColor"]),
                        ImgPath = dt.Rows[0]["imgPath"] == DBNull.Value ? "" : Convert.ToString(dt.Rows[0]["imgPath"]),
                        Price = dt.Rows[0]["price"] == DBNull.Value ? 0 : Convert.ToDecimal(dt.Rows[0]["price"]),
                        UrlPath = dt.Rows[0]["urlPath"] == DBNull.Value ? "" : Convert.ToString(dt.Rows[0]["urlPath"])
                    };
                }
                return result;
            }
            catch (Exception )
            {
                LogControl.WriteError("getFavorableInfo获取优惠配置失败");
                return null;
            }

        }

        public FavorableInfoEntity getFavorableInfoById(string id)
        {
            try
            {
                FavorableInfoEntity result = null;
                SqlParameter[] para = new SqlParameter[] { new SqlParameter("@Id", id) };
                var dt = helper.GetDataTableStoredProcedure("gsp_GetFavorableInfoById", para);
                if (dt != null && dt.Rows.Count > 0)
                {                  
                    result = new FavorableInfoEntity()
                    {
                        CouponCode = "",
                        ImgAlt = dt.Rows[0]["imgAlt"] == DBNull.Value ? "" : Convert.ToString(dt.Rows[0]["imgAlt"]),
                        ImgBackColor = dt.Rows[0]["imgBackColor"] == DBNull.Value ? "" : Convert.ToString(dt.Rows[0]["imgBackColor"]),
                        ImgPath = dt.Rows[0]["imgPath"] == DBNull.Value ? "" : Convert.ToString(dt.Rows[0]["imgPath"]),
                        Price = 0,
                        UrlPath = dt.Rows[0]["urlPath"] == DBNull.Value ? "" : Convert.ToString(dt.Rows[0]["urlPath"])
                    };
                }
                return result;
            }
            catch (Exception )
            {
                LogControl.WriteError("getFavorableInfoById获取优惠配置失败");
                return null;
            }

        }

        public bool getIsFirstOrder(string cellPhone)
        {
            try
            {
                string sql =string.Format("select orderId from d_orderInfo where state=30 and cellPhone='{0}'",cellPhone);

                var dt = helper.GetDataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                LogControl.WriteError("getIsFirstOrder获取是否第一次下单失败");
                return false;
            }
        }

        public bool UpdateOrderStatus(string orderId,int status, out string msg)
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[] { new SqlParameter("@Status",SqlDbType.Int,2),
                new SqlParameter("@OrderId", SqlDbType.NVarChar,30),
                new SqlParameter("@BeginLat",SqlDbType.Float),
                new SqlParameter("@BeginLng",SqlDbType.Float),
               new SqlParameter("@ErrorMsg",SqlDbType.NVarChar,100)};
                paras[0].Value = status;
                paras[1].Value = orderId;
                paras[2].Value = 0;
                paras[3].Value = 0;
                paras[4].Direction = ParameterDirection.Output;
                msg = "";
                
                var value = helper.ExecuteCommandProc("gsp_UpdateStatus", paras);
                if (value > 0)
                {
                    return true;
                }
                else
                {
                    msg = paras[4] == null ? "" : paras[4].Value.ToString();
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogControl.WriteError("UpdateOrderStatus修改订单状态失败");
                msg = ex.Message;
                return false;
            }

        }
       
    }
}
