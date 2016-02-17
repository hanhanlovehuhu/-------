using Aidaijia.API.DAL;
using Aidaijia.API.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aidaijia.API.BLL
{
   public class DriverBLL
    {
       static DriverDal dal = new DriverDal();
       public static Driver GetDriverByUcode(string ucode)
        {
            return dal.GetDriverByUcode(ucode);
        }
       public static List<Driver> GetDriverByUcodes(string ucodes)
       {
           return dal.GetDriverByUcodes(ucodes);
       }

       public static DriverCount GetDriverCountByCity(string cityId)
       {
           return dal.GetDriverCountByCity(cityId);
       }

       public static List<Driver> GetDriverInfoByRange(double lat, double lng, double range,int top)
       {
           return dal.GetDriverInfoByRange(lat, lng, range, top);
       }

       public static List<DriverRange> GetDriverByRange(double lat, double lng, double range, string driverCarType)
       {
           return dal.GetDriverByRange(lat, lng, range, driverCarType);
       }
       public static bool GetDriverStatus(string ucode)
       {
           return dal.GetDriverStatus(ucode);
       }
    }
}
