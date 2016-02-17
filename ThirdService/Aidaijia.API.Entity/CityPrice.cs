using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aidaijia.API.Entity
{
  public  class CityPrice
    {
     public string  StartTime{set;get;}
     public string  EndTime{set;get;}
     public double StartMileage{set;get;}      
     public double StartPrice{set;get;}
     public double PMoney{set;get;}
     public double UnitMileage{set;get;}
     public double UnitPrice{set;get;}
     public double WaitUnitTime{set;get;}
     public double WaitUnitPrice { set; get; }
    }
}
