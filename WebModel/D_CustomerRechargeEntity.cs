using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace WebModel
{
    //D_CustomerRecharge
    public class D_CustomerRechargeEntity 
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
        /// RechargeId
        /// </summary>		
        private string _RechargeId;
        public string RechargeId
        {
            get { return _RechargeId; }
            set { _RechargeId = value; }
        }
        /// <summary>
        /// RechargeType
        /// </summary>		
        private int? _RechargeType;
        public int? RechargeType
        {
            get { return _RechargeType; }
            set { _RechargeType = value; }
        }
        /// <summary>
        /// From
        /// </summary>		
        private int? _From;
        public int? From
        {
            get { return _From; }
            set { _From = value; }
        }
        /// <summary>
        /// TradeNo
        /// </summary>		
        private string _TradeNo;
        public string TradeNo
        {
            get { return _TradeNo; }
            set { _TradeNo = value; }
        }
        /// <summary>
        /// Ucode
        /// </summary>		
        private string _CustomerId;
        public string CustomerId
        {
            get { return _CustomerId; }
            set { _CustomerId = value; }
        }
        /// <summary>
        /// Ucode
        /// </summary>		
        private string _CustomerName;
        public string CustomerName
        {
            get { return _CustomerName; }
            set { _CustomerName = value; }
        }
        /// <summary>
        /// Amount
        /// </summary>		
        private decimal? _Amount;
        public decimal? Amount
        {
            get { return _Amount; }
            set { _Amount = value; }
        }
        /// <summary>
        /// Status
        /// </summary>		
        private int? _Status;
        public int? Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        /// <summary>
        /// ResponseTime
        /// </summary>		
        private DateTime? _ResponseTime;
        public DateTime? ResponseTime
        {
            get { return _ResponseTime; }
            set { _ResponseTime = value; }
        }
        /// <summary>
        /// Message
        /// </summary>		
        private string _Message;
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
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
        private string _orderId;
        /// <summary>
        /// 订单Id
        /// </summary>
        public string OrderId
        {
            get { return _orderId; }
            set { _orderId = value; }
        }

        private int? _subRechargeType;
        /// <summary>
        /// 子类型
        /// </summary>
        public int? SubRechargeType
        {
            get { return _subRechargeType; }
            set { _subRechargeType = value; }
        }
    }
}
