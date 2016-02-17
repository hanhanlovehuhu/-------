using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aidaijia.API.Entity
{
   public class HistoryOrderDetail:HistoryOrderEntity
    {
        public int AccountType { set; get; }

        public decimal SSMoney { set; get; }

        public decimal MileageMoney { set; get; }

        public decimal Money { set; get; }

        public decimal WaitMoney { set; get; }

        public decimal OtherMoney { set; get; }

        public int WaitTime { set; get; }

        public string BeginDriveTime { set; get; }

        public string EndDriveTime { set; get; }
      

        public double Mileage { set; get; }

        public decimal BalanceAmount { set; get; }
        public decimal CashAmount { set; get; }

        public string BeginDriveAddress { set; get; }

        public string EndDriveAddress { set; get; }
    }
}
