using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aidaijia.API.Entity
{
   public class HistoryOrderPage
    {
       public int PageCount { set; get; }
       public List<HistoryOrderEntity> OrderList { set; get; }
    }
}
