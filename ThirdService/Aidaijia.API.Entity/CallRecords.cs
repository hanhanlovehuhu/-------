using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aidaijia.API.Entity
{
   public class CallRecords
    {
       public string CallType { set; get; }

       public string CallAddress { set; get; }

       public string CallNo { set; get; }

       public string CalledNo { set; get; }


       public string Seat { set; get; }


       public string CustomerService { set; get; }

       public string RingTime { set; get; }

       public string StartTime { set; get; }


       public string EndTime { set; get; }

       public string CallTime { set; get; }

       public string SoundRecord { set; get; }

       public int InCallType { set; get; }

    }
}
