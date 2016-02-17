using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebModel;
using WebUtility;

namespace WebDal
{
    public class DistributeOrderInfoDAL
    {
        /// <summary>
        /// 获取派单编号种子
        /// </summary>
        /// <returns></returns>
        public int SelectSeqOrder()
        {
            AdjDBObject adjDbObject = new AdjDBObject();
            try
            {
                adjDbObject.GetSqlStringCommand("INSERT INTO D_SeqOrder DEFAULT VALUES SELECT @@IDENTITY");
                object obj = adjDbObject.ExecuteScalar();
                if (obj != null)
                    return Convert.ToInt32(obj);
                else return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public string GenerateDistributeOrderId(int seq)
        {
            string returnStr = string.Empty;
           // Number number = new Number("1234567890"); //清除随机数生成，直接根据种子生产
           // string code = number.ToString(seq);

            string code = seq.ToString();
            int count = code.Length;
            string buStr = string.Empty;
            for (int i = 0; i < 8 - count; i++)
            {
                buStr += @"0";
            }
            returnStr = "DOOT" + buStr + code;
            return returnStr;
        }
        /// <summary>
        /// 添加派单
        /// </summary>
        /// <param name="model">派单实体</param>
        /// <returns></returns>
        public bool AddDistributeOrderInfo(DistributeOrderInfoEntity model)
        {
            AdjDBObject adjDbObject = new AdjDBObject();
            bool isok = true;
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into D_DistributeOrderInfo(");
                strSql.Append("PartyId,DistributeUser,Cellphone,Address,SuccessTime,Ucode,lng,lat,CustomerId,CustomerName,CustomerType,DistributeOrderId,AccountType,FromCellphone,AppointmentTime,create_time,create_user,update_time,update_user,delete_flag,delete_time,delete_user,AppointOrderId,OrderId,BusinessType,OrderFrom,OrderType,Status,DistributeUserType");
                strSql.Append(") values (");
                strSql.Append("@PartyId,@DistributeUser,@Cellphone,@Address,@SuccessTime,@Ucode,@lng,@lat,@CustomerId,@CustomerName,@CustomerType,@DistributeOrderId,@AccountType,@FromCellphone,@AppointmentTime,GETDATE(),@create_user,@update_time,@update_user,@delete_flag,@delete_time,@delete_user,@AppointOrderId,@OrderId,@BusinessType,@OrderFrom,@OrderType,@Status,@DistributeUserType");
                strSql.Append(") ");

                adjDbObject.GetSqlStringCommand(strSql.ToString());
                adjDbObject.AddInParameter("@partyId", DbType.String, model.PartyId);
                adjDbObject.AddInParameter("@DistributeUser", DbType.String, model.DistributeUser);
                adjDbObject.AddInParameter("@Cellphone", DbType.String, model.Cellphone);
                adjDbObject.AddInParameter("@Address", DbType.String, model.Address);
                adjDbObject.AddInParameter("@SuccessTime", DbType.DateTime, model.SuccessTime);
                adjDbObject.AddInParameter("@Ucode", DbType.String, model.Ucode);
                adjDbObject.AddInParameter("@lng", DbType.Decimal, model.Lng);
                adjDbObject.AddInParameter("@lat", DbType.Decimal, model.Lat);
                adjDbObject.AddInParameter("@CustomerId", DbType.String, model.CustomerId);
                adjDbObject.AddInParameter("@CustomerName", DbType.String, model.CustomerName);
                adjDbObject.AddInParameter("@CustomerType", DbType.Int32, model.CustomerType);
                adjDbObject.AddInParameter("@DistributeOrderId", DbType.String, model.DistributeOrderId);
                adjDbObject.AddInParameter("@AccountType", DbType.Int32, model.AccountType);
                adjDbObject.AddInParameter("@FromCellphone", DbType.String, model.Fromcellphone);
                adjDbObject.AddInParameter("@AppointmentTime", DbType.DateTime, model.AppointmentTime);
                adjDbObject.AddInParameter("@create_user", DbType.String, model.Create_user);
                adjDbObject.AddInParameter("@update_time", DbType.DateTime, model.Update_time);
                adjDbObject.AddInParameter("@update_user", DbType.String, model.Update_user);
                adjDbObject.AddInParameter("@delete_flag", DbType.Boolean, model.Delete_flag);
                adjDbObject.AddInParameter("@delete_time", DbType.DateTime, model.Delete_time);
                adjDbObject.AddInParameter("@delete_user", DbType.String, model.Delete_user);
                adjDbObject.AddInParameter("@AppointOrderId", DbType.String, model.AppointOrderId);
                adjDbObject.AddInParameter("@OrderId", DbType.String, model.OrderId);
                adjDbObject.AddInParameter("@BusinessType", DbType.Int32, model.BusinessType);
                adjDbObject.AddInParameter("@OrderFrom", DbType.Int32, model.OrderFrom);
                adjDbObject.AddInParameter("@OrderType", DbType.Int32, model.OrderType);
                adjDbObject.AddInParameter("@Status", DbType.Int32, model.Status);
                adjDbObject.AddInParameter("@DistributeUserType", DbType.Int32, model.DistributeUserType);
                adjDbObject.Execute();
            }
            catch (Exception ex)
            {
                isok = false;
            }
            return isok;
        }



        public bool UpdateOnDoService(int OnDoService, string ucode)
        {
            bool isok = false;
            string sql = "UPDATE dbo.D_DriverInfo SET OnDOService = @OnDOService WHERE Ucode = @Ucode";
            AdjDBObject adjDbObject = new AdjDBObject();
            adjDbObject.GetSqlStringCommand(sql);
            adjDbObject.AddInParameter("@OnDOService", DbType.Int32, OnDoService);
            adjDbObject.AddInParameter("@Ucode", DbType.String, ucode);
            int i = adjDbObject.Execute();
            if (i != 0)
            {
                isok = true;
            }
            return isok;
        }


    }
}
