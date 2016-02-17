using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aidaijia.API.Entity
{
  public  class Customer
    {
      /// <summary>
      /// 客户名称
      /// </summary>
      public string CustomerName { set; get; }
      /// <summary>
      /// 客户id
      /// </summary>
      public string CustomerId { set; get; }

      /// <summary>
      /// 客户类型
      /// </summary>
      public int CustomerType { set; get; }

      /// <summary>
      /// 客户余额
      /// </summary>
      public decimal Amount { set; get; }

      /// <summary>
      /// 客户电话
      /// </summary>
      public string CellPhone { set; get; }
  }
}
