using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aidaijia.API.Entity
{
  public  class HistoryConditions
    {
      public string CellPhone { set; get; }

      public string FromCellPhone { set; get; }

      public string Ucode { set; get; }

      public string BeginTime { set; get; }

      public string EndTime { set; get; }

      public int BusinessType { set; get; }

      public int OrderFrom { set; get; }
      public int Status { set; get; }

      public string OrderId { set; get; }
      public int PageSize { set; get; }
      public int StartIndex { set; get; }
    }
}
