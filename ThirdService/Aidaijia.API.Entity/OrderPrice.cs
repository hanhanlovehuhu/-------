using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aidaijia.API.Entity
{
  public  class OrderPrice
    {
      public double SSMoney { set; get; }

      public double MileageMoney { set; get; }

      public double WaitMoney { set; get; }

      public double WaitTime { set; get; }

      public int State { set; get; }

      public double Mileage { set; get; }
    }
}
