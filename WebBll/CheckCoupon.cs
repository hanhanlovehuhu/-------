using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using WebDal;
using WebModel;

namespace WebBll
{
    public class ThirdPartyCoupon 
    {
        public bool check(string coupon, string couponValue,string url)
        {
            WebRequest request = null;
            WebResponse response = null;
            try
            {
                //string sql = "select 1 from [d_youhui] where [CardCode] = @CardCode and UseState = 0";
                //AdjDBObject db = new AdjDBObject();
                //db.GetSqlStringCommand(sql);
                //db.AddInParameter("@CardCode", System.Data.DbType.String, coupon);
                //DataTable dt = db.ExecuteDataSet().Tables[0];
                //if (dt.Rows.Count > 0)
                //{
                request = WebRequest.Create(string.Format(url + "couponCode={0}&couponValue={1}", coupon, couponValue));
                response = request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string strMsg = reader.ReadToEnd();
                    return strMsg.Equals("1");
                }
                //}
                //else
                //{
                    //return false;
                //}
                
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void isCoupon(string url)
        {
            WebRequest request = null;
            WebResponse response = null;
            try
            {
                request = WebRequest.Create(string.Format(url + "?isCoupon=1"));
                response = request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string strMsg = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string addCoupon(string partyid,string couponCode,string couponValue,CustomerInfoEntity ci) 
        {
            string coupon = "TP_" + partyid + "_" + couponCode;
            CouponDAL cd = new CouponDAL();
            if (!cd.isExistCoupon(coupon))
            {
                cd.addCoupon(coupon, Convert.ToDouble(couponValue), partyid, ci);
            }
            return coupon;
        }

    }
}
