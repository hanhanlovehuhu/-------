using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.V2.DbConfig
{
    internal class LogicDb
    {
        internal LogicDb()
        {
            TableList = new Dictionary<string, Table>();
        }

        /// <summary>
        /// 获取默认数据库连接
        /// </summary>
        internal Connection DefaultConnection
        {
            get;
            set;
        }

        internal Dictionary<string, Table> TableList
        {
            get;
            private set;
        }
    }
}
