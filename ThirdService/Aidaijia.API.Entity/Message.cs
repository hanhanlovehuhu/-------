using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aidaijia.API.Entity
{
    public class Message
    {
        /// <summary>
        /// 1司机 2客户
        /// </summary>
        public int TypeId { set; get; }

        /// <summary>
        /// 接收的手机号
        /// </summary>
        public string ReceivePhone { set; get; }

        /// <summary>
        /// 短信内容
        /// </summary>
        public string MsgContent { set; get; }

        /// <summary>
        /// 短信状态 0未发送 1已发送
        /// </summary>
        public int State { set; get; }

        /// <summary>
        /// 创建人
        /// </summary>
        public int Created { set; get; }

        /// <summary>
        /// 短信来源 1招聘司机平台 2接口webService 3后台 4爱推广短信
        /// </summary>
        public int FromType { set; get; }
    }
}
