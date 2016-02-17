using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebModel;

namespace WebDal
{
   public  class D_DriverCommentDal
    {
       public List<SjComment> GetPingLuns(string ucode)
       {
           AdjDBObject adjDbObject = new AdjDBObject();
           try
           {
               List<SjComment> ret = null;
               string cmdtext = "SELECT Ucode,Evaluate,CustomerName,Cellphone,Comment,Create_time FROM dbo.D_DriverComment where Status=2 AND Ucode=@ucode ORDER BY create_time DESC";
               adjDbObject.GetSqlStringCommand(cmdtext);
               adjDbObject.AddInParameter("@ucode", DbType.String, ucode);
               using (DataSet ds = adjDbObject.ExecuteDataSet())
               {
                   if (ds.Tables[0].Rows.Count > 0)
                   {
                       ret = GlobalFunction.GetEntityListByTable<SjComment>(ds.Tables[0]);
                   }
               }
               return ret;
           }
           catch (Exception)
           {
               return null;
           }
       }


    }
}
