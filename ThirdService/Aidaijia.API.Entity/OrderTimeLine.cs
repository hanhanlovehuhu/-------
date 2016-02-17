using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aidaijia.API.Entity
{
   public class OrderTimeLine
    {
       public string OrderTime { set; get; }
       public string ArriveTime { set; get; }
       public string BeginTime { set; get; }
       public string EndTime { set; get; }
       public int Status { set; get; }
    }
}
