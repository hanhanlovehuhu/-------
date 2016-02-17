using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.V2.DbConfig
{
    /// <summary>
    /// 数据表操作动作
    /// </summary>
    [Flags]
    public enum OperationType
    {
        /// <summary>
        /// 读取
        /// </summary>
        Read,
        /// <summary>
        /// 写入
        /// </summary>
        Write
    }
}
