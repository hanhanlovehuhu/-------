using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aidaijia.API.Entity
{
   public class Driver
    {
       public string Ucode { set; get; }

       public int Sex { set; get; }

       public string Photo { set; get; }

       public string Tel { set; get; }

       public string DriverlicenseType { set; get; }

       public string DriverName { set; get; }

       public int DrivedYears { set; get; }
       public int DriveCount { set; get; }
       public int Grade { set; get; }
       public decimal Deposit { set; get; }
       public int DisposableGoodsCount { set; get; }

       public string Phone { set; get; }

       public string BirthPlace { set; get; }

       public double Lat { set; get; }
       public double Lng { set; get; }

       public int Online { set; get; }

       public int OnService { set; get; }
   }
}
