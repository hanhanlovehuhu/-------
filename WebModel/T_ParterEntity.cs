//业务实体
//******************************************************************
//功能:
//
//历史: 2013/12/29 
//说明: 这是由一个工具自动生成的代码
//******************************************************************/
using System;
using System.Text;
namespace WebModel
{
  /// <summary>
  /// 此处添加类的说明
  /// </summary>
  [Serializable]
	public class T_ParterEntity
	{
		private int _id;
		
		private string _logname;
		
		private string _logpwd;
		
		private string _name;
		
		private string _phone;
		
		private string _pic;
		
		private string _sign;
		
		private decimal _fhmoney;
		
		private int _state;
		
		private int _daycount;
		
		private DateTime _addtime;

        public string url { get; set; }
		
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
		public string Logname
		{
		    get { return this._logname; }
		    set { this._logname = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		public string Logpwd
		{
		    get { return this._logpwd; }
		    set { this._logpwd = value; }
		}
		
		/// <summary>
		/// 公司名称
		/// </summary>
		public string Name
		{
		    get { return this._name; }
		    set { this._name = value; }
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
		/// 公司LOGO
		/// </summary>
		public string Pic
		{
		    get { return this._pic; }
		    set { this._pic = value; }
		}
		
		/// <summary>
		/// MD5验证值
		/// </summary>
		public string Sign
		{
		    get { return this._sign; }
		    set { this._sign = value; }
		}
		
		/// <summary>
		/// 每出一单给多少钱
		/// </summary>
		public decimal Fhmoney
		{
		    get { return this._fhmoney; }
		    set { this._fhmoney = value; }
		}
		
		/// <summary>
		/// 1、测试2、上线3、无效
		/// </summary>
		public int State
		{
		    get { return this._state; }
		    set { this._state = value; }
		}
		
		/// <summary>
		/// 每天允许调用次数
		/// </summary>
		public int Daycount
		{
		    get { return this._daycount; }
		    set { this._daycount = value; }
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
