using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.V2.DbConfig
{
    /// <summary>
    /// 为应用程序提供数据库类型选择
    /// </summary>
    public enum DataProvider : short
    {
        SqlServer = 0,
        /// <summary>
        /// Sql Server CE 3.5数据库，用于小型桌面数据库程序及智能设备的开发
        /// </summary>
        SqlServerCE = 1,
        /// <summary>
        /// OleDb类型数据库，包括Excel、Access等所有支持OleDb连接的数据库
        /// </summary>
        OleDb = 2,
        /// <summary>
        /// ODBC驱动程序连接的数据库
        /// </summary>
        Odbc = 3,
        /// <summary>
        /// MySql数据库
        /// </summary>
        MySql = 4,
        /// <summary>
        /// Oracle数据库
        /// </summary>
        //Oracle = 5,
        /// <summary>
        /// PostgreSQL数据库
        /// </summary>
        PostgreSQL = 6,
        /// <summary>
        /// 小型SQLite数据库。用于嵌入式设备、小型桌面应用程序的开发
        /// </summary>
        //SQLite = 6,
        /// <summary>
        /// 流行的Firebird小型数据库，用于嵌入式、小型桌面应用程序的开发
        /// </summary>
        //Firebird = 7,
        /// <summary>
        /// 流行的小型数据库，用于嵌入式、小型桌面应用程序的开发
        /// </summary>
        //BerkeleyDb = 8,
    }
}
