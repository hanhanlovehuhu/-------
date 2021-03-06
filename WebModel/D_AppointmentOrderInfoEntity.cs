using System;
namespace WebModel
{
    /// <summary>
    /// 用来记录所有的预约订单
    /// </summary>
    [Serializable]
    public class AppointmentOrderInfoEntity
    {
        private int _id;

        private string _appointOrderId;

        private string _orderId;

        private int _appointOrderType;

        private int _appointOrderState;

        private int _businessType;

        private int _appointOrderFrom;

        private string _customerId;

        private string _customerName;

        private string _cellphone;

        private string _fromCellphone;

        private string _appointAddress;

        private string _appointTimeStr;

        private DateTime _appointTime;

        private DateTime _estimateEndTime;

        private string _lockUser;

        private string _sendOrderUser;

        private string _appointOrderSign;

        private int _phoneCallTime;

        private decimal _lng;

        private decimal _lat;

        private DateTime _create_time;

        private string _create_user;

        private DateTime _update_time;

        private string _update_user;

        private int _delete_flag;

        private DateTime _delete_time;

        private string _delete_user;
        private int _customerType;
        private int _accountType;
        private int _partyId;
        /// <summary>
        /// 第3方合作商ID
        /// </summary>
        public int PartyId
        {
            get { return this._partyId; }
            set { this._partyId = value; }
        }
        public int AccountType
        {
            get { return this._accountType; }
            set { this._accountType = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int CustomerType
        {
            get { return this._customerType; }
            set { this._customerType = value; }
        }

        /// <summary>
        /// 预约订单编号
        /// </summary>
        public string AppointOrderId
        {
            get { return this._appointOrderId; }
            set { this._appointOrderId = value; }
        }

        /// <summary>
        /// 订单编号（此张预约订单转成的订单）
        /// </summary>
        public string OrderId
        {
            get { return this._orderId; }
            set { this._orderId = value; }
        }

        /// <summary>
        /// 预约订单的类型1，自己叫2，代叫3，代付
        /// </summary>
        public int AppointOrderType
        {
            get { return this._appointOrderType; }
            set { this._appointOrderType = value; }
        }

        /// <summary>
        /// 预约订单的状态"10，已提交21，经纬度解析成功30，已派单；31，智能派单无司机响应；32，已派单；41，经纬度地址解析失败51,客服取消；52，系统取消53，地址错误无法派单，54，客户取消"
        /// </summary>
        public int AppointOrderState
        {
            get { return this._appointOrderState; }
            set { this._appointOrderState = value; }
        }

        /// <summary>
        /// 业务类型10，酒后代驾；20，婚庆代驾；30，商务代驾；40，长途代驾；50，旅游代驾
        /// </summary>
        public int BusinessType
        {
            get { return this._businessType; }
            set { this._businessType = value; }
        }

        /// <summary>
        /// 预约单来源	"1:400,2:android APP,3:ios APP 4:U信拍,5:大众点评,6:爱推广,7:酒店B2B"
        /// </summary>
        public int AppointOrderFrom
        {
            get { return this._appointOrderFrom; }
            set { this._appointOrderFrom = value; }
        }

        /// <summary>
        /// 客户编号
        /// </summary>
        public string CustomerId
        {
            get { return this._customerId; }
            set { this._customerId = value; }
        }

        /// <summary>
        /// 客户姓名
        /// </summary>
        public string CustomerName
        {
            get { return this._customerName; }
            set { this._customerName = value; }
        }

        /// <summary>
        /// 客户手机号码
        /// </summary>
        public string Cellphone
        {
            get { return this._cellphone; }
            set { this._cellphone = value; }
        }

        /// <summary>
        /// 来电号码
        /// </summary>
        public string FromCellphone
        {
            get { return this._fromCellphone; }
            set { this._fromCellphone = value; }
        }

        /// <summary>
        /// 预约地址
        /// </summary>
        public string AppointAddress
        {
            get { return this._appointAddress; }
            set { this._appointAddress = value; }
        }

        /// <summary>
        /// 预约时间（字符串）yyyy-MM-dd-HH-mm
        /// </summary>
        public string AppointTimeStr
        {
            get { return this._appointTimeStr; }
            set { this._appointTimeStr = value; }
        }

        /// <summary>
        /// 预约时间
        /// </summary>
        public DateTime AppointTime
        {
            get { return this._appointTime; }
            set { this._appointTime = value; }
        }

        /// <summary>
        /// 预计代驾结束时间
        /// </summary>
        public DateTime EstimateEndTime
        {
            get { return this._estimateEndTime; }
            set { this._estimateEndTime = value; }
        }

        /// <summary>
        /// 锁定人
        /// </summary>
        public string LockUser
        {
            get { return this._lockUser; }
            set { this._lockUser = value; }
        }

        /// <summary>
        /// 派单人
        /// </summary>
        public string SendOrderUser
        {
            get { return this._sendOrderUser; }
            set { this._sendOrderUser = value; }
        }

        /// <summary>
        /// 客户手工派单客户标签
        /// </summary>
        public string AppointOrderSign
        {
            get { return this._appointOrderSign; }
            set { this._appointOrderSign = value; }
        }

        /// <summary>
        /// 预约订单接听时间（秒）
        /// </summary>
        public int PhoneCallTime
        {
            get { return this._phoneCallTime; }
            set { this._phoneCallTime = value; }
        }

        /// <summary>
        /// 经度
        /// </summary>
        public decimal Lng
        {
            get { return this._lng; }
            set { this._lng = value; }
        }

        /// <summary>
        /// 纬度
        /// </summary>
        public decimal Lat
        {
            get { return this._lat; }
            set { this._lat = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Create_time
        {
            get { return this._create_time; }
            set { this._create_time = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Create_user
        {
            get { return this._create_user; }
            set { this._create_user = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Update_time
        {
            get { return this._update_time; }
            set { this._update_time = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Update_user
        {
            get { return this._update_user; }
            set { this._update_user = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Delete_flag
        {
            get { return this._delete_flag; }
            set { this._delete_flag = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Delete_time
        {
            get { return this._delete_time; }
            set { this._delete_time = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Delete_user
        {
            get { return this._delete_user; }
            set { this._delete_user = value; }
        }

    }
}
