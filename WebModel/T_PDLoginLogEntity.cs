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
    public partial class T_ParterDyLog
    {
        public T_ParterDyLog()
        { }
        #region Model
        private int _id;
        private string _sign;
        private int? _typeid;
        private string _imei;
        private string _lat;
        private string _lng;
        private DateTime? _addtime;
        /// <summary>
        /// 
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string sign
        {
            set { _sign = value; }
            get { return _sign; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? typeid
        {
            set { _typeid = value; }
            get { return _typeid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string imei
        {
            set { _imei = value; }
            get { return _imei; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string lat
        {
            set { _lat = value; }
            get { return _lat; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string lng
        {
            set { _lng = value; }
            get { return _lng; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? addtime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }
        #endregion Model

    }
}
