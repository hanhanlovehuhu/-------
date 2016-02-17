using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebModel
{
    public class D_OrderComplaint
    {
        public int Id { get; set; }
        public string OrderId { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Cellphone { get; set; }
        public string Ucode { get; set; }
        public string DriverName { get; set; }
        public int ComplaintType { get; set; }
        public string  ComplaintContent { get; set; }
        public string CreateUser { get; set; }
    }
}
