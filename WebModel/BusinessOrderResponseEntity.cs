using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebModel
{
    public class BusinessOrderResponseEntity
    {
        public string Result { get; set; }
        public string OrderId { get; set; }
        public string Status { get; set; }
        public string ErrorMsg { get; set; }
    }
}
