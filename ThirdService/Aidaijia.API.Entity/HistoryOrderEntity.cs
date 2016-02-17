using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aidaijia.API.Entity
{
   public class HistoryOrderEntity
    {
       public string OrderId { set; get; }

       public int OrderFrom { set; get; }

       public int BusinessType { set; get; }

       public string CustomerName { set; get; }

       public string CellPhone { set; get; }

       public string FromCellPhone { set; get; }

       public string Ucode { set; get; }
     
       public int Status { set; get; }

       public string CreateTime { set; get; }

       public string AppointTime { set; get; }
    
       public string Address { set; get; }

   }
}
