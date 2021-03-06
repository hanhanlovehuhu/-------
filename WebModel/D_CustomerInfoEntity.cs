using System;
namespace WebModel
{
  /// <summary>
  /// 客户信息
  /// </summary>
  [Serializable]

    public class CustomerInfoEntity
    {
        public string ParentPhone { get; set; }
        private int _id;

        private string _customerId;

        private string _customerName;

        private string _recommendCode;

        private string _cellphone;

        private string _customerPhone;

        private int _registerFrom;

        private string _registerVersion;

        private int _provinceId;

        private int _cityId;

        private int _districtId;

        private string _email;

        private string _homeAddress;

        private int _customerType;

        private int _accountType;

       private bool _isNewCustomer;

        private int _lastDriverType;

        private string _lastDriver;

        private int _lastDriverCounts;

        private int _lastDriverUrgent;

        private DateTime _lastServiceTime;

        private string _lastCalledAddress;

        private string _lastAppointmentTime;

        private DateTime _lastLoginTime;

        private decimal _amount;

        private int _businessmanId;

        private string _remark;

        private DateTime _create_time;

        private string _create_user;

        private DateTime _update_time;

        private string _update_user;

        private int _delete_flag;

        private DateTime _delete_time;

        private string _delete_user;

        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            get { return this._id; }
            set { this._id = value; }
        }

        /// <summary>
        /// 用户编号
        /// </summary>
        public string CustomerId
        {
            get { return this._customerId; }
            set { this._customerId = value; }
        }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string CustomerName
        {
            get { return this._customerName; }
            set { this._customerName = value; }
        }

        /// <summary>
        /// 用户唯一推荐码
        /// </summary>
        public string RecommendCode
        {
            get { return this._recommendCode; }
            set { this._recommendCode = value; }
        }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Cellphone
        {
            get { return this._cellphone; }
            set { this._cellphone = value; }
        }

        /// <summary>
        /// 客户电话
        /// </summary>
        public string CustomerPhone
        {
            get { return this._customerPhone; }
            set { this._customerPhone = value; }
        }

        /// <summary>
        /// 注册来源
        /// </summary>
        public int RegisterFrom
        {
            get { return this._registerFrom; }
            set { this._registerFrom = value; }
        }

        /// <summary>
        /// 注册版本号
        /// </summary>
        public string RegisterVersion
        {
            get { return this._registerVersion; }
            set { this._registerVersion = value; }
        }

        /// <summary>
        /// 省ID
        /// </summary>
        public int ProvinceId
        {
            get { return this._provinceId; }
            set { this._provinceId = value; }
        }

        /// <summary>
        /// 市ID
        /// </summary>
        public int CityId
        {
            get { return this._cityId; }
            set { this._cityId = value; }
        }

        /// <summary>
        /// 区ID
        /// </summary>
        public int DistrictId
        {
            get { return this._districtId; }
            set { this._districtId = value; }
        }

        /// <summary>
        /// 邮件地址
        /// </summary>
        public string Email
        {
            get { return this._email; }
            set { this._email = value; }
        }

        /// <summary>
        /// 家庭住址
        /// </summary>
        public string HomeAddress
        {
            get { return this._homeAddress; }
            set { this._homeAddress = value; }
        }

        /// <summary>
        /// 用户类型1：个人；2：集团
        /// </summary>
        public int CustomerType
        {
            get { return this._customerType; }
            set { this._customerType = value; }
        }

        /// <summary>
        /// 账户类型1：VIP；2：普通
        /// </summary>
        public int AccountType
        {
            get { return this._accountType; }
            set { this._accountType = value; }
        }

        ///// <summary>
        ///// 是否新用户只有当一个订单成功的时候，此字段才为false            
        ///// </summary>
        public bool IsNewCustomer
        {
            get { return this._isNewCustomer; }
            set { this._isNewCustomer = value; }
        }

        /// <summary>
        /// 上次代驾类型
        /// </summary>
        public int LastDriverType
        {
            get { return this._lastDriverType; }
            set { this._lastDriverType = value; }
        }

        /// <summary>
        /// 上次代驾指定的司机
        /// </summary>
        public string LastDriver
        {
            get { return this._lastDriver; }
            set { this._lastDriver = value; }
        }

        /// <summary>
        /// 上次代驾数
        /// </summary>
        public int LastDriverCounts
        {
            get { return this._lastDriverCounts; }
            set { this._lastDriverCounts = value; }
        }

        /// <summary>
        /// 上次代驾紧急程度
        /// </summary>
        public int LastDriverUrgent
        {
            get { return this._lastDriverUrgent; }
            set { this._lastDriverUrgent = value; }
        }

        /// <summary>
        /// 上一次服务时间
        /// </summary>
        public DateTime LastServiceTime
        {
            get { return this._lastServiceTime; }
            set { this._lastServiceTime = value; }
        }

        /// <summary>
        /// 上一次叫代驾地址
        /// </summary>
        public string LastCalledAddress
        {
            get { return this._lastCalledAddress; }
            set { this._lastCalledAddress = value; }
        }

        /// <summary>
        /// 上次预约时间（字符串）yyyy-MM-dd-HH-mm（字符串）
        /// </summary>
        public string LastAppointmentTime
        {
            get { return this._lastAppointmentTime; }
            set { this._lastAppointmentTime = value; }
        }

        /// <summary>
        /// 最后一次登录时间
        /// </summary>
        public DateTime LastLoginTime
        {
            get { return this._lastLoginTime; }
            set { this._lastLoginTime = value; }
        }

        /// <summary>
        /// 账户余额
        /// </summary>
        public decimal Amount
        {
            get { return this._amount; }
            set { this._amount = value; }
        }

        /// <summary>
        /// 业务员ID
        /// </summary>
        public int BusinessmanId
        {
            get { return this._businessmanId; }
            set { this._businessmanId = value; }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this._remark; }
            set { this._remark = value; }
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
