using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebModel;

namespace WebDal
{
    public class T_PartyDal
    {
        /// <summary>
        /// ADO获取实体BY SIGN
        /// </summary>
        /// <param name="sign"></param>
        /// <returns></returns>
        public T_ParterEntity GetParterModelBySign(string sign)
        {
            T_ParterEntity pEntity = null;
            AdjDBObject adjDbObject = new AdjDBObject();
            string cmdtext = "select * from dbo.T_Parter (nolock) WHERE  sign=@sign";
            adjDbObject.GetSqlStringCommand(cmdtext);
            adjDbObject.AddInParameter("@sign", DbType.String, sign);
            using (IDataReader datareader = adjDbObject.ExecuteReader())
            {
                List<T_ParterEntity> reList = GlobalFunction.GetEntityList<T_ParterEntity>(datareader);
                if (reList != null)
                {
                    pEntity = reList[0];
                }
            }
            return pEntity;
        }
    }
}
