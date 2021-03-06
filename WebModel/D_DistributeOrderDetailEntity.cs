using System;
namespace WebModel
{
  /// <summary>
  /// 此处添加类的说明
  /// </summary>
  [Serializable]
	public class DistributeOrderDetailEntity
	{
		private int _id;
		
		private string _distributeOrderId;
		
		private int _status;
		
		private string _ucode;
		
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
		public int Status
		{
		    get { return this._status; }
		    set { this._status = value; }
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
