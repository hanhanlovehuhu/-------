using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebModel
{
    public partial class D_CityPriceDetailEntity
    {
        public D_CityPriceDetailEntity()
        { }
        #region Model
        private string _citypriceid;
        private string _starttime;
        private string _endtime;
        private int? _startmileage = 0;
        private decimal? _startprice;
        private decimal? _pmoney;
        private int? _unitmileage = 0;
        private decimal? _unitprice;
        private int? _waitunittime;
        private decimal? _waitunitprice;
        private int? _status;
        private string _remark;
        private DateTime? _create_time;
        private string _create_user;
        private DateTime? _update_time;
        private string _update_user;
        private bool _delete_flag;
        private DateTime? _delete_time;
        private string _delete_user;
        private decimal? _price;
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
        public string StartTime
        {
            set { _starttime = value; }
            get { return _starttime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EndTime
        {
            set { _endtime = value; }
            get { return _endtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? StartMileage
        {
            set { _startmileage = value; }
            get { return _startmileage; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? StartPrice
        {
            set { _startprice = value; }
            get { return _startprice; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? PMoney
        {
            set { _pmoney = value; }
            get { return _pmoney; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? UnitMileage
        {
            set { _unitmileage = value; }
            get { return _unitmileage; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? UnitPrice
        {
            set { _unitprice = value; }
            get { return _unitprice; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? WaitUnitTime
        {
            set { _waitunittime = value; }
            get { return _waitunittime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? WaitUnitPrice
        {
            set { _waitunitprice = value; }
            get { return _waitunitprice; }
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
        /// <summary>
        /// 
        /// </summary>
        public decimal? Price
        {
            set { _price = value; }
            get { return _price; }
        }
        #endregion Model

    }
}
