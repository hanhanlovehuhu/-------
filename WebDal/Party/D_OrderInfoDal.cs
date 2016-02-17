using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace WebDal
{
   public  class D_OrderInfoDal
    {
        /// <summary>
        /// 是否首次生成订单
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public int IsPayinfoFirst(string phone)
        {
            int count = 0;
            try
            {
                AdjDBObject adjDbObject = new AdjDBObject();
                string sqlCou = "SELECT COUNT(1) as count FROM dbo.D_OrderInfo (nolock) WHERE state=30 AND cellphone=@phone";
                adjDbObject.GetSqlStringCommand(sqlCou);
                adjDbObject.AddInParameter("@phone", DbType.String, phone);
                using (DataTable dt = adjDbObject.ExecuteDataSet().Tables[0])
                {
                    count = int.Parse(dt.Rows[0]["count"].ToString());
                }
                return count;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        public void getOrderCellphone_Ucode(string orderid, out string cellphone, out string ucode)
        {
            cellphone = "";
            ucode = "";
            AdjDBObject adjDbObject = new AdjDBObject();
            adjDbObject.GetSqlStringCommand("select cellphone,ucode from D_OrderInfo where orderid = @orderid");
            adjDbObject.AddInParameter("@orderid", DbType.String, orderid);
            DataTable dt = adjDbObject.ExecuteDataSet().Tables[0];
            if (dt.Rows.Count > 0)
            {
                cellphone = dt.Rows[0]["cellphone"].ToString();
                ucode = dt.Rows[0]["ucode"].ToString();
            }
            else
            {
                throw new Exception("没有该订单");
            }
        }
    }
}
