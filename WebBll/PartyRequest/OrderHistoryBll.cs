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
    class OrderHistoryBll : JsonCommand
    {
        // 1215|sign|orderid|7a31f99279327f8b75506acbf0503973
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
                    string sql = "SELECT Id,OrderId,OrderStatus,StatusText,create_time FROM D_OrderStatusChangeLog where OrderId = @OrderId";
                    db.GetSqlStringCommand(sql);
                    db.AddInParameter("@OrderId",System.Data.DbType.String,req[2]);
                    DataTable dt = db.ExecuteDataSet().Tables[0];
                    List<OrderHistory> list = new List<OrderHistory>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        list.Add(new OrderHistory() { 
                            orderid = dt.Rows[i]["OrderId"].ToString(),
                            OrderStatus = Convert.ToInt32(dt.Rows[i]["OrderStatus"]),
                            StatusText = dt.Rows[i]["StatusText"].ToString(),
                            create_time = Convert.ToDateTime(dt.Rows[i]["create_time"])
                        });
                    }
                    return JsonConvert.SerializeObject(list);
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
    public class OrderHistory
    {
        public string orderid { get; set; }
        public int OrderStatus { get; set; }
        public string StatusText { get; set; }
        public DateTime create_time { get; set; }
    }
}
