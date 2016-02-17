using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebModel;

namespace WebDal
{
    public class T_ParterDyLogDal
    {
        public bool AddParterDyLog(T_ParterDyLog model)
        {
            AdjDBObject adjDbObject = new AdjDBObject();
            bool isok = true;
            try
            {
                adjDbObject.GetStoredProcCommand("sp3_T_ParterDyLog_i");
                adjDbObject.AddOutParameter("@id", DbType.Int32, 4);
                adjDbObject.AddInParameter("@sign", DbType.String, model.sign);
                adjDbObject.AddInParameter("@typeid", DbType.Int32, model.typeid);
                adjDbObject.AddInParameter("@imei", DbType.String, model.imei);
                adjDbObject.AddInParameter("@lat", DbType.String, model.lat);
                adjDbObject.AddInParameter("@lng", DbType.String, model.lng);
                adjDbObject.AddInParameter("@addtime", DbType.DateTime, model.addtime);
                adjDbObject.Execute();

            }
            catch (Exception ex)
            {
                isok = false;
            }
            return isok;
        }

        /// <summary>
        /// 根据sign获取当天访问量
        /// </summary>
        /// <param name="sign"></param>
        /// <returns></returns>
        public int GetcountBysign(string sign)
        {
            int cou = 0;
            try
            {
                AdjDBObject adjDbObject = new AdjDBObject();
                string cmdtext = "select count(*) as counts  from T_ParterDyLog where sign=@sign  and addtime between'" +
                            DateTime.Now.ToString("yyyy-MM-dd") + "' and '" +
                            DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + "' ";
                adjDbObject.GetSqlStringCommand(cmdtext);
                adjDbObject.AddInParameter("@sign", DbType.String, sign);
                cou = int.Parse(adjDbObject.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                cou = 0;
            }
            return cou;
        }

    }
}
