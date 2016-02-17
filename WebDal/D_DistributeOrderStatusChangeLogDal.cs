using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebModel;

namespace WebDal
{
   public class D_DistributeOrderStatusChangeLogDal
    {
       public bool CreateDistributeOrderStatusChangeLog(DistributeOrderStatusChangeLogEntity model)
       {
           try
           {
               AdjDBObject adjDbObject = new AdjDBObject();
               string sql = "INSERT INTO D_DistributeOrderStatusChangeLog(DistributeOrderId,Status,StatusText,create_time,create_user )VALUES(@DistributeOrderId,@Status,@StatusText,getdate(),@create_user )";
               adjDbObject.GetSqlStringCommand(sql);
               adjDbObject.AddInParameter("@DistributeOrderId", DbType.String, model.DistributeOrderId);
               adjDbObject.AddInParameter("@Status", DbType.Int32, model.Status);
               adjDbObject.AddInParameter("@StatusText", DbType.String, model.StatusText);
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
