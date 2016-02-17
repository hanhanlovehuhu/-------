using Aidaijia.API.Common;
using Aidaijia.API.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
namespace aidaijia.API.DA
{
   public class DriverLatLngDAL
    {
         private SqlHelper helper;

         public DriverLatLngDAL()
       {
           helper = new SqlHelper("AiDaiJiaDStr");
       }

         public bool UpdateDriverLatLng(string Ucode,double Lat,double Lng)
         {
             try
             {
                 string sSql = string.Format(@"Update D_DriverPositon Set Longitude=@Longitude,Latitude=@Latitude,LastUpdatetime=getdate() where Ucode=@Ucode");
                 SqlParameter[] paras = new SqlParameter[] { new SqlParameter("@Latitude",Lat),
                 new SqlParameter("@Longitude",Lng),
                  new SqlParameter("@Ucode",Ucode)};
                 var value = helper.ExecuteCommand(sSql,paras);
                 return value>0;
             }
             catch (Exception ex)
             {
                 LogControl.WriteError("UpdateDriverLatLng更新司机经纬度失败|Error:" + ex.Message);
                 return false;
             }

         }
    }
}
