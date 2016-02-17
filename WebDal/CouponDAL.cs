using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebModel;

namespace WebDal
{
    public class CouponDAL
    {
        /// <summary>
        /// 判断优惠券是否存在
        /// </summary>
        /// <param name="couponCode"></param>
        /// <returns></returns>
        public bool isExistCoupon(string couponCode)
        {
            AdjDBObject adjDbObject = new AdjDBObject();
            adjDbObject.GetSqlStringCommand("select count(1) from d_youhui where Youhuima = @Youhuima");
            adjDbObject.AddInParameter("@Youhuima", System.Data.DbType.String, couponCode);
            return Convert.ToInt32(adjDbObject.ExecuteScalar()) > 0;
        }

        public bool addCoupon(string couponsCode, double couponsValue, string partyid, CustomerInfoEntity ci)
        {
            AdjDBObject adjDbObject = new AdjDBObject();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("insert into d_youhui");
            sb.AppendLine("(BatchId,cardcode,Money,Youhuima,Type,UseState,property,usecount,Remark ,Addtime,CREATE_USER,ExpiredDate,UsePeople,UserPeopleId,UserType)");
            sb.AppendLine("values");
            sb.AppendLine("(@BatchId,@cardcode,@Money,@Youhuima,@Type,@UseState,@property,@usecount,@Remark ,@Addtime,@CREATE_USER,@ExpiredDate,@UsePeople,@UserPeopleId,@UserType)");
            adjDbObject.GetSqlStringCommand(sb.ToString());
            adjDbObject.AddInParameter("@BatchId",System.Data.DbType.Int32,0);
            adjDbObject.AddInParameter("@cardcode", System.Data.DbType.String, couponsCode);
            adjDbObject.AddInParameter("@Money", System.Data.DbType.Decimal, couponsValue);
            adjDbObject.AddInParameter("@Youhuima", System.Data.DbType.String, couponsCode);
            adjDbObject.AddInParameter("@Type", System.Data.DbType.Int32, 0);
            adjDbObject.AddInParameter("@UseState", System.Data.DbType.Int32, 0);
            adjDbObject.AddInParameter("@property", System.Data.DbType.Int32, 20);
            adjDbObject.AddInParameter("@usecount", System.Data.DbType.Int32, 0);
            adjDbObject.AddInParameter("@Remark", System.Data.DbType.String, partyid);
            adjDbObject.AddInParameter("@Addtime", System.Data.DbType.DateTime, DateTime.Now);
            adjDbObject.AddInParameter("@CREATE_USER", System.Data.DbType.String, "Third Party");
            adjDbObject.AddInParameter("@ExpiredDate", System.Data.DbType.DateTime, DateTime.Now.AddYears(1));
            adjDbObject.AddInParameter("@UsePeople", System.Data.DbType.String, "");
            adjDbObject.AddInParameter("@UserPeopleId", System.Data.DbType.String, ci.CustomerId);
            adjDbObject.AddInParameter("@UserType", System.Data.DbType.String, 1);
            return adjDbObject.Execute() > 0;
        }
    }
}
