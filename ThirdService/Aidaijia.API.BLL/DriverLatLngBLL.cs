using aidaijia.API.DA;
using Aidaijia.API.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aidaijia.API.BLL
{
   public class DriverLatLngBLL
    {
       static DriverLatLngDAL dal = new DriverLatLngDAL();
       public static bool UpdateDriverLatLng(string Ucode,double Lat,double Lng)
       {
           return dal.UpdateDriverLatLng(Ucode, Lat, Lng);
       }
    }
}
