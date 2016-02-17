using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace WebUtility
{
    public class ConfigHelper
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static readonly string SqlServerConnStr = ConfigurationManager.ConnectionStrings["SQLConnStr"].ConnectionString;
        /// <summary>
        /// 页面大小
        /// </summary>
        public static readonly int PageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"]);

        /// <summary>
        /// 发送邮箱
        /// </summary>
        public static readonly string ServiceEmailName = ConfigurationManager.AppSettings["ServiceEmailName"];
        /// <summary>
        /// 发送名称
        /// </summary>
        public static readonly string ServiceName = ConfigurationManager.AppSettings["ServiceName"];
        /// <summary>
        /// 发送用户名
        /// </summary>
        public static readonly string EmailName = ConfigurationManager.AppSettings["EmailName"];
        /// <summary>
        /// 发送密码
        /// </summary>
        public static readonly string EmailPwd = ConfigurationManager.AppSettings["EmailPwd"];
        /// <summary>
        /// smtp
        /// </summary>
        public static readonly string ServiceEmailSmtp = ConfigurationManager.AppSettings["ServiceEmailSmtp"];
        /// <summary>
        /// 图片地址
        /// </summary>
        public static readonly string ImagePath = ConfigurationManager.AppSettings["ImagePath"];
        /// <summary>
        /// 主域名
        /// </summary>
        public static readonly string Domain = ConfigurationManager.AppSettings["Domain"];
        /// <summary>
        /// BBS域名
        /// </summary>
        public static readonly string BBS = ConfigurationManager.AppSettings["BBS"];
        /// <summary>
        /// Mall域名
        /// </summary>
        public static readonly string Mall = ConfigurationManager.AppSettings["Mall"];
        /// <summary>
        /// 支付宝合作商
        /// </summary>
        public static readonly string ZFBHZ = ConfigurationManager.AppSettings["ZFBHZ"];
        /// <summary>
        /// 收款帐号
        /// </summary>
        public static readonly string GetmoneyEmail = ConfigurationManager.AppSettings["GetmoneyEmail"];
        /// <summary>
        /// 支持宝安全验证码
        /// </summary>
        public static readonly string ZFBKEY = ConfigurationManager.AppSettings["ZFBKEY"];
        /// <summary>
        /// 短信密码
        /// </summary>
        public static readonly string Dxpwd = ConfigurationManager.AppSettings["Dxpwd"];
        /// <summary>
        /// 短信用户名
        /// </summary>
        public static readonly string Dxuname = ConfigurationManager.AppSettings["Dxuname"];
        /// <summary>
        /// 订单创建服务
        /// </summary>
        public static readonly string AdjOrderService = ConfigurationManager.AppSettings["AdjOrderService"];
        /// <summary>
        /// 创建订单Redis 接口
        /// </summary>
        public static readonly string APIServer = ConfigurationManager.AppSettings["apiServer"];
        /// <summary>
        /// 合作商标识是否可用  1：生产环境下 2：测试环境下 3：禁用
        /// </summary>
        public static readonly int Sign =Convert.ToInt32(ConfigurationManager.AppSettings["Sign"]);

        /// <summary>
        /// 关闭的第三方发券  a|b|c
        /// </summary>
        public static readonly string CloseSign = ConfigurationManager.AppSettings["CloseSign"];
    }
}
