using System;
namespace WebModel
{
  /// <summary>
  /// 此处添加类的说明
  /// </summary>
  [Serializable]
	public class DistributeOrderInfoEntity
	{
        private int _id;
        private string _distributeOrderId;
        private string _appointOrderId;
        private string _orderId;
        private int? _businesstype;
        private int? _orderfrom;
        private int? _ordertype;
        private int? _status;
        private int? _distributeusertype;
        private string _distributeuser;
        private string _cellphone;
        private string _fromcellphone;
        private string _address;
        private DateTime? _lastdistributeordertime;
        private DateTime? _successtime;
        private string _ucode;
        private int? _ondoservice = 0;
        private float? _lng;
        private float? _lat;
        private string _customerid;
        private string _customername;
        private int? _customertype;
        private int? _accounttype;
        private DateTime? _appointmenttime;
        private DateTime? _create_time;
        private string _create_user;
        private DateTime? _update_time;
        private string _update_user;
        private bool _delete_flag;
        private DateTime? _delete_time;
        private string _delete_user;
        private string _remark;
        private int _isLock;
        private string _lockUser;
        private int _partyId;
        /// <summary>
        /// 第3方合作商ID
        /// </summary>
        public int PartyId
        {
            get { return this._partyId; }
            set { this._partyId = value; }
        }
        public string LockUser
        {
            get { return _lockUser; }
            set { _lockUser = value; }
        }

        public int IsLock
        {
            get { return _isLock; }
            set { _isLock = value; }
        }

        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
		
        public string Fromcellphone
        {
            get { return _fromcellphone; }
            set { _fromcellphone = value; }
        }
		/// <summary>
		/// 
		/// </summary>
		public int Id
		{
		    get { return this._id; }
		    set { this._id = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		public string DistributeOrderId
		{
		    get { return this._distributeOrderId; }
		    set { this._distributeOrderId = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		public string AppointOrderId
		{
		    get { return this._appointOrderId; }
		    set { this._appointOrderId = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		public string OrderId
		{
		    get { return this._orderId; }
		    set { this._orderId = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		public int? BusinessType
		{
		    get { return this._businesstype; }
		    set { this._businesstype = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		public int? OrderFrom
		{
		    get { return this._orderfrom; }
		    set { this._orderfrom = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		public int? OrderType
		{
		    get { return this._ordertype; }
		    set { this._ordertype = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		public int? Status
		{
		    get { return this._status; }
		    set { this._status = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		public int? DistributeUserType
		{
		    get { return this._distributeusertype; }
		    set { this._distributeusertype = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		public string DistributeUser
		{
		    get { return this._distributeuser; }
		    set { this._distributeuser = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		public string Cellphone
		{
		    get { return this._cellphone; }
		    set { this._cellphone = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		public string Address
		{
		    get { return this._address; }
		    set { this._address = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		public DateTime? SuccessTime
		{
		    get { return this._successtime; }
		    set { this._successtime = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		public string Ucode
		{
		    get { return this._ucode; }
		    set { this._ucode = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
        public float? Lng
		{
		    get { return this._lng; }
		    set { this._lng = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
        public float? Lat
		{
		    get { return this._lat; }
		    set { this._lat = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		public string CustomerId
		{
		    get { return this._customerid; }
		    set { this._customerid = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		public string CustomerName
		{
		    get { return this._customername; }
		    set { this._customername = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		public int? CustomerType
		{
		    get { return this._customertype; }
		    set { this._customertype = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		public int? AccountType
		{
		    get { return this._accounttype; }
		    set { this._accounttype = value; }
		}	
		
		/// <summary>
		/// 
		/// </summary>
		public DateTime? AppointmentTime
		{
		    get { return this._appointmenttime; }
		    set { this._appointmenttime = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Create_time
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
		public DateTime? Update_time
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
		public DateTime? Delete_time
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
