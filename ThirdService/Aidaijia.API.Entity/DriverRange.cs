using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aidaijia.API.Entity
{
  public  class DriverRange
    {
      public string DriverName { set; get; }
      public double Lat { set; get; }
      public double Lng { set; get; }
      public string Ucode { set; get; }

      public int Online { set; get; }

      public int OnService { set; get; }
    }
}
