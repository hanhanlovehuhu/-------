using System;
namespace WebModel
{
  /// <summary>
  /// 此处添加类的说明
  /// </summary>
  [Serializable]
	public class ClientInfoEntity
	{
		private string _id;
		
		private string _phone;
		
		private string _realname;
		
		private string _phone2;
		
		private int _provinceId;
		
		private int _cityId;
		
		private int _districtId;
		
		private string _email;
		
		private string _homeAddress;
		
		private int _userType;
		
		private string _lastCalledAddress;
		
		private string _lastYuyueTime;
		
		private decimal _usableMoney;
		
		private int _individualOrGroup;
		
		private int _businessmanId;
		
		private DateTime _addtime;
		
		/// <summary>
		/// 
		/// </summary>
		public string Id
		{
		    get { return this._id; }
		    set { this._id = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		public string Phone
		{
		    get { return this._phone; }
		    set { this._phone = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		public string Realname
		{
		    get { return this._realname; }
		    set { this._realname = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		public string Phone2
		{
		    get { return this._phone2; }
		    set { this._phone2 = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		public int ProvinceId
		{
		    get { return this._provinceId; }
		    set { this._provinceId = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		public int CityId
		{
		    get { return this._cityId; }
		    set { this._cityId = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		public int DistrictId
		{
		    get { return this._districtId; }
		    set { this._districtId = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		public string Email
		{
		    get { return this._email; }
		    set { this._email = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		public string HomeAddress
		{
		    get { return this._homeAddress; }
		    set { this._homeAddress = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		public int UserType
		{
		    get { return this._userType; }
		    set { this._userType = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		public string LastCalledAddress
		{
		    get { return this._lastCalledAddress; }
		    set { this._lastCalledAddress = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		public string LastYuyueTime
		{
		    get { return this._lastYuyueTime; }
		    set { this._lastYuyueTime = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		public decimal UsableMoney
		{
		    get { return this._usableMoney; }
		    set { this._usableMoney = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		public int IndividualOrGroup
		{
		    get { return this._individualOrGroup; }
		    set { this._individualOrGroup = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		public int BusinessmanId
		{
		    get { return this._businessmanId; }
		    set { this._businessmanId = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		public DateTime Addtime
		{
		    get { return this._addtime; }
		    set { this._addtime = value; }
		}
		
	}
}
