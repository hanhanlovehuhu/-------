using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebModel;

namespace WebDal
{
   public class D_DistributeOrderDetailDal
    {

        /// <summary>
        /// 创建派单明细
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool CreateDistributeOrderDetail(DistributeOrderDetailEntity model)
        {
            try
            {
                AdjDBObject adjDbObject = new AdjDBObject();
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into D_DistributeOrderDetail(");
                strSql.Append("delete_time,delete_user,DistributeOrderId,Status,Ucode,create_time,create_user,update_time,update_user,delete_flag");
                strSql.Append(") values (");
                strSql.Append("NULL,NULL,@DistributeOrderId,@Status,@Ucode,GETDATE(),@create_user,NULL,NULL,NULL");
                strSql.Append(") ");
                adjDbObject.GetSqlStringCommand(strSql.ToString());
                adjDbObject.AddInParameter("@DistributeOrderId", DbType.String, model.DistributeOrderId);
                adjDbObject.AddInParameter("@Status", DbType.Int32, model.Status);
                adjDbObject.AddInParameter("@Ucode", DbType.String, model.Ucode);
                adjDbObject.AddInParameter("@create_user", DbType.String, model.Create_user);
                adjDbObject.Execute();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
