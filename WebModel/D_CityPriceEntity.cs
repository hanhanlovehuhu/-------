using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebModel
{
    public partial class D_CityPriceEntity
    {
        public D_CityPriceEntity()
        { }
        #region Model
        private string _citypriceid;
        private int? _cityid;
        private string _cityname;
        private string _citypinyin;
        private int? _businesstype;
        private int? _version;
        private int? _status;
        private string _remark;
        private string _businessremark;
        private DateTime? _create_time;
        private string _create_user;
        private DateTime? _update_time;
        private string _update_user;
        private bool _delete_flag;
        private DateTime? _delete_time;
        private string _delete_user;
        /// <summary>
        /// 
        /// </summary>
        public string CityPriceId
        {
            set { _citypriceid = value; }
            get { return _citypriceid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CityId
        {
            set { _cityid = value; }
            get { return _cityid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CityName
        {
            set { _cityname = value; }
            get { return _cityname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CityPinYin
        {
            set { _citypinyin = value; }
            get { return _citypinyin; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? BusinessType
        {
            set { _businesstype = value; }
            get { return _businesstype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Version
        {
            set { _version = value; }
            get { return _version; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BusinessRemark
        {
            set { _businessremark = value; }
            get { return _businessremark; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? create_time
        {
            set { _create_time = value; }
            get { return _create_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string create_user
        {
            set { _create_user = value; }
            get { return _create_user; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? update_time
        {
            set { _update_time = value; }
            get { return _update_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string update_user
        {
            set { _update_user = value; }
            get { return _update_user; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool delete_flag
        {
            set { _delete_flag = value; }
            get { return _delete_flag; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? delete_time
        {
            set { _delete_time = value; }
            get { return _delete_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string delete_user
        {
            set { _delete_user = value; }
            get { return _delete_user; }
        }
        #endregion Model

    }
}
