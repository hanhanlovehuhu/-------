using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aidaijia.API.Entity
{
   public class D_OrderDetailEntity
    {
        /// <summary>
        /// Id
        /// </summary>		
        private int? _Id;
        public int? Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        /// <summary>
        /// OrderId
        /// </summary>		
        private string _OrderId;
        public string OrderId
        {
            get { return _OrderId; }
            set { _OrderId = value; }
        }
        /// <summary>
        /// ProvinceId
        /// </summary>		
        private int? _ProvinceId;
        public int? ProvinceId
        {
            get { return _ProvinceId; }
            set { _ProvinceId = value; }
        }
        /// <summary>
        /// CityId
        /// </summary>		
        private int? _CityId;
        public int? CityId
        {
            get { return _CityId; }
            set { _CityId = value; }
        }
        /// <summary>
        /// DistrictId
        /// </summary>		
        private int? _DistrictId;
        public int? DistrictId
        {
            get { return _DistrictId; }
            set { _DistrictId = value; }
        }
        /// <summary>
        /// VipCode
        /// </summary>		
        private string _VipCode;
        public string VipCode
        {
            get { return _VipCode; }
            set { _VipCode = value; }
        }
        /// <summary>
        /// CusPhone
        /// </summary>		
        private string _CusPhone;
        public string CusPhone
        {
            get { return _CusPhone; }
            set { _CusPhone = value; }
        }
        /// <summary>
        /// ArrivedTime
        /// </summary>		
        private DateTime? _ArrivedTime;
        public DateTime? ArrivedTime
        {
            get { return _ArrivedTime; }
            set { _ArrivedTime = value; }
        }
        /// <summary>
        /// BeginDriveTime
        /// </summary>		
        private DateTime? _BeginDriveTime;
        public DateTime? BeginDriveTime
        {
            get { return _BeginDriveTime; }
            set { _BeginDriveTime = value; }
        }
        /// <summary>
        /// AddrivedDestinationTime
        /// </summary>		
        private DateTime? _AddrivedDestinationTime;
        public DateTime? AddrivedDestinationTime
        {
            get { return _AddrivedDestinationTime; }
            set { _AddrivedDestinationTime = value; }
        }
        /// <summary>
        /// Lichen
        /// </summary>		
        private int? _Lichen;
        public int? Lichen
        {
            get { return _Lichen; }
            set { _Lichen = value; }
        }
        /// <summary>
        /// ArrivedAddress
        /// </summary>		
        private string _ArrivedAddress;
        public string ArrivedAddress
        {
            get { return _ArrivedAddress; }
            set { _ArrivedAddress = value; }
        }
        /// <summary>
        /// ConfirmTime
        /// </summary>		
        private DateTime? _ConfirmTime;
        public DateTime? ConfirmTime
        {
            get { return _ConfirmTime; }
            set { _ConfirmTime = value; }
        }
        /// <summary>
        /// BDtime
        /// </summary>		
        private DateTime? _BDtime;
        public DateTime? BDtime
        {
            get { return _BDtime; }
            set { _BDtime = value; }
        }
        /// <summary>
        /// BDState
        /// </summary>		
        private int? _BDState;
        public int? BDState
        {
            get { return _BDState; }
            set { _BDState = value; }
        }
        /// <summary>
        /// CarAddress
        /// </summary>		
        private string _CarAddress;
        public string CarAddress
        {
            get { return _CarAddress; }
            set { _CarAddress = value; }
        }
        /// <summary>
        /// ReplenishedOrNot
        /// </summary>		
        private int? _ReplenishedOrNot;
        public int? ReplenishedOrNot
        {
            get { return _ReplenishedOrNot; }
            set { _ReplenishedOrNot = value; }
        }
        /// <summary>
        /// DriverRemark
        /// </summary>		
        private string _DriverRemark;
        public string DriverRemark
        {
            get { return _DriverRemark; }
            set { _DriverRemark = value; }
        }
        /// <summary>
        /// CustomerRemark
        /// </summary>		
        private string _CustomerRemark;
        public string CustomerRemark
        {
            get { return _CustomerRemark; }
            set { _CustomerRemark = value; }
        }
        /// <summary>
        /// Addtime
        /// </summary>		
        private DateTime? _Addtime;
        public DateTime? Addtime
        {
            get { return _Addtime; }
            set { _Addtime = value; }
        }
    }
}
