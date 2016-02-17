using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aidaijia.API.Entity
{
   public class Html5UnfinishedOrder
    {
        /// <summary>
        /// 订单号码
        /// </summary>
        public string OrderId { set; get; }

        /// <summary>
        /// 客户电话
        /// </summary>
        public string CellPhone { set; get; }

        /// <summary>
        /// 来电电话
        /// </summary>
        public string FromCellPhone { set; get; }
        /// <summary>
        /// 司机工号
        /// </summary>
        public string Ucode { set; get; }

        /// <summary>
        /// 预约时间
        /// </summary>
        public string AppointTime { set; get; }

        /// <summary>
        /// 0 新建订单  1客服指派订单 2分派订单 3成功 4司机无响应 5无可派司机 6客服取消 7 用户取消 8司机取消
        ///9 司机到达 10 司机开始代驾 11司机结束代驾 12司机报单 13报单失败
        /// </summary>
        /// 
        public int Status { set; get; }

        public string Photo { set; get; }

        public string Tel { set; get; }

        public string DriverName { set; get; }

        public int DriveCount { set; get; }
     
        public decimal Deposit { set; get; }    

        public string Phone { set; get; }
      
    }
}
