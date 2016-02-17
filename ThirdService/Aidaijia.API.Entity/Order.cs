using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aidaijia.API.Entity
{
    public class Order
    {
        /// <summary>
        /// 订单号码
        /// </summary>
        public string OrderId { set; get; }

        /// <summary>
        /// 1，酒后代驾；2，婚庆代驾；3，商务代驾；4，长途代驾；5，旅游代驾
        /// </summary>
        public int BusinessType { set; get; }

        /// <summary>
        /// 1:400,2:android APP,3:ios APP 4:爱推广,5:合作商
        /// </summary>
        public int OrderFrom { set; get; }

        /// <summary>
        /// 订单建立时间
        /// </summary>
        public string OrderTime { set; get; }



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
        /// 爱代驾自己平台为0其它为合作商ID
        /// </summary>
        public int Parterid { set; get; }


        /// <summary>
        /// 预约时间
        /// </summary>
        public string AppointTime { set; get; }

        /// <summary>
        /// 订单地址
        /// </summary>
        public string Address { set; get; }

        /// <summary>
        /// 纬度
        /// </summary>
        public double Lat { set; get; }

        /// <summary>
        /// 经度
        /// </summary>
        public double Lng { set; get; }

        /// <summary>
        /// 订单总人数
        /// </summary>
        public int AllUserCount { set; get; }


        /// <summary>
        /// 司机纬度
        /// </summary>
        public double DriverLat { set; get; }

        /// <summary>
        /// 司机经度
        /// </summary>
        public double DriverLng { set; get; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { set; get; }
       
        /// <summary>
        /// 0 新建订单  1客服指派订单 2分派订单 3成功 4司机无响应 5无可派司机 6客服取消 7 用户取消 8司机取消
        ///9 司机到达 10 司机开始代驾 11司机结束代驾 12司机报单 13报单失败
        /// </summary>
        /// 
        public int Status { set; get; }

        public string CarNumber { set; get; }

        public string CustomerName { set; get; }

        public string RecommendCode { set; get; }

        public int PhoneCallTime { set; get; }

        public int SendUserId { set; get; }

        public string SendUserName { set; get; }

        public string CreateUser { set; get; }

        /// <summary>
        /// 里程
        /// </summary>
        public double Mileage { set; get; }

        /// <summary>
        /// 里程费用
        /// </summary>
        public decimal MileageMoney { set; get; }

        /// <summary>
        /// 等待时间
        /// </summary>
        public double WaitTime { set; get; }

        /// <summary>
        /// 等待费用
        /// </summary>
        public decimal WaitMoney { set; get; }

        /// <summary>
        /// 总金额
        /// </summary>
        public decimal AllMoney { set; get; }

        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal DiscountMoney { set; get; }

        public string DriverPhone { set; get; }

        /// <summary>
        /// 司机姓名
        /// </summary>
        public string DriverName { set; get; }

        /// <summary>
        /// 开始代驾时间
        /// </summary>
        public string BeginDriveTime { set; get; }

        /// <summary>
        /// 结束代驾时间
        /// </summary>
        public string EndDriveTime { set; get; }
        
        /// <summary>
        /// 到达时间
        /// </summary>
        public string ArrivedTime { set; get; }
    }
}
