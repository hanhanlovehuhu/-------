using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using WebModel;

namespace WebDal
{
    public class D_AppointmentOrderInfoDA
    {
        /// <summary>
        /// 添加预约
        /// </summary>
        /// <param name="model">预约实体</param>
        /// <returns></returns>
        public bool AddAppointmentOrderInfo(AppointmentOrderInfoEntity model)
        {
            AdjDBObject adjDbObject = new AdjDBObject();
            bool isok = true;
            try
            {
                adjDbObject.GetStoredProcCommand("sp3_D_AppointmentOrderInfo_i");
                adjDbObject.AddOutParameter("@Id", DbType.Int32, 4);
                adjDbObject.AddInParameter("@AppointOrderId", DbType.String, model.AppointOrderId);
                adjDbObject.AddInParameter("@OrderId", DbType.String, model.OrderId);
                adjDbObject.AddInParameter("@AppointOrderType", DbType.Int32, model.AppointOrderType);
                adjDbObject.AddInParameter("@AppointOrderState", DbType.Int32, model.AppointOrderState);
                adjDbObject.AddInParameter("@BusinessType", DbType.Int32, model.BusinessType);
                adjDbObject.AddInParameter("@AppointOrderFrom", DbType.Int32, model.AppointOrderFrom);
                adjDbObject.AddInParameter("@CustomerId", DbType.String, model.CustomerId);
                adjDbObject.AddInParameter("@CustomerName", DbType.String, model.CustomerName);
                adjDbObject.AddInParameter("@CustomerType", DbType.Int32, model.CustomerType);
                adjDbObject.AddInParameter("@AccountType", DbType.Int32, model.AccountType);
                adjDbObject.AddInParameter("@Cellphone", DbType.String, model.Cellphone);
                adjDbObject.AddInParameter("@FromCellphone", DbType.String, model.FromCellphone);
                adjDbObject.AddInParameter("@AppointAddress", DbType.String, model.AppointAddress);
                adjDbObject.AddInParameter("@AppointTimeStr", DbType.String, model.AppointTimeStr);
                adjDbObject.AddInParameter("@AppointTime", DbType.DateTime, model.AppointTime);
                //adjDbObject.AddInParameter("@EstimateEndTime", DbType.DateTime, model.EstimateEndTime);
                adjDbObject.AddInParameter("@IsLock", DbType.Boolean, false);
                adjDbObject.AddInParameter("@LockUser", DbType.String, model.LockUser);
                adjDbObject.AddInParameter("@partyId", DbType.Int32, model.PartyId);
                adjDbObject.AddInParameter("@SendOrderUser", DbType.String, model.SendOrderUser);
                adjDbObject.AddInParameter("@AppointOrderSign", DbType.String, model.AppointOrderSign);
                adjDbObject.AddInParameter("@PhoneCallTime", DbType.Int32, model.PhoneCallTime);
                adjDbObject.AddInParameter("@lng", DbType.Double, model.Lng);
                adjDbObject.AddInParameter("@lat", DbType.Double, model.Lat);
                adjDbObject.AddInParameter("@create_time", DbType.DateTime, model.Create_time);
                adjDbObject.AddInParameter("@create_user", DbType.String, model.Create_user);
                //adjDbObject.AddInParameter("@update_time", DbType.DateTime, model.Update_time);
                // adjDbObject.AddInParameter("@update_user", DbType.String, model.Update_user);
                //adjDbObject.AddInParameter("@delete_time", DbType.DateTime, model.Delete_time);
                // adjDbObject.AddInParameter("@delete_user", DbType.String, model.Delete_user);
                // adjDbObject.AddInParameter("@delete_flag", DbType.Boolean, model.Delete_flag);
                adjDbObject.Execute();
            }
            catch (Exception ex)
            {
                isok = false;
            }
            return isok;
        }
        /// <summary>
        /// 获取订单预约种子数
        /// </summary>
        /// <returns></returns>
        public int SelectSeqOrder()
        {
            AdjDBObject adjdbobject = new AdjDBObject();
            try
            {
                string cmdtext = "INSERT INTO D_SeqOrder DEFAULT VALUES SELECT @@IDENTITY";
                adjdbobject.GetSqlStringCommand(cmdtext);
                return int.Parse(adjdbobject.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

    }
}
