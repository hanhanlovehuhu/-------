using System;
namespace WebModel
{
    /// <summary>
    /// 司机信息表
    /// </summary>
    [Serializable]
    public class DriverInfoEntity
    {
        private int _id;

        private string _ucode;

        private string _password;

        private string _driverName;

        private int _provinceId;

        private int _cityId;

        private int _rating;

        private int _districtId;

        private string _phone;

        private string _tel;

        private string _birthPlace;

        private string _address;

        private int _sex;

        private string _photo;

        private string _identificationPhoto;

        private int _identificationType;

        private string _iMEI;

        private string _identificationCardcode;

        private string _bankcardCode;

        private decimal _usableMoney;

        private string _remark;

        private int _drivedYears;

        private string _driverlicenseCode;

        private string _driverlicenseType;

        private string _licensecodePhoto;

        private DateTime _getLicensecodeDate;

        private int _haveCar;

        private string _carBrand;

        private string _carType;

        private string _carNumber;

        private string _telModelNumber;

        private int _online;

        private int _onService;

        private int _onDOService;

        private int _state;

        private int _rzState;

        private int _driveCount;

        private int _priority;

        private int _grade;

        private decimal _deposit;

        private int _disposableGoodsCount;

        private int _businessType;

        private string _refundPeople;

        private DateTime _turnoverTime;

        private DateTime _refundTime;

        private DateTime _addtime;

        private DateTime _create_time;

        private string _create_user;

        private DateTime _update_time;

        private string _update_user;

        private bool _delete_flag;

        private DateTime _delete_time;

        private string _delete_user;

        /// <summary>
        /// 自增ID
        /// </summary>
        public int Rating
        {
            get { return this._rating; }
            set { this._rating = value; }
        }
        public int OnDOService
        {
            get { return this._onDOService; }
            set { this._onDOService = value; }
        }

        /// <summary>
        /// 自增ID
        /// </summary>
        public int Id
        {
            get { return this._id; }
            set { this._id = value; }
        }

        /// <summary>
        /// 司机工号
        /// </summary>
        public string Ucode
        {
            get { return this._ucode; }
            set { this._ucode = value; }
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get { return this._password; }
            set { this._password = value; }
        }

        /// <summary>
        /// 姓名
        /// </summary>
        public string DriverName
        {
            get { return this._driverName; }
            set { this._driverName = value; }
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
        /// 手机号码
        /// </summary>
        public string Phone
        {
            get { return this._phone; }
            set { this._phone = value; }
        }

        /// <summary>
        /// 私人号码
        /// </summary>
        public string Tel
        {
            get { return this._tel; }
            set { this._tel = value; }
        }

        /// <summary>
        /// 籍贯
        /// </summary>
        public string BirthPlace
        {
            get { return this._birthPlace; }
            set { this._birthPlace = value; }
        }

        /// <summary>
        /// 居住地址
        /// </summary>
        public string Address
        {
            get { return this._address; }
            set { this._address = value; }
        }

        /// <summary>
        /// 姓别
        /// </summary>
        public int Sex
        {
            get { return this._sex; }
            set { this._sex = value; }
        }

        /// <summary>
        /// 头像
        /// </summary>
        public string Photo
        {
            get { return this._photo; }
            set { this._photo = value; }
        }

        /// <summary>
        /// 证件照
        /// </summary>
        public string IdentificationPhoto
        {
            get { return this._identificationPhoto; }
            set { this._identificationPhoto = value; }
        }

        /// <summary>
        /// 证件类型
        /// </summary>
        public int IdentificationType
        {
            get { return this._identificationType; }
            set { this._identificationType = value; }
        }

        /// <summary>
        /// 手机唯一识别号
        /// </summary>
        public string IMEI
        {
            get { return this._iMEI; }
            set { this._iMEI = value; }
        }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdentificationCardcode
        {
            get { return this._identificationCardcode; }
            set { this._identificationCardcode = value; }
        }

        /// <summary>
        /// 银行卡号
        /// </summary>
        public string BankcardCode
        {
            get { return this._bankcardCode; }
            set { this._bankcardCode = value; }
        }

        /// <summary>
        /// 可用余额
        /// </summary>
        public decimal UsableMoney
        {
            get { return this._usableMoney; }
            set { this._usableMoney = value; }
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
        /// 驾龄
        /// </summary>
        public int DrivedYears
        {
            get { return this._drivedYears; }
            set { this._drivedYears = value; }
        }

        /// <summary>
        /// 驾照编码
        /// </summary>
        public string DriverlicenseCode
        {
            get { return this._driverlicenseCode; }
            set { this._driverlicenseCode = value; }
        }

        /// <summary>
        /// 驾照类型
        /// </summary>
        public string DriverlicenseType
        {
            get { return this._driverlicenseType; }
            set { this._driverlicenseType = value; }
        }

        /// <summary>
        /// 相片
        /// </summary>
        public string LicensecodePhoto
        {
            get { return this._licensecodePhoto; }
            set { this._licensecodePhoto = value; }
        }

        /// <summary>
        /// 领证时间
        /// </summary>
        public DateTime GetLicensecodeDate
        {
            get { return this._getLicensecodeDate; }
            set { this._getLicensecodeDate = value; }
        }

        /// <summary>
        /// 是否有车
        /// </summary>
        public int HaveCar
        {
            get { return this._haveCar; }
            set { this._haveCar = value; }
        }

        /// <summary>
        /// 车品牌
        /// </summary>
        public string CarBrand
        {
            get { return this._carBrand; }
            set { this._carBrand = value; }
        }

        /// <summary>
        /// 车型号
        /// </summary>
        public string CarType
        {
            get { return this._carType; }
            set { this._carType = value; }
        }

        /// <summary>
        /// 车牌号码
        /// </summary>
        public string CarNumber
        {
            get { return this._carNumber; }
            set { this._carNumber = value; }
        }

        /// <summary>
        /// 手机型号
        /// </summary>
        public string TelModelNumber
        {
            get { return this._telModelNumber; }
            set { this._telModelNumber = value; }
        }

        /// <summary>
        ///  司机是否在线0，未在线；1，在线
        /// </summary>
        public int Online
        {
            get { return this._online; }
            set { this._online = value; }
        }

        /// <summary>
        ///司机是否在服务中0，未服务；1，服务中
        /// </summary>
        public int OnService
        {
            get { return this._onService; }
            set { this._onService = value; }
        }

        /// <summary>
        /// 状态（1、正常；2、欠费；3、休假；4，处罚；5，离职已退款；6，离职未退款）
        /// </summary>
        public int State
        {
            get { return this._state; }
            set { this._state = value; }
        }

        /// <summary>
        /// 入职状态
        /// </summary>
        public int RzState
        {
            get { return this._rzState; }
            set { this._rzState = value; }
        }

        /// <summary>
        /// 代驾次数
        /// </summary>
        public int DriveCount
        {
            get { return this._driveCount; }
            set { this._driveCount = value; }
        }

        /// <summary>
        /// 优先级（自动派单是的优先级，越大优先级越高）
        /// </summary>
        public int Priority
        {
            get { return this._priority; }
            set { this._priority = value; }
        }

        /// <summary>
        /// 评级
        /// </summary>
        public int Grade
        {
            get { return this._grade; }
            set { this._grade = value; }
        }

        /// <summary>
        /// 押金
        /// </summary>
        public decimal Deposit
        {
            get { return this._deposit; }
            set { this._deposit = value; }
        }

        /// <summary>
        /// 领取的一次性用品数量
        /// </summary>
        public int DisposableGoodsCount
        {
            get { return this._disposableGoodsCount; }
            set { this._disposableGoodsCount = value; }
        }

        /// <summary>
        /// 业务类型
        /// </summary>
        public int BusinessType
        {
            get { return this._businessType; }
            set { this._businessType = value; }
        }

        /// <summary>
        /// 退款人
        /// </summary>
        public string RefundPeople
        {
            get { return this._refundPeople; }
            set { this._refundPeople = value; }
        }

        /// <summary>
        /// 离职时间
        /// </summary>
        public DateTime TurnoverTime
        {
            get { return this._turnoverTime; }
            set { this._turnoverTime = value; }
        }

        /// <summary>
        /// 退款时间
        /// </summary>
        public DateTime RefundTime
        {
            get { return this._refundTime; }
            set { this._refundTime = value; }
        }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime Addtime
        {
            get { return this._addtime; }
            set { this._addtime = value; }
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
        public bool Delete_flag
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
