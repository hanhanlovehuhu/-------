using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aidaijia.API.Entity
{
   public class OrderStatus
    {
       /// <summary>
       /// 状态
       /// </summary>
       public string Status { set; get; }

       /// <summary>
       /// 错误
       /// </summary>
       public string OrderId { set; get; }
    }
}
