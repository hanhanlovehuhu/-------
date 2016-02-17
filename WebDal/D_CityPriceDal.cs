using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebModel;

namespace WebDal
{
    public class D_CityPriceDal
    {

        /// <summary>
        /// 获取城市信息  兼容老版本 适应新版本 费劲
        /// </summary>
        /// <param name="cityname"></param>
        /// <returns></returns>
        public ChengShiModel GetChengShiInfo(string cityname)
        {
            try
            {
                ChengShiModel csm = new ChengShiModel();
                Citys cEntity = null;
                AdjDBObject adjDbObject = new AdjDBObject();
                string cmdtext = "select top 1  d.CityPriceId as id,CityName as name,d.Remark as notice,StartMileage as firstkm from D_CityPrice d WITH(NOLOCK) join D_CityPriceDetail dc with(nolock) on d.CityPriceId=dc.CityPriceId where CityName like '%" + cityname + "%'";
                adjDbObject.GetSqlStringCommand(cmdtext);
                // adjDbObject.AddInParameter("@cityname", DbType.String, cityname);
                using (IDataReader datareader = adjDbObject.ExecuteReader())
                {
                    List<Citys> reList = GlobalFunction.GetEntityList<Citys>(datareader);
                    if (reList != null)
                    {
                        cEntity = reList[0];
                        csm.Name = cEntity.Name;
                        csm.FirstKm = cEntity.FirstKm;
                        csm.Notice = cEntity.Notice;
                        cmdtext = "select StartTime as TStart,EndTime as TEnd,StartPrice as Price from D_CityPriceDetail WITH(NOLOCK) where [CityPriceId]=" + cEntity.Id;
                        adjDbObject.GetSqlStringCommand(cmdtext);
                        using (IDataReader datareader1 = adjDbObject.ExecuteReader())
                        {
                            List<PriceItems> reList1 = GlobalFunction.GetEntityList<PriceItems>(datareader1);
                            if (reList1 != null)
                            {
                                csm.PriceItems = reList1;
                            }
                        }
                    }
                }
                return csm;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IList<ChengShiName> GetChengShiName(string cityname)
        {
            IList<ChengShiName> citys = null;
            AdjDBObject adjDbObject = new AdjDBObject();
            try
            {
                string sql = "SELECT CityName as name FROM dbo.D_CityPrice WITH(NOLOCK) ";
                adjDbObject.GetSqlStringCommand(sql);
                using (IDataReader datareader = adjDbObject.ExecuteReader())
                {
                    citys = GlobalFunction.GetEntityList<ChengShiName>(datareader);
                }
            }
            catch (Exception ex)
            {
                citys = null;
            }
           return citys;
        }


    }
}
