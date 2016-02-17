using Aidaijia.API.Common;
using Aidaijia.API.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aidaijia.API.DAL
{
   public class DriverDal
    {
        private SqlHelper helper;

        public DriverDal()
       {
           helper = new SqlHelper("AiDaiJiaConStr");
       }
        public Driver GetDriverByUcode(string ucode)
       {          
           try
           {
               string sSql = string.Format(@"Select ddi.DriveCount,ddi.DrivedYears,ddi.DriverlicenseType,
                        ddi.Grade,ddi.Photo,ddi.Sex,ddi.Tel,ddi.Ucode,ddi.Phone,ddi.DriverName,ddi.BirthPlace,
                        dp.Latitude,dp.Longitude,ddi.Online,ddi.OnService
                         From D_DriverInfo (nolock) ddi join D_DriverPositon(nolock) dp on ddi.Ucode=dp.Ucode
                         where ddi.Ucode='{0}' or ddi.DriverCarType='{0}' or ddi.Phone='{0}'", ucode);               
               var dt = helper.GetDataTable(sSql);
               if (dt != null && dt.Rows.Count>0)
               {
                   Driver entity = new Driver();                 
                   entity.DriveCount = Convert.ToInt32(dt.Rows[0]["DriveCount"] == DBNull.Value ? 0 : dt.Rows[0]["DriveCount"]);
                   entity.DrivedYears = Convert.ToInt32(dt.Rows[0]["DrivedYears"] == DBNull.Value ? 0 : dt.Rows[0]["DrivedYears"]);
                   entity.DriverlicenseType =dt.Rows[0]["DriverlicenseType"]==DBNull.Value?"": dt.Rows[0]["DriverlicenseType"].ToString();
                   entity.Grade = Convert.ToInt32(dt.Rows[0]["Grade"] == DBNull.Value ? 0 : dt.Rows[0]["Grade"]);
                   entity.Photo =dt.Rows[0]["Photo"]==DBNull.Value?"": dt.Rows[0]["Photo"].ToString();
                   entity.Sex = Convert.ToInt32(dt.Rows[0]["Sex"] == DBNull.Value ? 0 : dt.Rows[0]["Sex"]);
                   entity.Tel =dt.Rows[0]["Tel"]==DBNull.Value?"": dt.Rows[0]["Tel"].ToString();
                   entity.Ucode =dt.Rows[0]["Ucode"]==DBNull.Value?"": dt.Rows[0]["Ucode"].ToString();
                   entity.Phone = dt.Rows[0]["Phone"] == DBNull.Value ? "" : dt.Rows[0]["Phone"].ToString();
                   entity.DriverName = dt.Rows[0]["DriverName"] == DBNull.Value ? "" : dt.Rows[0]["DriverName"].ToString();
                   entity.BirthPlace = dt.Rows[0]["BirthPlace"] == DBNull.Value ? "" : dt.Rows[0]["BirthPlace"].ToString();
                   entity.Lat = Convert.ToDouble(dt.Rows[0]["Latitude"] == DBNull.Value ? 0 : dt.Rows[0]["Latitude"]);
                   entity.Lng = Convert.ToDouble(dt.Rows[0]["Longitude"] == DBNull.Value ? 0 : dt.Rows[0]["Longitude"]);
                   entity.Online = Convert.ToInt32(dt.Rows[0]["Online"] == DBNull.Value ? 0 : dt.Rows[0]["Online"]);
                   entity.OnService = Convert.ToInt32(dt.Rows[0]["OnService"] == DBNull.Value ? 0 : dt.Rows[0]["OnService"]);
                  return entity;
               }
               else
               {                  
                   return null;
               }
           }
           catch (Exception ex)
           {
               LogControl.WriteError("GetDriverByUcode查询司机失败|Error:" + ex.Message);
              return null;
           }

       }

        public List<Driver> GetDriverByUcodes(string ucodes)
        {
            List<Driver> result = new List<Driver>();
            try
            {
                string[] sIds = ucodes.Split(',');
                string newIds = "";

                for (int i = 0; i < sIds.Length; i++)
                {
                    newIds += "'" + sIds[i] + "',";                   
                }
                newIds = newIds.Substring(0, newIds.Length - 1);
                string sSql = string.Format("Select * From D_DriverInfo (nolock) where Ucode in ({0})", newIds);
                var dt = helper.GetDataTable(sSql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Driver entity = new Driver();
                        entity.Deposit = Convert.ToDecimal(dt.Rows[i]["Deposit"] == DBNull.Value ? 0 : dt.Rows[i]["Deposit"]);
                        entity.DisposableGoodsCount = Convert.ToInt32(dt.Rows[i]["DisposableGoodsCount"] == DBNull.Value ? 0 : dt.Rows[i]["DisposableGoodsCount"]);
                        entity.DriveCount = Convert.ToInt32(dt.Rows[i]["DriveCount"] == DBNull.Value ? 0 : dt.Rows[i]["DriveCount"]);
                        entity.DrivedYears = Convert.ToInt32(dt.Rows[i]["DrivedYears"] == DBNull.Value ? 0 : dt.Rows[i]["DrivedYears"]);
                        entity.DriverlicenseType = dt.Rows[i]["DriverlicenseType"] == DBNull.Value ? "" : dt.Rows[i]["DriverlicenseType"].ToString();
                        entity.Grade = Convert.ToInt32(dt.Rows[i]["Grade"] == DBNull.Value ? 0 : dt.Rows[i]["Grade"]);
                        entity.Photo = dt.Rows[i]["Photo"] == DBNull.Value ? "" : dt.Rows[i]["Photo"].ToString();
                        entity.Sex = Convert.ToInt32(dt.Rows[i]["Sex"] == DBNull.Value ? 0 : dt.Rows[i]["Sex"]);
                        entity.Tel = dt.Rows[i]["Tel"] == DBNull.Value ? "" : dt.Rows[i]["Tel"].ToString();
                        entity.Ucode = dt.Rows[i]["Ucode"] == DBNull.Value ? "" : dt.Rows[i]["Ucode"].ToString();
                        entity.Phone = dt.Rows[i]["Phone"] == DBNull.Value ? "" : dt.Rows[i]["Phone"].ToString();
                        entity.DriverName = dt.Rows[i]["DriverName"] == DBNull.Value ? "" : dt.Rows[i]["DriverName"].ToString();
                        entity.BirthPlace = dt.Rows[i]["BirthPlace"] == DBNull.Value ? "" : dt.Rows[i]["BirthPlace"].ToString();
                        result.Add(entity);
                    }                   
                }              
            }
            catch (Exception ex)
            {
                LogControl.WriteError("GetDriverByUcodes查询司机列表失败|Error:" + ex.Message);               
            }
            return result;

        }

        public DriverCount GetDriverCountByCity(string cityId)
        {
            try
            {
                string sSql = string.Format(@"select a.online,b.busy from 
                   (select COUNT(1) online from  D_DriverInfo(nolock) where Online=1  and CityId='{0}')a,
                   (select COUNT(1) busy from  D_DriverInfo(nolock) where OnService=1  and CityId='{0}')b", cityId);
                var dt = helper.GetDataTable(sSql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DriverCount entity = new DriverCount();
                    entity.Online = Convert.ToInt32(dt.Rows[0]["online"] == DBNull.Value ? 0 : dt.Rows[0]["online"]);
                    entity.Busy = Convert.ToInt32(dt.Rows[0]["busy"] == DBNull.Value ? 0 : dt.Rows[0]["busy"]);                   
                    return entity;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogControl.WriteError("GetDriverCountByCity查询司机失败|Error:" + ex.Message);
                return null;
            }
        }

        public List<Driver> GetDriverInfoByRange(double lat, double lng, double range,int Top)
        {
            List<Driver> result = new List<Driver>();
            try
            {               
                string sSql = string.Format(@" SELECT TOP {3}  ddi.Ucode, ddi.DriverName, ddi.Deposit, ddi.DisposableGoodsCount, ddi.DriveCount, ddi.DrivedYears, 
	                                  ddi.DriverlicenseType,ddi.Grade,ddi.Photo,ddi.Sex,ddi.Tel,ddi.Phone,ddi.BirthPlace,ddp.Latitude,ddp.Longitude
                                       FROM D_DriverInfo AS ddi WITH(NOLOCK)
		                                INNER JOIN D_DriverPositon AS ddp WITH(NOLOCK)
			                                ON ddi.Ucode = ddp.Ucode
	                                WHERE ddi.OnLine = 1
		                                AND ddi.OnService = 0
										AND	dbo.fnGetDistance({0},{1},ddp.Latitude,ddp.Longitude)<={2}
                                        Order by  dbo.fnGetDistance({0},{1},ddp.Latitude,ddp.Longitude)
										", lat, lng, range,Top<0?10:Top);
                var dt = helper.GetDataTable(sSql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Driver entity = new Driver();
                        entity.Deposit = Convert.ToDecimal(dt.Rows[i]["Deposit"] == DBNull.Value ? 0 : dt.Rows[i]["Deposit"]);
                        entity.DisposableGoodsCount = Convert.ToInt32(dt.Rows[i]["DisposableGoodsCount"] == DBNull.Value ? 0 : dt.Rows[i]["DisposableGoodsCount"]);
                        entity.DriveCount = Convert.ToInt32(dt.Rows[i]["DriveCount"] == DBNull.Value ? 0 : dt.Rows[i]["DriveCount"]);
                        entity.DrivedYears = Convert.ToInt32(dt.Rows[i]["DrivedYears"] == DBNull.Value ? 0 : dt.Rows[i]["DrivedYears"]);
                        entity.DriverlicenseType = dt.Rows[i]["DriverlicenseType"] == DBNull.Value ? "" : dt.Rows[i]["DriverlicenseType"].ToString();
                        entity.Grade = Convert.ToInt32(dt.Rows[i]["Grade"] == DBNull.Value ? 0 : dt.Rows[i]["Grade"]);
                        entity.Photo = dt.Rows[i]["Photo"] == DBNull.Value ? "" : dt.Rows[i]["Photo"].ToString();
                        entity.Sex = Convert.ToInt32(dt.Rows[i]["Sex"] == DBNull.Value ? 0 : dt.Rows[i]["Sex"]);
                        entity.Phone = dt.Rows[i]["Phone"] == DBNull.Value ? "" : dt.Rows[i]["Phone"].ToString();
                        entity.Tel = dt.Rows[i]["Tel"] == DBNull.Value ? "" : dt.Rows[i]["Tel"].ToString();
                        entity.Ucode = dt.Rows[i]["Ucode"] == DBNull.Value ? "" : dt.Rows[i]["Ucode"].ToString();
                        entity.DriverName = dt.Rows[i]["DriverName"] == DBNull.Value ? "" : dt.Rows[i]["DriverName"].ToString();
                        entity.BirthPlace = dt.Rows[i]["BirthPlace"] == DBNull.Value ? "" : dt.Rows[i]["BirthPlace"].ToString();
                        entity.Lat = dt.Rows[i]["Latitude"] == DBNull.Value ? 0 : Convert.ToDouble(dt.Rows[i]["Latitude"]);
                        entity.Lng = dt.Rows[i]["Longitude"] == DBNull.Value ? 0 : Convert.ToDouble(dt.Rows[i]["Longitude"]);
                        result.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                LogControl.WriteError("GetDriverInfoByRange查询周边司机列表失败|Error:" + ex.Message);
            }
            return result;
        }

        public List<DriverRange> GetDriverByRange(double lat, double lng, double range, string driverCarType)
        {
            List<DriverRange> result = new List<DriverRange>();
            try
            {
                string where = "";
                if (!string.IsNullOrEmpty(driverCarType))
                {
                    where =string.Format(" AND ddi.DriverCarType='{0}'",driverCarType);
                }
                string sSql = string.Format(@" SELECT TOP 100  ddi.Ucode, ddi.DriverName,ddp.Latitude,ddp.Longitude,ddi.Online,ddi.OnService
                                       FROM D_DriverInfo AS ddi WITH(NOLOCK)
		                                INNER JOIN D_DriverPositon AS ddp WITH(NOLOCK)
			                                ON ddi.Ucode = ddp.Ucode
	                                WHERE ddi.OnLine = 1
		                                AND ddi.OnService = 0
										AND	dbo.fnGetDistance({0},{1},ddp.Latitude,ddp.Longitude)<={2} 
                                         {3}
										Order by dbo.fnGetDistance({0},{1},ddp.Latitude,ddp.Longitude)    ", lat, lng, range,where);
                var dt = helper.GetDataTable(sSql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DriverRange entity = new DriverRange();                      
                        entity.Ucode = dt.Rows[i]["Ucode"] == DBNull.Value ? "" : dt.Rows[i]["Ucode"].ToString();
                        entity.DriverName = dt.Rows[i]["DriverName"] == DBNull.Value ? "" : dt.Rows[i]["DriverName"].ToString();                      
                        entity.Lat = dt.Rows[i]["Latitude"] == DBNull.Value ? 0 : Convert.ToDouble(dt.Rows[i]["Latitude"]);
                        entity.Lng = dt.Rows[i]["Longitude"] == DBNull.Value ? 0 : Convert.ToDouble(dt.Rows[i]["Longitude"]);
                        entity.Online = Convert.ToInt32(dt.Rows[0]["Online"] == DBNull.Value ? 0 : dt.Rows[0]["Online"]);
                        entity.OnService = Convert.ToInt32(dt.Rows[0]["OnService"] == DBNull.Value ? 0 : dt.Rows[0]["OnService"]);
                        result.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                LogControl.WriteError("GetDriverByRange查询周边司机列表失败|Error:" + ex.Message);
            }
            return result;
        }

        public bool GetDriverStatus(string ucode)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(@"Select onService 
                      from  D_DriverInfo(nolock)  where Ucode='{0}'", ucode);
                var dt = helper.GetDataTable(strSql.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    int state = dt.Rows[0]["onService"] == DBNull.Value ? 1 : Convert.ToInt32(dt.Rows[0]["onService"]);
                    if (state == 1)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogControl.WriteError(string.Format("{0}|GetDriverStatus获取失败|Error:{1}", DateTime.Now, ex.Message));
                return false;
            }
        }
    }
}
