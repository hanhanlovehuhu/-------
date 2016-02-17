using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aidaijia.API.Entity
{
  public  class OrderDriver
    {
        public bool IsSuccess { set; get; }

        public string ErrorMsg { set; get; }

        public int ErrorCode { set; get; }

        public OrderDriverEntity Result { set; get; }
      
    }
}
