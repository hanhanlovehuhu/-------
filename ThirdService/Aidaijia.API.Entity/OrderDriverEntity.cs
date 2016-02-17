using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aidaijia.API.Entity
{
   public class OrderDriverEntity
    {
       public string OrderId { set; get; }

       public int BusinessType { set; get; }

       public string AppointTime { set; get; }

       public string Address { set; get; }

       public string CellPhone { set; get; }

       public double Lat { set; get; }

       public double Lng { set; get; }

       public string Remark { set; get; }

       public decimal VipPrice { set; get; }

       public string OrderTime { set; get; }

       public string ArrivedTime { set; get; }

       public decimal StartPrice { set; get; }

       public int StartMileage { set; get; }

       public int UnitMileage { set; get; }

       public decimal UnitPrice { set; get; }

       public int WaitUnitTime { set; get; }

       public decimal WaitUnitPrice { set; get; }

       public int IsFixed { set; get; }

       public decimal FixedPrice { set; get; }

       public string BeginTime { set; get; }

       public decimal CusSubsidiesMoney { set; get; }

       public decimal AppSubsidiesMoney { set; get; }

       public int Status { set; get; }

       public string Ucode { set; get; }

       public string Error { set; get; }
    }
}
