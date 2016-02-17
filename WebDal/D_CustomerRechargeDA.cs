using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebModel;

namespace WebDal
{
    public class D_CustomerRechargeDA
    {
        #region Public Properties

        #endregion
        #region Private  Properties
        AdjDBObject adjDbObject;
        #endregion
        public D_CustomerRechargeDA()
        {
            adjDbObject = new AdjDBObject();
        }
        #region Constructors
        
        #endregion

        #region Public Methods
        public bool Insert(D_CustomerRechargeEntity model)
        {           
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into D_CustomerRecharge(");
            strSql.Append("Message,create_time,create_user,update_time,update_user,delete_flag,delete_time,delete_user,RechargeId,RechargeType,[From],TradeNo,CustomerId,CustomerName,Amount,Status,ResponseTime,OrderId,SubRechargeType");
            strSql.Append(") values (");
            strSql.Append("@Message,GETDATE(),@create_user,NULL,NULL,NULL,NULL,NULL,@RechargeId,@RechargeType,@From,@TradeNo,@CustomerId,@CustomerName,@Amount,@Status,NULL,@OrderId,@SubRechargeType");
            strSql.Append(") ");

            adjDbObject.GetSqlStringCommand(strSql.ToString());
            adjDbObject.AddInParameter("@Message", DbType.String, model.Message);
            adjDbObject.AddInParameter("@create_user", DbType.String, model.create_user);
            adjDbObject.AddInParameter("@RechargeId", DbType.String, model.RechargeId);
            adjDbObject.AddInParameter("@CustomerName", DbType.String, model.CustomerName);
            adjDbObject.AddInParameter("@RechargeType", DbType.String, model.RechargeType);
            adjDbObject.AddInParameter("@From", DbType.Int32, model.From);
            adjDbObject.AddInParameter("@TradeNo", DbType.String, model.TradeNo);
            adjDbObject.AddInParameter("@CustomerId", DbType.String, model.CustomerId);
            adjDbObject.AddInParameter("@Amount", DbType.Decimal, model.Amount);
            adjDbObject.AddInParameter("@Status", DbType.Int32, model.Status);

            if (!string.IsNullOrEmpty(model.OrderId))
            {
                adjDbObject.AddInParameter("@OrderId", DbType.String, model.OrderId);
            }
            else
            {
                adjDbObject.AddInParameter("@OrderId", DbType.String, DBNull.Value);
            }
            if (model.SubRechargeType != null)
            {
                adjDbObject.AddInParameter("@SubRechargeType", DbType.String, model.SubRechargeType);
            }
            else
            {
                adjDbObject.AddInParameter("@SubRechargeType", DbType.String, DBNull.Value);
            }

            return adjDbObject.Execute() > 0;
        }
        public int SelectSeq()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"INSERT INTO D_Seq DEFAULT VALUES
                                SELECT @@IDENTITY");
            adjDbObject.GetSqlStringCommand(strSql.ToString());
            return Convert.ToInt32(adjDbObject.ExecuteScalar());           
        }
        #endregion
    }
}
