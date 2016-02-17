using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.V2.DbConfig
{
    /// <summary>
    /// 模块
    /// </summary>
    internal class Module
    {
        internal Module()
        {
            LogicDbList = new Dictionary<string, LogicDb>();
            ConnectionList = new Dictionary<string, Connection>();
        }

        /// <summary>
        /// 获取模块的逻辑数据库
        /// </summary>
        internal Dictionary<string, LogicDb> LogicDbList
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取模块中的数据库连接列表
        /// </summary>
        internal Dictionary<string, Connection> ConnectionList
        {
            get;
            private set;
        }
    }
}
