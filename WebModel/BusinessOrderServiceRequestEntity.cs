using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebModel
{
    public class BusinessOrderServiceRequestEntity
    {
        public string Name { get; set; }                  //预约人称呼
        public string Phone { get; set; }                   //预约人手机号
        public string Address { get; set; }             // 需要代驾的起点
        public string AllUserCount { get; set; }           //叫单人数
        public string AppointTime { get; set; }             //预约时间
        public string CalledNo { get; set; }               //被叫号码
        public string CarNumber { get; set; }              //车辆号码
        public string CellPhone { get; set; }              //账户号码
        public string DriverRemark { get; set; }           //备注
        public double Lat { get; set; }                 // 纬度
        public double Lng { get; set; }                 // 经度
        public string From { get; set; }               //默认值为8
        public string Sign { get; set; }              //合作商标识
        public string IMEI { get; set; }              //IMEI
        public string Remark { get; set; }             //备注
        public string SendCarUserName { get; set; }    //发车人姓名（商务订单使用）
        public string SendCarPhone { get; set; }        //发车人电话（商务订单使用）
        public double EndDriveLat { get; set; }            //结束代驾经纬度 （商务订单使用）
        public double EndDriveLng { get; set; }             //结束代驾经纬度（商务订单使用）
        public string EndDriveAddress { get; set; }         //结束代驾地址（商务订单使用）
        public string ReceiveCarPhone { get; set; }           //接车人电话
        public string ReceiveCarUserName { get; set; }          //接车人姓名
        public int Smart { get; set; }
        public int BusinessType { get; set; }               //商务订单类型
        public int OrderFrom { get; set; }                 //订单来源
        public DateTime OrderTime { get; set; }            //订单时间

    }       
}
