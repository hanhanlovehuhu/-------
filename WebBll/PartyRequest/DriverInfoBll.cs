using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDal;
using WebModel;
using WebUtility;

namespace WebBll.PartyRequest
{
    class DriverInfoBll : JsonCommand
    {
        // 1216|sign|UCode|7a31f99279327f8b75506acbf0503973
        public string execute(string request)
        {
            string[] req = request.Split('|');
            var sign = EncodingUtil.GetMd5(req[2] + "aidaijia", "utf-8");
            if (sign.ToLower() == req[3].ToLower())
            {
                T_ParterEntity parter = new T_PartyDal().GetParterModelBySign(req[1].ToLower());
                if (parter != null)
                {
                    AdjDBObject db = new AdjDBObject();
                    string sql = "select Ucode, DriverName,'" + ConfigHelper.ImagePath + "/' + Photo Photo,DrivedYears,DriveCount,NewGrade,SatisfactionRate from d_driverinfo where UCode = @UCode";
                    db.GetSqlStringCommand(sql);
                    db.AddInParameter("@UCode", System.Data.DbType.String, req[2]);
                    DataTable dt = db.ExecuteDataSet().Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        return JsonConvert.SerializeObject(new { 
                            Ucode = dt.Rows[0]["Ucode"].ToString(),
                            DriverName = dt.Rows[0]["DriverName"].ToString(),
                            DrivedYears = dt.Rows[0]["DrivedYears"].ToString(),
                            Photo = dt.Rows[0]["Photo"].ToString(),
                            DriveCount = dt.Rows[0]["DriveCount"].ToString(),
                            NewGrade = dt.Rows[0]["NewGrade"].ToString(),
                            SatisfactionRate = dt.Rows[0]["SatisfactionRate"].ToString()
                        });
                    }
                    throw new Exception("没有该司机！");
                }
                else
                {
                    throw new Exception("商户标识错误");
                }

            }
            else
            {
                throw new Exception("签名错误。");
            }
        }
    }
}
