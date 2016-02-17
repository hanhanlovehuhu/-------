using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebModel;
using WebUtility;
using System.Web;

namespace WebDal
{
    public class D_DriverInfoDal
    {

        public static string GenerateDistributeOrderId(int seq)
        {
            string returnStr = string.Empty;
            Number number = new Number("1234567890");
            string code = number.ToString(seq);
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
        /// 获取附近10个司机 优化过
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <returns></returns>
        public List<sjinfo> GetNewUserByLatlng(string lat, string lng, string parterid=null)
        {
            List<sjinfo> drvModels = new List<sjinfo>();
            AdjDBObject adjDbObject = new AdjDBObject();
            try
            {
                adjDbObject.GetStoredProcCommand("GetNearSjBylanlngCount");
                adjDbObject.AddInParameter("@lat", DbType.Double, lat);
                adjDbObject.AddInParameter("@lng", DbType.Double, lng);
                adjDbObject.AddInParameter("@online", DbType.Int32, 6);
                adjDbObject.AddInParameter("@onservice", DbType.Int32, 4);
                DataSet ds = adjDbObject.ExecuteDataSet();
                List<sjinfo> list = new List<sjinfo>();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        sjinfo model = new sjinfo();
                        model.uid = row["id"].ToString();
                        model.ucode = row["ucode"].ToString();
                        model.name = row["realname"].ToString();
                        model.phone = "";//row["phone"].ToString();//row["phone"].ToString().Substring(0, 3) + "****" + row["phone"].ToString().Substring(7, 4);
                        model.pic = ConfigHelper.ImagePath + row["pic"].ToString();
                        model.jialin = row["jialin"].ToString();
                        model.cishu = row["cishu"].ToString();
                        model.SatisfactionRate = row["SatisfactionRate"] == DBNull.Value ? "" : row["SatisfactionRate"].ToString();
                        // model.istogether = Convert.ToInt32(row["IsTogether"].ToString());
                        if (Convert.ToBoolean(row["onservice"]))
                        {
                            model.state = "服务中";
                        }
                        else
                        {
                            model.state = "空闲中";
                        }
                        model.lat = row["lat"].ToString();
                        model.lng = row["lng"].ToString();
                        double mi = Convert.ToDouble(row["juli"]);
                        model.juli = mi;
                        model.jiguan = row["huji"].ToString();
                        if (!String.IsNullOrEmpty(row["idcode"].ToString()))
                        {
                            model.idcode = row["idcode"].ToString().Substring(0, row["idcode"].ToString().Length - 4) + "****";
                        }
                        else
                        {
                            model.idcode = String.Empty;
                        }
                        model.goodrate = row["hp"].ToString();
                        model.xinji = row["cp"].ToString();

                        if (!string.IsNullOrEmpty(parterid) && parterid.Equals("32d374227e5b70ab6cc55f1b994f0e7c"))  //如果合作商是中国4S在线那么电话将显示
                        {
                            model.phone = "4006138138";
                        }

                        drvModels.Add(model);
                    }
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        sjinfo model = new sjinfo();
                        model.uid = row["id"].ToString();
                        model.ucode = row["ucode"].ToString();
                        model.name = row["realname"].ToString();
                        model.phone = row["phone"].ToString();
                        model.pic = ConfigHelper.ImagePath + row["pic"].ToString();
                        model.jialin = row["jialin"].ToString();
                        model.cishu = row["cishu"].ToString();
                        // model.istogether = Convert.ToInt32(row["IsTogether"].ToString());
                        if (Convert.ToBoolean(row["onservice"]))
                        {
                            model.state = "服务中";
                        }
                        else
                        {
                            model.state = "空闲中";
                        }
                        model.lat = row["lat"].ToString();
                        model.lng = row["lng"].ToString();
                        double mi = Convert.ToDouble(row["juli"]);
                        model.juli = mi;
                        model.jiguan = row["huji"].ToString();
                        if (!String.IsNullOrEmpty(row["idcode"].ToString()))
                        {
                            model.idcode = row["idcode"].ToString().Substring(0, row["idcode"].ToString().Length - 4) + "****";
                        }
                        else
                        {
                            model.idcode = String.Empty;
                        }
                        model.goodrate = row["hp"].ToString();
                        model.xinji = row["cp"].ToString();
                        drvModels.Add(model);
                    }
                }
            }
            catch
            {
            }
            return drvModels;

        }

        /// <summary>
        /// 根据工号获取司机信息
        /// </summary>
        /// <param name="ucode"></param>
        /// <returns></returns>
        public DriverInfoEntity GetDriverInfoByUcode(string ucode)
        {
            AdjDBObject adjDbObject = new AdjDBObject();
            string sql = "SELECT * FROM [dbo].[D_DriverInfo] (NOLOCK) WHERE Ucode = @Ucode";
            adjDbObject.GetSqlStringCommand(sql);
            adjDbObject.AddInParameter("@Ucode", DbType.String, ucode);
            try
            {
                using (IDataReader dr = adjDbObject.ExecuteReader())
                {
                    List<DriverInfoEntity> users = GlobalFunction.GetEntityList<DriverInfoEntity>(dr);
                    if (users != null)
                    {
                        return users[0];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper lh = new LogHelper();
                lh.log(e.ToString(), HttpContext.Current.Server.MapPath("Log/error"));
                return null;
            }
        }

        /// <summary>
        /// 根据ID获取司机工号
        /// </summary>
        /// <returns></returns>
        public string  GetUcodeById(string uid)
        {
            string result = string.Empty;
            AdjDBObject adjDbObject = new AdjDBObject();
            string sql = "SELECT top 1 Ucode FROM [dbo].[D_DriverInfo] (NOLOCK) WHERE  id= @uid";
            adjDbObject.GetSqlStringCommand(sql);
            adjDbObject.AddInParameter("@uid", DbType.String, uid);
            try
            {
                using (IDataReader dr = adjDbObject.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        result = dr[0].ToString();
                    }
                    return result;
                }
            }
            catch (Exception)
            {

                return "";
            }

        }
    }
}
