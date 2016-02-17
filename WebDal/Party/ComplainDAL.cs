using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebModel;

namespace WebDal.Party
{
    public class ComplainDAL
    {
        /// <summary>
        /// 新增一条投诉记录
        /// </summary>
        /// <param name="complain"></param>
        /// <returns></returns>
        public int addComplain(D_OrderComplaint complain)
        {
            AdjDBObject db = new AdjDBObject();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("insert into D_OrderComplaint ");
            sb.AppendLine("(OrderId ,CustomerId,CustomerName,Cellphone,Ucode,DriverName,ComplaintType,ComplaintContent,create_time,create_user,delete_flag)");
            sb.AppendLine("values");
            sb.AppendLine("(@OrderId ,@CustomerId,@CustomerName,@Cellphone,@Ucode,@DriverName,1,@ComplaintContent,getdate(),@create_user,0)");
            db.GetSqlStringCommand(sb.ToString());
            db.AddInParameter("@OrderId",System.Data.DbType.String,complain.OrderId);
            db.AddInParameter("@CustomerId", System.Data.DbType.String, complain.CustomerId);
            db.AddInParameter("@CustomerName", System.Data.DbType.String, complain.CustomerName);
            db.AddInParameter("@Cellphone", System.Data.DbType.String, complain.Cellphone);
            db.AddInParameter("@Ucode", System.Data.DbType.String, complain.Ucode);
            db.AddInParameter("@DriverName", System.Data.DbType.String, complain.DriverName);
            db.AddInParameter("@ComplaintContent", System.Data.DbType.String, complain.ComplaintContent);
            db.AddInParameter("@create_user", System.Data.DbType.String, complain.CreateUser);
            return db.Execute();
        }
    }
}
