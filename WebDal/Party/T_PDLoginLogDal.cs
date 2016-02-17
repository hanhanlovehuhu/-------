using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebDal
{
  public   class T_PDLoginLogDal
    {
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
