using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aidaijia.API.Entity
{
    public class D_OrderInfoEntity
    {
        public List<D_OrderDetailEntity> OrderDetailList { get; set; }

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
        /// OrderType
        /// </summary>		
        private int? _OrderType;
        public int? OrderType
        {
            get { return _OrderType; }
            set { _OrderType = value; }
        }
        /// <summary>
        /// OrderFrom
        /// </summary>		
        private int? _OrderFrom;
        public int? OrderFrom
        {
            get { return _OrderFrom; }
            set { _OrderFrom = value; }
        }
        /// <summary>
        /// OrderTime
        /// </summary>		
        private DateTime? _OrderTime;
        public DateTime? OrderTime
        {
            get { return _OrderTime; }
            set { _OrderTime = value; }
        }
        /// <summary>
        /// CustomerId
        /// </summary>		
        private string _CustomerId;
        public string CustomerId
        {
            get { return _CustomerId; }
            set { _CustomerId = value; }
        }
        /// <summary>
        /// FromPhone
        /// </summary>		
        private string _FromPhone;
        public string FromPhone
        {
            get { return _FromPhone; }
            set { _FromPhone = value; }
        }
        /// <summary>
        /// CustomerType
        /// </summary>		
        private int? _CustomerType;
        public int? CustomerType
        {
            get { return _CustomerType; }
            set { _CustomerType = value; }
        }
        /// <summary>
        /// Uid
        /// </summary>		
        private int? _Uid;
        public int? Uid
        {
            get { return _Uid; }
            set { _Uid = value; }
        }
        /// <summary>
        /// Ucode
        /// </summary>		
        private string _Ucode;
        public string Ucode
        {
            get { return _Ucode; }
            set { _Ucode = value; }
        }
        /// <summary>
        /// EstimateMoney
        /// </summary>		
        private decimal? _EstimateMoney;
        public decimal? EstimateMoney
        {
            get { return _EstimateMoney; }
            set { _EstimateMoney = value; }
        }
        /// <summary>
        /// SSMoney
        /// </summary>		
        private decimal? _SSMoney;
        public decimal? SSMoney
        {
            get { return _SSMoney; }
            set { _SSMoney = value; }
        }
        /// <summary>
        /// Money
        /// </summary>		
        private decimal? _Money;
        public decimal? Money
        {
            get { return _Money; }
            set { _Money = value; }
        }
        /// <summary>
        /// PMoney
        /// </summary>		
        private decimal? _PMoney;
        public decimal? PMoney
        {
            get { return _PMoney; }
            set { _PMoney = value; }
        }
        /// <summary>
        /// WaitMoney
        /// </summary>		
        private decimal? _WaitMoney;
        public decimal? WaitMoney
        {
            get { return _WaitMoney; }
            set { _WaitMoney = value; }
        }
        /// <summary>
        /// OtherMoney
        /// </summary>		
        private decimal? _OtherMoney;
        public decimal? OtherMoney
        {
            get { return _OtherMoney; }
            set { _OtherMoney = value; }
        }
        /// <summary>
        /// State
        /// </summary>		
        private int? _State;
        public int? State
        {
            get { return _State; }
            set { _State = value; }
        }
        /// <summary>
        /// BeginAddress
        /// </summary>		
        private string _BeginAddress;
        public string BeginAddress
        {
            get { return _BeginAddress; }
            set { _BeginAddress = value; }
        }
        /// <summary>
        /// AppointmentTime
        /// </summary>		
        private DateTime? _AppointmentTime;
        public DateTime? AppointmentTime
        {
            get { return _AppointmentTime; }
            set { _AppointmentTime = value; }
        }
        /// <summary>
        /// IsComment
        /// </summary>		
        private bool? _IsComment;
        public bool? IsComment
        {
            get { return _IsComment; }
            set { _IsComment = value; }
        }
        /// <summary>
        /// IsInvoiced
        /// </summary>		
        private bool? _IsInvoiced;
        public bool? IsInvoiced
        {
            get { return _IsInvoiced; }
            set { _IsInvoiced = value; }
        }
        /// <summary>
        /// IsCallBack
        /// </summary>		
        private bool? _IsCallBack;
        public bool? IsCallBack
        {
            get { return _IsCallBack; }
            set { _IsCallBack = value; }
        }
        /// <summary>
        /// AccumulatedApproveState
        /// </summary>		
        private int? _AccumulatedApproveState;
        public int? AccumulatedApproveState
        {
            get { return _AccumulatedApproveState; }
            set { _AccumulatedApproveState = value; }
        }
        /// <summary>
        /// create_time
        /// </summary>		
        private DateTime? _create_time;
        public DateTime? create_time
        {
            get { return _create_time; }
            set { _create_time = value; }
        }
        /// <summary>
        /// create_user
        /// </summary>		
        private string _create_user;
        public string create_user
        {
            get { return _create_user; }
            set { _create_user = value; }
        }
        /// <summary>
        /// update_time
        /// </summary>		
        private DateTime? _update_time;
        public DateTime? update_time
        {
            get { return _update_time; }
            set { _update_time = value; }
        }
        /// <summary>
        /// update_user
        /// </summary>		
        private string _update_user;
        public string update_user
        {
            get { return _update_user; }
            set { _update_user = value; }
        }
        /// <summary>
        /// delete_flag
        /// </summary>		
        private bool? _delete_flag;
        public bool? delete_flag
        {
            get { return _delete_flag; }
            set { _delete_flag = value; }
        }
        /// <summary>
        /// delete_time
        /// </summary>		
        private DateTime? _delete_time;
        public DateTime? delete_time
        {
            get { return _delete_time; }
            set { _delete_time = value; }
        }
        /// <summary>
        /// delete_user
        /// </summary>		
        private string _delete_user;
        public string delete_user
        {
            get { return _delete_user; }
            set { _delete_user = value; }
        }
    }
}
