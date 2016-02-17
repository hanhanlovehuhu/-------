using System;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using System.Data.OleDb;
using System.Data.Odbc;
//using System.Data.SqlServerCe;
//using System.Data.SQLite;
//using FirebirdSql.Data.FirebirdClient;
using System.Text;
using DataAccess.V2.DbConfig;
//using Npgsql;
//using NpgsqlTypes;
//using Oracle.DataAccess.Client;
//using MySql.Data.MySqlClient;


namespace DataAccess.V2
{
    /// <summary>
    /// 数据操作对象
    /// </summary>
    public class DataOperator : IDisposable
    {
        #region 属性
        /// <summary>
        /// 获取或设置SQL命令行
        /// </summary>
        public string CommandText
        {
            get
            {
                return _DbCommand.CommandText;
            }
            set
            {
                _DbCommand.CommandText = value;
            }
        }

        /// <summary>
        /// 获取或设置查询语句的类型
        /// </summary>
        public CommandType CommandType
        {
            get
            {
                return _DbCommand.CommandType;
            }
            set
            {
                _DbCommand.CommandType = value;
            }
        }

        /// <summary>
        /// 获取或设置查询的超时时间
        /// </summary>
        public int CommandTimeout
        {
            get
            {
                return _DbCommand.CommandTimeout;
            }
            set
            {
                _DbCommand.CommandTimeout = value;
            }
        }

        /// <summary>
        /// 获取当前操作类的数据库类型
        /// </summary>
        public DataProvider DataProvider
        {
            get
            {
                return _Connection.Provider;
            }
        }

        /// <summary>
        /// 获取或设置此连接所对应的逻辑数据库
        /// </summary>
        private string LogicDbName
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置此连接所对应的表名
        /// </summary>
        private string TableName
        {
            get;
            set;
        }

        //		List<DbConnection> _DbConnectionList;
        Connection _Connection;
        private DbCommand _DbCommand = null;
        private DbConnection _DbConnection = null;
        private DbTransaction _DbTransaction = null;
        #endregion

        #region 构造函数
        private DataOperator()
        {
        }

        /// <summary>
        /// 创建并初始化一个数据操作对象
        /// </summary>
        /// <param name="moduleName">当前模块名</param>
        /// <param name="logicDb">要操作的逻辑数据库名</param>
        /// <param name="tableName">要操作的表名</param>
        /// <param name="action">操作动作</param>
        public DataOperator(string moduleName, string logicDb, string tableName, OperationType action)
        {
            LogicDbName = logicDb;
            TableName = tableName;
            // 根据logicDbName、tableName、action获取数据库连接对象
            ConfigReader dbConfigReader = ConfigReader.Create();
            _Connection = dbConfigReader.GetConnectionString(moduleName, logicDb, tableName, action);

            switch (_Connection.Provider)
            {
                case DataProvider.SqlServer:
                default:
                    _DbConnection = new SqlConnection(_Connection.ConnectionString);
                    _DbCommand = new SqlCommand();
                    break;
                //case DataProvider.SqlServerCE:
                //    _DbConnection = new SqlCeConnection(_Connection.ConnectionString);
                //    _DbCommand = new SqlCeCommand();
                //    break;
                case DataProvider.OleDb:
                    _DbConnection = new OleDbConnection(_Connection.ConnectionString);
                    _DbCommand = new OleDbCommand();
                    break;
                case DataProvider.Odbc:
                    _DbConnection = new OdbcConnection(_Connection.ConnectionString);
                    _DbCommand = new OdbcCommand();
                    break;
                case DataProvider.MySql:
                    _DbConnection = new MySql.Data.MySqlClient.MySqlConnection(_Connection.ConnectionString);
                    _DbCommand = new MySql.Data.MySqlClient.MySqlCommand();
                    break;
                case DataProvider.PostgreSQL:
                    _DbConnection = new Npgsql.NpgsqlConnection(_Connection.ConnectionString);
                    _DbCommand = new Npgsql.NpgsqlCommand();
                    break;
                //case DataProvider.Oracle:
                //    dbConnection = new OracleConnection(_Connection.ConnectionString);
                //    _DBCommand = new OracleCommand();
                //    break;
                //case DataProvider.SQLite:
                //    dbConnection = new SQLiteConnection(_Connection.ConnectionString);
                //    _DBCommand = new SQLiteCommand();
                //    break;
                //case DataProvider.Firebird:
                //    dbConnection = new FbConnection(_Connection.ConnectionString);
                //    _DBCommand = new FbCommand();
                //    break;
                //case DataProvider.BerkeleyDb:
                //    dbConnection = new BerkeleyDb(_Connection.ConnectionString);
                //    _DBCommand = new BerkeleyDb();
                //    break;
            }
            _DbConnection.Open();
            _DbCommand.Connection = _DbConnection;
        }

        private bool disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // 如果还存在未提交或回滚的事物，则将事务回滚
                    if (_DbTransaction != null)
                    {
                        try
                        {
                            _DbTransaction.Rollback();
                            _DbTransaction = null;
                        }
                        catch { }
                    }

                    if (_DbCommand != null)
                    {
                        _DbCommand.Dispose();
                    }

                    if (_DbConnection.State == ConnectionState.Open)
                    {
                        try
                        {
                            _DbConnection.Close();
                        }
                        catch
                        {
                        }
                    }
                    if (_DbConnection != null)
                    {
                        _DbConnection.Dispose();
                    }
                }
                disposed = true;
            }
        }
        #endregion

        #region SQL Server和Sql Server CE参数操作
        /// <summary>
        /// 设置SQL参数的值，当参数类型为int、bit、bigint、smallint、datatime、smalldatetime、money、float、real、text、ntext、uniqueidentity、image、smallmoney、tinyint等不需要设置长度的类型时调用。
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="paraData">要设置的参数值</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="direction">参数方向</param>
        public void SetParameter(string paraName, object paraData, SqlDbType dbType, ParameterDirection direction)
        {
            if (DataProvider == DataProvider.SqlServer)
            {
                SqlParameter param;
                SqlCommand command = (SqlCommand)_DbCommand;
                bool blnNewParam = false;

                if (command.Parameters.IndexOf(paraName) < 0)
                {
                    blnNewParam = true;
                    param = command.CreateParameter();
                    param.ParameterName = paraName;
                }
                else
                {
                    param = command.Parameters[paraName];
                }

                param.SqlDbType = dbType;
                param.Direction = direction;
                // 如果值为空，则将值设置为DBNull.Value，即插入数据库后仍然保持null;
                if (paraData == null)
                    param.Value = System.DBNull.Value;
                else
                    param.Value = paraData;

                // 如果是新参数，添加参数到Command中
                if (blnNewParam)
                    command.Parameters.Add(param);
            }
            //else if (DataProvider == DataProvider.SqlServerCE)
            //{
            //    SqlCeParameter param;
            //    SqlCeCommand command = (SqlCeCommand)_DbCommand;
            //    bool blnNewParam = false;

            //    if (command.Parameters.IndexOf(paraName) < 0)
            //    {
            //        blnNewParam = true;
            //        param = command.CreateParameter();
            //        param.ParameterName = paraName;
            //    }
            //    else
            //    {
            //        param = command.Parameters[paraName];
            //    }

            //    param.SqlDbType = dbType;
            //    param.Direction = direction;
            //    // 如果值为空，则将值设置为DBNull.Value，即插入数据库后仍然保持null;
            //    if (paraData == null)
            //        param.Value = System.DBNull.Value;
            //    else
            //        param.Value = paraData;

            //    // 如果是新参数，添加参数到Command中
            //    if (blnNewParam)
            //        command.Parameters.Add(param);
            //}
            else
            {
                throw new Exceptions.DbException("错误的参数类型，请调用相应数据库对应的参数设置方法！");
            }
        }

        /// <summary>
        /// 设置SQL参数的值，当参数类型为int、bit、bigint、smallint、datatime、smalldatetime、money、float、real、text、ntext、uniqueidentity、image、smallmoney、tinyint等不需要设置长度的类型时调用。
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="paraData">要设置的参数值</param>
        /// <param name="dbType">数据类型</param>
        public void SetParameter(string paraName, object paraData, SqlDbType dbType)
        {
            SetParameter(paraName, paraData, dbType, ParameterDirection.Input);
        }

        /// <summary>
        /// 设置SQL参数的值，当参数类型为varchar、nvarchar、char、nchar、binary、varbinary类型时，指定参数的长度。
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="paraData">要设置的参数值</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="size">字符串长度</param>
        /// <param name="direction">参数方向</param>
        public void SetParameter(string paraName, object paraData, SqlDbType dbType, int size, ParameterDirection direction)
        {
            if (DataProvider == DataProvider.SqlServer)
            {
                SqlParameter param;
                SqlCommand command = (SqlCommand)_DbCommand;
                bool blnNewParam = false;

                if (command.Parameters.IndexOf(paraName) < 0)
                {
                    blnNewParam = true;
                    param = command.CreateParameter();
                    param.ParameterName = paraName;
                }
                else
                {
                    param = command.Parameters[paraName];
                }

                param.SqlDbType = dbType;
                param.Direction = direction;
                // 如果值为空，则将值设置为DBNull.Value，即插入数据库后仍然保持null;
                if (paraData == null)
                    param.Value = System.DBNull.Value;
                else
                    param.Value = paraData;

                param.Size = size;

                // 如果是新参数，添加参数到Command中
                if (blnNewParam)
                    command.Parameters.Add(param);
            }
            //else if (DataProvider == DataProvider.SqlServerCE)
            //{
            //    SqlCeParameter param;
            //    SqlCeCommand command = (SqlCeCommand)_DbCommand;
            //    bool blnNewParam = false;

            //    if (command.Parameters.IndexOf(paraName) < 0)
            //    {
            //        blnNewParam = true;
            //        param = command.CreateParameter();
            //        param.ParameterName = paraName;
            //    }
            //    else
            //    {
            //        param = command.Parameters[paraName];
            //    }

            //    param.SqlDbType = dbType;
            //    param.Direction = direction;
            //    // 如果值为空，则将值设置为DBNull.Value，即插入数据库后仍然保持null;
            //    if (paraData == null)
            //        param.Value = System.DBNull.Value;
            //    else
            //        param.Value = paraData;

            //    param.Size = size;

            //    // 如果是新参数，添加参数到Command中
            //    if (blnNewParam)
            //        command.Parameters.Add(param);
            //}
            else
            {
                throw new Exceptions.DbException("错误的参数类型，请调用相应数据库对应的参数设置方法！");
            }
        }

        /// <summary>
        /// 设置SQL参数的值，当参数类型为varchar、nvarchar、char、nchar、binary、varbinary类型时，指定参数的长度。
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="paraData">要设置的参数值</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="size">字符串长度</param>
        public void SetParameter(string paraName, object paraData, SqlDbType dbType, int size)
        {
            SetParameter(paraName, paraData, dbType, size, ParameterDirection.Input);
        }

        /// <summary>
        /// 设置SQL参数的值。数据类型为decimal、numeric时调用此方法
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="paraData">要设置的参数值</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="precision">参数的最大位数。 </param>
        /// <param name="scale">参数的小数位数</param>
        /// <param name="direction">参数方向</param>
        public void SetParameter(string paraName, object paraData, SqlDbType dbType, byte precision, byte scale, ParameterDirection direction)
        {
            if (DataProvider == DataProvider.SqlServer)
            {
                SqlParameter param;
                SqlCommand command = (SqlCommand)_DbCommand;
                bool blnNewParam = false;

                if (command.Parameters.IndexOf(paraName) < 0)
                {
                    blnNewParam = true;
                    param = command.CreateParameter();
                    param.ParameterName = paraName;
                }
                else
                {
                    param = command.Parameters[paraName];
                }

                param.SqlDbType = dbType;
                param.Direction = direction;
                // 如果值为空，则将值设置为DBNull.Value，即插入数据库后仍然保持null;
                if (paraData == null)
                    param.Value = System.DBNull.Value;
                else
                    param.Value = paraData;

                param.Precision = precision;
                param.Scale = scale;

                // 如果是新参数，添加参数到Command中
                if (blnNewParam)
                    command.Parameters.Add(param);
            }
            //else if (DataProvider == DataProvider.SqlServerCE)
            //{
            //    SqlCeParameter param;
            //    SqlCeCommand command = (SqlCeCommand)_DbCommand;
            //    bool blnNewParam = false;

            //    if (command.Parameters.IndexOf(paraName) < 0)
            //    {
            //        blnNewParam = true;
            //        param = command.CreateParameter();
            //        param.ParameterName = paraName;
            //    }
            //    else
            //    {
            //        param = command.Parameters[paraName];
            //    }

            //    param.SqlDbType = dbType;
            //    param.Direction = direction;
            //    // 如果值为空，则将值设置为DBNull.Value，即插入数据库后仍然保持null;
            //    if (paraData == null)
            //        param.Value = System.DBNull.Value;
            //    else
            //        param.Value = paraData;

            //    param.Precision = precision;
            //    param.Scale = scale;

            //    // 如果是新参数，添加参数到Command中
            //    if (blnNewParam)
            //        command.Parameters.Add(param);
            //}
            else
            {
                throw new Exceptions.DbException("错误的参数类型，请调用相应数据库对应的参数设置方法！");
            }
        }

        /// <summary>
        /// 设置SQL参数的值。数据类型为decimal、numeric时调用此方法
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="paraData">要设置的参数值</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="precision">参数的最大位数。 </param>
        /// <param name="scale">参数的小数位数</param>
        public void SetParameter(string paraName, object paraData, SqlDbType dbType, byte precision, byte scale)
        {
            SetParameter(paraName, paraData, dbType, precision, scale, ParameterDirection.Input);
        }
        #endregion

        #region MySql参数操作
        /// <summary>
        /// 设置SQL参数的值，当参数类型为int、bit、bigint、smallint、datatime、smalldatetime、money、float、real、text、ntext、uniqueidentity、image、smallmoney、tinyint等不需要设置长度的类型时调用。
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="paraData">要设置的参数值</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="direction">参数方向</param>
        public void SetParameter(string paraName, object paraData, MySql.Data.MySqlClient.MySqlDbType dbType, ParameterDirection direction)
        {
            if (DataProvider == DataProvider.MySql)
            {
                MySql.Data.MySqlClient.MySqlParameter param;
                MySql.Data.MySqlClient.MySqlCommand command = (MySql.Data.MySqlClient.MySqlCommand)_DbCommand;
                bool blnNewParam = false;

                if (command.Parameters.IndexOf(paraName) < 0)
                {
                    blnNewParam = true;
                    param = command.CreateParameter();
                    param.ParameterName = paraName;
                }
                else
                {
                    param = command.Parameters[paraName];
                }

                param.MySqlDbType = dbType;
                param.Direction = direction;
                // 如果值为空，则将值设置为DBNull.Value，即插入数据库后仍然保持null;
                if (paraData == null)
                    param.Value = System.DBNull.Value;
                else
                    param.Value = paraData;

                // 如果是新参数，添加参数到Command中
                if (blnNewParam)
                    command.Parameters.Add(param);
            }
            else
            {
                throw new Exceptions.DbException("错误的参数类型，请调用相应数据库对应的参数设置方法！");
            }
        }

        /// <summary>
        /// 设置SQL参数的值，当参数类型为int、bit、bigint、smallint、datatime、smalldatetime、money、float、real、text、ntext、uniqueidentity、image、smallmoney、tinyint等不需要设置长度的类型时调用。
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="paraData">要设置的参数值</param>
        /// <param name="dbType">数据类型</param>
        public void SetParameter(string paraName, object paraData, MySql.Data.MySqlClient.MySqlDbType dbType)
        {
            SetParameter(paraName, paraData, dbType, ParameterDirection.Input);
        }

        /// <summary>
        /// 设置SQL参数的值，当参数类型为varchar、nvarchar、char、nchar、binary、varbinary类型时，指定参数的长度。
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="paraData">要设置的参数值</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="size">字符串长度</param>
        /// <param name="direction">参数方向</param>
        public void SetParameter(string paraName, object paraData, MySql.Data.MySqlClient.MySqlDbType dbType, int size, ParameterDirection direction)
        {
            if (DataProvider == DataProvider.MySql)
            {
                MySql.Data.MySqlClient.MySqlParameter param;
                MySql.Data.MySqlClient.MySqlCommand command = (MySql.Data.MySqlClient.MySqlCommand)_DbCommand;
                bool blnNewParam = false;

                if (command.Parameters.IndexOf(paraName) < 0)
                {
                    blnNewParam = true;
                    param = command.CreateParameter();
                    param.ParameterName = paraName;
                }
                else
                {
                    param = command.Parameters[paraName];
                }

                param.MySqlDbType = dbType;
                param.Direction = direction;
                // 如果值为空，则将值设置为DBNull.Value，即插入数据库后仍然保持null;
                if (paraData == null)
                    param.Value = System.DBNull.Value;
                else
                    param.Value = paraData;

                param.Size = size;

                // 如果是新参数，添加参数到Command中
                if (blnNewParam)
                    command.Parameters.Add(param);
            }
            else
            {
                throw new Exceptions.DbException("错误的参数类型，请调用相应数据库对应的参数设置方法！");
            }
        }

        /// <summary>
        /// 设置SQL参数的值，当参数类型为varchar、nvarchar、char、nchar、binary、varbinary类型时，指定参数的长度。
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="paraData">要设置的参数值</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="size">字符串长度</param>
        public void SetParameter(string paraName, object paraData, MySql.Data.MySqlClient.MySqlDbType dbType, int size)
        {
            SetParameter(paraName, paraData, dbType, size, ParameterDirection.Input);
        }

        /// <summary>
        /// 设置SQL参数的值。数据类型为decimal、numeric时调用此方法
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="paraData">要设置的参数值</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="precision">参数的最大位数。 </param>
        /// <param name="scale">参数的小数位数</param>
        /// <param name="direction">参数方向</param>
        public void SetParameter(string paraName, object paraData, MySql.Data.MySqlClient.MySqlDbType dbType, byte precision, byte scale, ParameterDirection direction)
        {
            if (DataProvider == DataProvider.MySql)
            {
                MySql.Data.MySqlClient.MySqlParameter param;
                MySql.Data.MySqlClient.MySqlCommand command = (MySql.Data.MySqlClient.MySqlCommand)_DbCommand;
                bool blnNewParam = false;

                if (command.Parameters.IndexOf(paraName) < 0)
                {
                    blnNewParam = true;
                    param = command.CreateParameter();
                    param.ParameterName = paraName;
                }
                else
                {
                    param = command.Parameters[paraName];
                }

                param.MySqlDbType = dbType;
                param.Direction = direction;
                // 如果值为空，则将值设置为DBNull.Value，即插入数据库后仍然保持null;
                if (paraData == null)
                    param.Value = System.DBNull.Value;
                else
                    param.Value = paraData;

                param.Precision = precision;
                param.Scale = scale;

                // 如果是新参数，添加参数到Command中
                if (blnNewParam)
                    command.Parameters.Add(param);
            }
            else
            {
                throw new Exceptions.DbException("错误的参数类型，请调用相应数据库对应的参数设置方法！");
            }
        }

        /// <summary>
        /// 设置SQL参数的值。数据类型为decimal、numeric时调用此方法
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="paraData">要设置的参数值</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="precision">参数的最大位数。 </param>
        /// <param name="scale">参数的小数位数</param>
        public void SetParameter(string paraName, object paraData, MySql.Data.MySqlClient.MySqlDbType dbType, byte precision, byte scale)
        {
            SetParameter(paraName, paraData, dbType, precision, scale, ParameterDirection.Input);
        }
        #endregion

        #region OleDb参数操作
        /// <summary>
        /// 设置SQL参数的值，当参数类型为int、bit、bigint、smallint、datatime、smalldatetime、money、float、real、text、ntext、uniqueidentity、image、smallmoney、tinyint等不需要设置长度的类型时调用。
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="paraData">要设置的参数值</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="direction">参数方向</param>
        public void SetParameter(string paraName, object paraData, OleDbType dbType, ParameterDirection direction)
        {
            if (DataProvider == DataProvider.OleDb)
            {
                OleDbParameter param;
                OleDbCommand command = (OleDbCommand)_DbCommand;
                bool blnNewParam = false;

                if (command.Parameters.IndexOf(paraName) < 0)
                {
                    blnNewParam = true;
                    param = command.CreateParameter();
                    param.ParameterName = paraName;
                }
                else
                {
                    param = command.Parameters[paraName];
                }

                param.OleDbType = dbType;
                param.Direction = direction;
                // 如果值为空，则将值设置为DBNull.Value，即插入数据库后仍然保持null;
                if (paraData == null)
                    param.Value = System.DBNull.Value;
                else
                    param.Value = paraData;

                // 如果是新参数，添加参数到Command中
                if (blnNewParam)
                    command.Parameters.Add(param);
            }
            else
            {
                throw new Exceptions.DbException("错误的参数类型，请调用相应数据库对应的参数设置方法！");
            }
        }

        /// <summary>
        /// 设置SQL参数的值，当参数类型为int、bit、bigint、smallint、datatime、smalldatetime、money、float、real、text、ntext、uniqueidentity、image、smallmoney、tinyint等不需要设置长度的类型时调用。
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="paraData">要设置的参数值</param>
        /// <param name="dbType">数据类型</param>
        public void SetParameter(string paraName, object paraData, OleDbType dbType)
        {
            SetParameter(paraName, paraData, dbType, ParameterDirection.Input);
        }

        /// <summary>
        /// 设置SQL参数的值，当参数类型为varchar、nvarchar、char、nchar、binary、varbinary类型时，指定参数的长度。
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="paraData">要设置的参数值</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="size">字符串长度</param>
        /// <param name="direction">参数方向</param>
        public void SetParameter(string paraName, object paraData, OleDbType dbType, int size, ParameterDirection direction)
        {
            if (DataProvider == DataProvider.OleDb)
            {
                OleDbParameter param;
                OleDbCommand command = (OleDbCommand)_DbCommand;
                bool blnNewParam = false;

                if (command.Parameters.IndexOf(paraName) < 0)
                {
                    blnNewParam = true;
                    param = command.CreateParameter();
                    param.ParameterName = paraName;
                }
                else
                {
                    param = command.Parameters[paraName];
                }

                param.OleDbType = dbType;
                param.Direction = direction;
                // 如果值为空，则将值设置为DBNull.Value，即插入数据库后仍然保持null;
                if (paraData == null)
                    param.Value = System.DBNull.Value;
                else
                    param.Value = paraData;

                param.Size = size;

                // 如果是新参数，添加参数到Command中
                if (blnNewParam)
                    command.Parameters.Add(param);
            }
            else
            {
                throw new Exceptions.DbException("错误的参数类型，请调用相应数据库对应的参数设置方法！");
            }
        }

        /// <summary>
        /// 设置SQL参数的值，当参数类型为varchar、nvarchar、char、nchar、binary、varbinary类型时，指定参数的长度。
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="paraData">要设置的参数值</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="size">字符串长度</param>
        public void SetParameter(string paraName, object paraData, OleDbType dbType, int size)
        {
            SetParameter(paraName, paraData, dbType, size, ParameterDirection.Input);
        }

        /// <summary>
        /// 设置SQL参数的值。数据类型为decimal、numeric时调用此方法
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="paraData">要设置的参数值</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="precision">参数的最大位数。 </param>
        /// <param name="scale">参数的小数位数</param>
        /// <param name="direction">参数方向</param>
        public void SetParameter(string paraName, object paraData, OleDbType dbType, byte precision, byte scale, ParameterDirection direction)
        {
            if (DataProvider == DataProvider.OleDb)
            {
                OleDbParameter param;
                OleDbCommand command = (OleDbCommand)_DbCommand;
                bool blnNewParam = false;

                if (command.Parameters.IndexOf(paraName) < 0)
                {
                    blnNewParam = true;
                    param = command.CreateParameter();
                    param.ParameterName = paraName;
                }
                else
                {
                    param = command.Parameters[paraName];
                }

                param.OleDbType = dbType;
                param.Direction = direction;
                // 如果值为空，则将值设置为DBNull.Value，即插入数据库后仍然保持null;
                if (paraData == null)
                    param.Value = System.DBNull.Value;
                else
                    param.Value = paraData;

                param.Precision = precision;
                param.Scale = scale;

                // 如果是新参数，添加参数到Command中
                if (blnNewParam)
                    command.Parameters.Add(param);
            }
            else
            {
                throw new Exceptions.DbException("错误的参数类型，请调用相应数据库对应的参数设置方法！");
            }
        }

        /// <summary>
        /// 设置SQL参数的值。数据类型为decimal、numeric时调用此方法
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="paraData">要设置的参数值</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="precision">参数的最大位数。 </param>
        /// <param name="scale">参数的小数位数</param>
        public void SetParameter(string paraName, object paraData, OleDbType dbType, byte precision, byte scale)
        {
            SetParameter(paraName, paraData, dbType, precision, scale, ParameterDirection.Input);
        }
        #endregion

        #region ODBC参数操作
        /// <summary>
        /// 设置SQL参数的值，当参数类型为int、bit、bigint、smallint、datatime、smalldatetime、money、float、real、text、ntext、uniqueidentity、image、smallmoney、tinyint等不需要设置长度的类型时调用。
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="paraData">要设置的参数值</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="direction">参数方向</param>
        public void SetParameter(string paraName, object paraData, OdbcType dbType, ParameterDirection direction)
        {
            if (DataProvider == DataProvider.Odbc)
            {
                OdbcParameter param;
                OdbcCommand command = (OdbcCommand)_DbCommand;
                bool blnNewParam = false;

                if (command.Parameters.IndexOf(paraName) < 0)
                {
                    blnNewParam = true;
                    param = command.CreateParameter();
                    param.ParameterName = paraName;
                }
                else
                {
                    param = command.Parameters[paraName];
                }

                param.OdbcType = dbType;
                param.Direction = direction;
                // 如果值为空，则将值设置为DBNull.Value，即插入数据库后仍然保持null;
                if (paraData == null)
                    param.Value = System.DBNull.Value;
                else
                    param.Value = paraData;

                // 如果是新参数，添加参数到Command中
                if (blnNewParam)
                    command.Parameters.Add(param);
            }
            else
            {
                throw new Exceptions.DbException("错误的参数类型，请调用相应数据库对应的参数设置方法！");
            }
        }

        /// <summary>
        /// 设置SQL参数的值，当参数类型为int、bit、bigint、smallint、datatime、smalldatetime、money、float、real、text、ntext、uniqueidentity、image、smallmoney、tinyint等不需要设置长度的类型时调用。
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="paraData">要设置的参数值</param>
        /// <param name="dbType">数据类型</param>
        public void SetParameter(string paraName, object paraData, OdbcType dbType)
        {
            SetParameter(paraName, paraData, dbType, ParameterDirection.Input);
        }

        /// <summary>
        /// 设置SQL参数的值，当参数类型为varchar、nvarchar、char、nchar、binary、varbinary类型时，指定参数的长度。
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="paraData">要设置的参数值</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="size">字符串长度</param>
        /// <param name="direction">参数方向</param>
        public void SetParameter(string paraName, object paraData, OdbcType dbType, int size, ParameterDirection direction)
        {
            if (DataProvider == DataProvider.Odbc)
            {
                OdbcParameter param;
                OdbcCommand command = (OdbcCommand)_DbCommand;
                bool blnNewParam = false;

                if (command.Parameters.IndexOf(paraName) < 0)
                {
                    blnNewParam = true;
                    param = command.CreateParameter();
                    param.ParameterName = paraName;
                }
                else
                {
                    param = command.Parameters[paraName];
                }

                param.OdbcType = dbType;
                param.Direction = direction;
                // 如果值为空，则将值设置为DBNull.Value，即插入数据库后仍然保持null;
                if (paraData == null)
                    param.Value = System.DBNull.Value;
                else
                    param.Value = paraData;

                param.Size = size;

                // 如果是新参数，添加参数到Command中
                if (blnNewParam)
                    command.Parameters.Add(param);
            }
            else
            {
                throw new Exceptions.DbException("错误的参数类型，请调用相应数据库对应的参数设置方法！");
            }
        }

        /// <summary>
        /// 设置SQL参数的值，当参数类型为varchar、nvarchar、char、nchar、binary、varbinary类型时，指定参数的长度。
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="paraData">要设置的参数值</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="size">字符串长度</param>
        public void SetParameter(string paraName, object paraData, OdbcType dbType, int size)
        {
            SetParameter(paraName, paraData, dbType, size, ParameterDirection.Input);
        }

        /// <summary>
        /// 设置SQL参数的值。数据类型为decimal、numeric时调用此方法
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="paraData">要设置的参数值</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="precision">参数的最大位数。 </param>
        /// <param name="scale">参数的小数位数</param>
        /// <param name="direction">参数方向</param>
        public void SetParameter(string paraName, object paraData, OdbcType dbType, byte precision, byte scale, ParameterDirection direction)
        {
            if (DataProvider == DataProvider.Odbc)
            {
                OdbcParameter param;
                OdbcCommand command = (OdbcCommand)_DbCommand;
                bool blnNewParam = false;

                if (command.Parameters.IndexOf(paraName) < 0)
                {
                    blnNewParam = true;
                    param = command.CreateParameter();
                    param.ParameterName = paraName;
                }
                else
                {
                    param = command.Parameters[paraName];
                }

                param.OdbcType = dbType;
                param.Direction = direction;
                // 如果值为空，则将值设置为DBNull.Value，即插入数据库后仍然保持null;
                if (paraData == null)
                    param.Value = System.DBNull.Value;
                else
                    param.Value = paraData;

                param.Precision = precision;
                param.Scale = scale;

                // 如果是新参数，添加参数到Command中
                if (blnNewParam)
                    command.Parameters.Add(param);
            }
            else
            {
                throw new Exceptions.DbException("错误的参数类型，请调用相应数据库对应的参数设置方法！");
            }
        }

        /// <summary>
        /// 设置SQL参数的值。数据类型为decimal、numeric时调用此方法
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="paraData">要设置的参数值</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="precision">参数的最大位数。 </param>
        /// <param name="scale">参数的小数位数</param>
        public void SetParameter(string paraName, object paraData, OdbcType dbType, byte precision, byte scale)
        {
            SetParameter(paraName, paraData, dbType, precision, scale, ParameterDirection.Input);
        }
        #endregion

        #region PostgreSql参数操作
        /// <summary>
        /// 设置SQL参数的值，当参数类型为int、bit、bigint、smallint、datatime、smalldatetime、money、float、real、text、ntext、uniqueidentity、image、smallmoney、tinyint等不需要设置长度的类型时调用。
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="paraData">要设置的参数值</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="direction">参数方向</param>
        public void SetParameter(string paraName, object paraData, NpgsqlTypes.NpgsqlDbType dbType, ParameterDirection direction)
        {
            if (DataProvider == DataProvider.PostgreSQL)
            {
                Npgsql.NpgsqlParameter param;
                Npgsql.NpgsqlCommand command = (Npgsql.NpgsqlCommand)_DbCommand;
                bool blnNewParam = false;

                if (command.Parameters.IndexOf(paraName) < 0)
                {
                    blnNewParam = true;
                    param = command.CreateParameter();
                    param.ParameterName = paraName;
                }
                else
                {
                    param = command.Parameters[paraName];
                }

                param.NpgsqlDbType = dbType;
                param.Direction = direction;
                // 如果值为空，则将值设置为DBNull.Value，即插入数据库后仍然保持null;
                if (paraData == null)
                    param.Value = System.DBNull.Value;
                else
                    param.Value = paraData;

                // 如果是新参数，添加参数到Command中
                if (blnNewParam)
                    command.Parameters.Add(param);
            }
            else
            {
                throw new Exceptions.DbException("错误的参数类型，请调用相应数据库对应的参数设置方法！");
            }
        }

        /// <summary>
        /// 设置SQL参数的值，当参数类型为int、bit、bigint、smallint、datatime、smalldatetime、money、float、real、text、ntext、uniqueidentity、image、smallmoney、tinyint等不需要设置长度的类型时调用。
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="paraData">要设置的参数值</param>
        /// <param name="dbType">数据类型</param>
        public void SetParameter(string paraName, object paraData, NpgsqlTypes.NpgsqlDbType dbType)
        {
            SetParameter(paraName, paraData, dbType, ParameterDirection.Input);
        }

        /// <summary>
        /// 设置SQL参数的值，当参数类型为varchar、nvarchar、char、nchar、binary、varbinary类型时，指定参数的长度。
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="paraData">要设置的参数值</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="size">字符串长度</param>
        /// <param name="direction">参数方向</param>
        public void SetParameter(string paraName, object paraData, NpgsqlTypes.NpgsqlDbType dbType, int size, ParameterDirection direction)
        {
            if (DataProvider == DataProvider.PostgreSQL)
            {
                Npgsql.NpgsqlParameter param;
                Npgsql.NpgsqlCommand command = (Npgsql.NpgsqlCommand)_DbCommand;
                bool blnNewParam = false;

                if (command.Parameters.IndexOf(paraName) < 0)
                {
                    blnNewParam = true;
                    param = command.CreateParameter();
                    param.ParameterName = paraName;
                }
                else
                {
                    param = command.Parameters[paraName];
                }

                param.NpgsqlDbType = dbType;
                param.Direction = direction;
                // 如果值为空，则将值设置为DBNull.Value，即插入数据库后仍然保持null;
                if (paraData == null)
                    param.Value = System.DBNull.Value;
                else
                    param.Value = paraData;

                param.Size = size;

                // 如果是新参数，添加参数到Command中
                if (blnNewParam)
                    command.Parameters.Add(param);
            }
            else
            {
                throw new Exceptions.DbException("错误的参数类型，请调用相应数据库对应的参数设置方法！");
            }
        }

        /// <summary>
        /// 设置SQL参数的值，当参数类型为varchar、nvarchar、char、nchar、binary、varbinary类型时，指定参数的长度。
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="paraData">要设置的参数值</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="size">字符串长度</param>
        public void SetParameter(string paraName, object paraData, NpgsqlTypes.NpgsqlDbType dbType, int size)
        {
            SetParameter(paraName, paraData, dbType, size, ParameterDirection.Input);
        }

        /// <summary>
        /// 设置SQL参数的值。数据类型为decimal、numeric时调用此方法
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="paraData">要设置的参数值</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="precision">参数的最大位数。 </param>
        /// <param name="scale">参数的小数位数</param>
        /// <param name="direction">参数方向</param>
        public void SetParameter(string paraName, object paraData, NpgsqlTypes.NpgsqlDbType dbType, byte precision, byte scale, ParameterDirection direction)
        {
            if (DataProvider == DataProvider.PostgreSQL)
            {
                Npgsql.NpgsqlParameter param;
                Npgsql.NpgsqlCommand command = (Npgsql.NpgsqlCommand)_DbCommand;
                bool blnNewParam = false;

                if (command.Parameters.IndexOf(paraName) < 0)
                {
                    blnNewParam = true;
                    param = command.CreateParameter();
                    param.ParameterName = paraName;
                }
                else
                {
                    param = command.Parameters[paraName];
                }

                param.NpgsqlDbType = dbType;
                param.Direction = direction;
                // 如果值为空，则将值设置为DBNull.Value，即插入数据库后仍然保持null;
                if (paraData == null)
                    param.Value = System.DBNull.Value;
                else
                    param.Value = paraData;

                param.Precision = precision;
                param.Scale = scale;

                // 如果是新参数，添加参数到Command中
                if (blnNewParam)
                    command.Parameters.Add(param);
            }
            else
            {
                throw new Exceptions.DbException("错误的参数类型，请调用相应数据库对应的参数设置方法！");
            }
        }

        /// <summary>
        /// 设置SQL参数的值。数据类型为decimal、numeric时调用此方法
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="paraData">要设置的参数值</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="precision">参数的最大位数。 </param>
        /// <param name="scale">参数的小数位数</param>
        public void SetParameter(string paraName, object paraData, NpgsqlTypes.NpgsqlDbType dbType, byte precision, byte scale)
        {
            SetParameter(paraName, paraData, dbType, precision, scale, ParameterDirection.Input);
        }
        #endregion

        #region 清理参数
        /// <summary>
        /// 清空参数
        /// </summary>
        public void ClearParameter()
        {
            _DbCommand.Parameters.Clear();
        }
        #endregion

        #region 日志处理
        /// <summary>
        /// 将错误信息根据数据库连接的配置节写入日志文件
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="ex"></param>
        private void WriteErrorLog(Exception ex)
        {
            if (_Connection.ErrorLog)
            {
                string logFile = Path.Combine(_Connection.ErrorLogPath, DateTime.Now.ToString("yyyyMMdd") + ".log");
                FileStream fs = null;
                StreamWriter sw = null;

                try
                {
                    // 需要将错误信息写入到日志文件
                    string strError = "错误时间：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")
        + "\r\n错误信息：" + ex.Message
        + "\r\n错误堆栈：" + ex.StackTrace
        + "\r\n错误语句：\r\n" + _DbCommand.CommandText;

                    foreach (DbParameter param in _DbCommand.Parameters)
                    {
                        strError += "\r\n" + param.ParameterName + "='" + param.Value.ToString() + "'";
                    }

                    strError += "\r\n==============================================================";

                    // 将错误记录到日志中
                    fs = new FileStream(logFile, FileMode.Append, FileAccess.Write, FileShare.Read);
                    sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
                    sw.Write(strError);
                    sw.Flush();
                }
                catch { }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                        fs.Dispose();
                    }
                }
            }
        }
        #endregion 日志处理

        #region 执行Sql操作
        /// <summary>
        /// 执行Sql操作，并返回影响的行数
        /// </summary>
        /// <returns>影响操作的行数</returns>
        public int ExecuteNonQuery()
        {
            // 设置命令的事务
            if (_DbTransaction != null && _DbCommand.Transaction != _DbTransaction)
                _DbCommand.Transaction = _DbTransaction;

            if (_Connection.ErrorLog)
            {
                // 如果需要写日志，则用try{}catch{}
                try
                {
                    return _DbCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    WriteErrorLog(ex);
                    throw;
                }
            }
            else
            {
                return _DbCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 执行Sql操作，并返回执行结果的第一行第一列数据
        /// </summary>
        /// <returns>执行后的第一行第一列数据</returns>
        public object ExecuteScalar()
        {
            // 设置命令的事务
            if (_DbTransaction != null && _DbCommand.Transaction != _DbTransaction)
                _DbCommand.Transaction = _DbTransaction;

            if (_Connection.ErrorLog)
            {
                // 如果需要写日志，则用try{}catch{}
                try
                {
                    return _DbCommand.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    WriteErrorLog(ex);
                    throw;
                }
            }
            else
            {
                return _DbCommand.ExecuteScalar();
            }
        }

        /// <summary>
        /// 执行并返回DataTable。如果查询语句为复合查询（返回多个结果的），此操作只返回第一个查询的结果。
        /// 如果配置文件为表的Read操作配置了多个连接，则只执行第一个连接的操作结果，忽略其它连接操作结果
        /// </summary>
        /// <returns>执行结果</returns>
        public DataTable ExecuteDataTable()
        {
            IDataReader reader = null;
            DataTable dtReturn = new DataTable(TableName);
            try
            {
                // 设置命令的事务
                if (_DbTransaction != null && _DbCommand.Transaction != _DbTransaction)
                    _DbCommand.Transaction = _DbTransaction;

                // 执行DBCommand，返回DataReader对象
                reader = _DbCommand.ExecuteReader();

                // 如果返回的DataTable对象还没有构建结构，则第一次根据DataReader返回的记录创建表结构
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = reader.GetName(i);
                    System.Type type = reader.GetFieldType(i);
                    DataColumn column = new DataColumn(columnName, type);
                    dtReturn.Columns.Add(column);
                }

                while (reader.Read())
                {
                    // 在DataTable中添加一条记录
                    DataRow dr = dtReturn.NewRow();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string columnName = reader.GetName(i);
                        dr[columnName] = reader.GetValue(i);
                    }
                    dtReturn.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw;
            }
            finally
            {
                // 一定要关闭DataReader，以释放被占用的数据库连接资源
                if (reader != null)
                {
                    reader.Dispose();
                }
            }
            return dtReturn;
        }

        /// <summary>
        /// 分页输出查询结果。如果查询语句为复合查询（返回多个结果的），此操作只返回第一个查询的结果。
        /// 说明：在大数据量时性能较低，建议如果要获取总页数时，先使用SELECT COUNT(1)计算出查询结果的总记录数，再调用ExecuteDataTable(int, int)方法获取当前页数据
        /// 建议：查询结果集合超过2万条记录时，不建议采用此方法
        /// </summary>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNumber">当前页码，计数器从1开始</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(int pageSize, int pageNumber, out int recordCount, out int pageCount)
        {
            IDataReader reader = null;
            DataTable dtReturn = new DataTable(TableName);
            int startRowIndex = (pageNumber - 1) * pageSize + 1;
            int endRowIndex = startRowIndex + pageSize - 1;
            int currentIndex = 0;

            try
            {
                // 设置命令的事务
                if (_DbTransaction != null && _DbCommand.Transaction != _DbTransaction)
                    _DbCommand.Transaction = _DbTransaction;

                // 执行DBCommand，返回DataReader对象
                reader = _DbCommand.ExecuteReader();

                // 如果返回的DataTable对象还没有构建结构，则第一次根据DataReader返回的记录创建表结构
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = reader.GetName(i);
                    System.Type type = reader.GetFieldType(i);
                    DataColumn column = new DataColumn(columnName, type);
                    dtReturn.Columns.Add(column);
                }

                while (reader.Read())
                {
                    // 给计数器加1，这样第一行的索引就是1，而不是0
                    currentIndex++;

                    if (currentIndex >= startRowIndex && currentIndex <= endRowIndex)
                    {
                        // 如果记录在获取范围内，则在DataTable中添加一条记录
                        DataRow dr = dtReturn.NewRow();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);
                            dr[columnName] = reader.GetValue(i);
                        }
                        dtReturn.Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw;
            }
            finally
            {
                // 一定要关闭DataReader，以释放被占用的数据库连接资源
                if (reader != null)
                {
                    reader.Dispose();
                }
            }

            // 计算记录数与页数量
            recordCount = currentIndex;
            int.TryParse(Math.Ceiling(recordCount * 1d / pageSize).ToString(), out pageCount);

            return dtReturn;
        }

        /// <summary>
        /// 分页输出查询结果。此方法用于不需要知道总记录数的情况，有助于提升查询性能。
        /// </summary>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNumber">当前页码，计数器从1开始</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(int pageSize, int pageNumber)
        {
            IDataReader reader = null;
            DataTable dtReturn = new DataTable(TableName);
            int startRowIndex = (pageNumber - 1) * pageSize + 1;
            int endRowIndex = startRowIndex + pageSize - 1;
            int currentIndex = 0;

            try
            {
                // 设置命令的事务
                if (_DbTransaction != null && _DbCommand.Transaction != _DbTransaction)
                    _DbCommand.Transaction = _DbTransaction;

                // 执行DBCommand，返回DataReader对象
                reader = _DbCommand.ExecuteReader();

                // 如果返回的DataTable对象还没有构建结构，则第一次根据DataReader返回的记录创建表结构
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = reader.GetName(i);
                    System.Type type = reader.GetFieldType(i);
                    DataColumn column = new DataColumn(columnName, type);
                    dtReturn.Columns.Add(column);
                }

                while (reader.Read())
                {
                    // 给计数器加1，这样第一行的索引就是1，而不是0
                    currentIndex++;

                    if (currentIndex >= startRowIndex && currentIndex <= endRowIndex)
                    {
                        // 如果记录在获取范围内，则在DataTable中添加一条记录
                        DataRow dr = dtReturn.NewRow();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);
                            dr[columnName] = reader.GetValue(i);
                        }
                        dtReturn.Rows.Add(dr);
                    }

                    if (currentIndex >= endRowIndex)
                    {
                        // 如果已获取到所需的数据，跳出循环
                        _DbCommand.Cancel();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw;
            }
            finally
            {
                // 一定要关闭DataReader，以释放被占用的数据库连接资源
                if (reader != null)
                {
                    reader.Dispose();
                }
            }

            return dtReturn;
        }

        /// <summary>
        /// 执行操作并返回DataSet，此操作大多用于产生多个查询结果的操作。
        /// </summary>
        /// <param name="dataSetName">返回的DataSet名称</param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string dataSetName)
        {
            IDataReader reader = null;
            DataSet dsReturn = new DataSet(dataSetName);
            try
            {
                // 设置命令的事务
                if (_DbTransaction != null && _DbCommand.Transaction != _DbTransaction)
                    _DbCommand.Transaction = _DbTransaction;

                // 执行DBCommand，返回DataReader对象
                reader = _DbCommand.ExecuteReader();
                dsReturn.Tables.Add(ExecuteDataTable(reader));
                while (reader.NextResult())
                {
                    // 如果还有数据集，重复读一遍
                    dsReturn.Tables.Add(ExecuteDataTable(reader));
                }
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw;
            }
            finally
            {
                // 一定要关闭DataReader，以释放被占用的数据库连接资源
                if (reader != null)
                {
                    reader.Dispose();
                }
            }
            return dsReturn;
        }

        /// <summary>
        /// 执行操作并返回DataSet，此操作大多用于产生多个查询结果的操作。
        /// </summary>
        /// <returns></returns>
        public DataSet ExecuteDataSet()
        {
            return ExecuteDataSet(LogicDbName);
        }

        private DataTable ExecuteDataTable(IDataReader reader)
        {
            DataTable dt = new DataTable(TableName);

            // 如果返回的DataTable对象还没有构建结构，则第一次根据DataReader返回的记录创建表结构
            for (int i = 0; i < reader.FieldCount; i++)
            {
                string columnName = reader.GetName(i);
                System.Type type = reader.GetFieldType(i);
                DataColumn column = new DataColumn(columnName, type);
                dt.Columns.Add(column);
            }

            while (reader.Read())
            {
                // 在DataTable中添加一条记录
                DataRow dr = dt.NewRow();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = reader.GetName(i);
                    dr[columnName] = reader.GetValue(i);
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// 根据数据库类型选择最优的分页查询。此方法不支持sql server 2000
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldList">要获取的字段列表集合，多个字段间以,隔开，必填</param>
        /// <param name="whereClause">查询条件，查询条件不需要加where关键字</param>
        /// <param name="orderBy">排序字段，必填</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNumber">当前页码，计数器从1开始</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string tableName, string fieldList, string whereClause, string orderBy, int pageSize, int pageNumber, out int recordCount, out int pageCount)
        {
            recordCount = GetRecordCount(tableName, whereClause);
            int.TryParse(Math.Ceiling(recordCount * 1d / pageSize).ToString(), out pageCount);

            return ExecuteDataTable(tableName, fieldList, whereClause, orderBy, pageSize, pageNumber);
        }

        /// <summary>
        /// 根据数据库类型选择最优的分页查询。此方法不支持sql server 2000
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldList">要获取的字段列表集合，多个字段间以,隔开，必填</param>
        /// <param name="whereClause">查询条件，查询条件不需要加where关键字</param>
        /// <param name="orderBy">排序字段，SQL Server时必填</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNumber">当前页码，计数器从1开始</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string tableName, string fieldList, string whereClause, string orderBy, int pageSize, int pageNumber)
        {
            BuildQuery(tableName, fieldList, whereClause, orderBy, pageSize, pageNumber);

            return ExecuteDataTable();
        }

        /// <summary>
        /// 获取实体列表内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public BindingList<T> ExecuteEntityList<T>() where T : new()
        {
            IDataReader reader = null;
            BindingList<T> lstItem = new BindingList<T>();

            try
            {
                // 设置命令的事务
                if (_DbTransaction != null && _DbCommand.Transaction != _DbTransaction)
                    _DbCommand.Transaction = _DbTransaction;

                // 执行DBCommand，返回DataReader对象
                reader = _DbCommand.ExecuteReader();

                // 获取实体的所有属性
                Dictionary<PropertyInfo, int> dicProperties = new Dictionary<PropertyInfo, int>();
                System.Reflection.PropertyInfo[] p = typeof(T).GetProperties();
                for (int i = 0; i < p.Length; i++)
                {
                    for (int j = 0; j < reader.FieldCount; j++)
                    {
                        // 字段名和属性名不区分大小写
                        if (p[i].Name.Equals(reader.GetName(j), StringComparison.OrdinalIgnoreCase))
                        {
                            dicProperties.Add(p[i], j);
                            break;
                        }
                    }
                }

                while (reader.Read())
                {
                    // 在列表中添加一条记录
                    T item = new T();

                    foreach (PropertyInfo property in dicProperties.Keys)
                    {
                        object obj = reader.GetValue(dicProperties[property]);
                        if (obj == System.DBNull.Value || obj == null)
                        {
                            if (property.PropertyType.IsPrimitive)
                            {
                                // 如果属性为基础类型，则设置默认值
                                obj = GetTypeDefaultValue(property.PropertyType);
                            }
                            else
                            {
                                obj = null;
                            }
                        }
                        property.SetValue(item, obj, null);
                    }

                    //dtReturn.Rows.Add(dr);
                    lstItem.Add(item);
                }
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw;
            }
            finally
            {
                // 一定要关闭DataReader，以释放被占用的数据库连接资源
                if (reader != null)
                {
                    reader.Dispose();
                }
            }
            return lstItem;
        }

        /// <summary>
        /// 获取实体列表内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNumber">当前页码，计数器从1开始</param>
        /// <returns></returns>
        public BindingList<T> ExecuteEntityList<T>(int pageSize, int pageNumber) where T : new()
        {
            IDataReader reader = null;
            BindingList<T> lstItem = new BindingList<T>();
            int startRowIndex = (pageNumber - 1) * pageSize + 1;
            int endRowIndex = startRowIndex + pageSize - 1;
            int currentIndex = 0;

            try
            {
                // 设置命令的事务
                if (_DbTransaction != null && _DbCommand.Transaction != _DbTransaction)
                    _DbCommand.Transaction = _DbTransaction;

                // 执行DBCommand，返回DataReader对象
                reader = _DbCommand.ExecuteReader();

                // 获取实体的所有属性
                Dictionary<PropertyInfo, int> dicProperties = new Dictionary<PropertyInfo, int>();
                System.Reflection.PropertyInfo[] p = typeof(T).GetProperties();
                for (int i = 0; i < p.Length; i++)
                {
                    for (int j = 0; j < reader.FieldCount; j++)
                    {
                        // 字段名和属性名不区分大小写
                        if (p[i].Name.Equals(reader.GetName(j), StringComparison.OrdinalIgnoreCase))
                        {
                            dicProperties.Add(p[i], j);
                            break;
                        }
                    }
                }

                while (reader.Read())
                {
                    // 给计数器加1，这样第一行的索引就是1，而不是0
                    currentIndex++;

                    if (currentIndex >= startRowIndex && currentIndex <= endRowIndex)
                    {
                        // 如果记录在获取范围内，则在列表中添加一条记录
                        T item = new T();

                        foreach (PropertyInfo property in dicProperties.Keys)
                        {
                            object obj = reader.GetValue(dicProperties[property]);
                            if (obj == System.DBNull.Value || obj == null)
                            {
                                if (property.PropertyType.IsPrimitive)
                                {
                                    // 如果属性为基础类型，则设置默认值
                                    obj = GetTypeDefaultValue(property.PropertyType);
                                }
                                else
                                {
                                    obj = null;
                                }
                            }
                            property.SetValue(item, obj, null);
                        }

                        //dtReturn.Rows.Add(dr);
                        lstItem.Add(item);
                    }

                    if (currentIndex >= endRowIndex)
                    {
                        // 如果已获取到所需的数据，跳出循环
                        _DbCommand.Cancel();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw;
            }
            finally
            {
                // 一定要关闭DataReader，以释放被占用的数据库连接资源
                if (reader != null)
                {
                    reader.Dispose();
                }
            }
            return lstItem;
        }

        /// <summary>
        /// 获取实体列表内容，此方法会导致查询结果的全部扫描，如果查询结果数较大，性能会受到影响。
        /// 如果不需要总记录数，请使用BindingList<T> ExecuteEntityList<T>(int, int)方法以提高性能
        /// 对于查询结果数较大的分页，建议先通过select count(1)查询获取总记录数后，再通过BindingList<T> ExecuteEntityList<T>(int,int)方法获取指定页的数据。
        /// 这样会避免扫描全部数据而带来性能影响。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNumber">当前页码，计数器从1开始</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns></returns>
        public BindingList<T> ExecuteEntityList<T>(int pageSize, int pageNumber, out int recordCount, out int pageCount) where T : new()
        {
            IDataReader reader = null;
            BindingList<T> lstItem = new BindingList<T>();
            int startRowIndex = (pageNumber - 1) * pageSize + 1;
            int endRowIndex = startRowIndex + pageSize - 1;
            int currentIndex = 0;

            try
            {
                // 设置命令的事务
                if (_DbTransaction != null && _DbCommand.Transaction != _DbTransaction)
                    _DbCommand.Transaction = _DbTransaction;

                // 执行DBCommand，返回DataReader对象
                reader = _DbCommand.ExecuteReader();

                // 获取实体的所有属性
                Dictionary<PropertyInfo, int> dicProperties = new Dictionary<PropertyInfo, int>();
                System.Reflection.PropertyInfo[] p = typeof(T).GetProperties();
                for (int i = 0; i < p.Length; i++)
                {
                    for (int j = 0; j < reader.FieldCount; j++)
                    {
                        // 字段名和属性名不区分大小写
                        if (p[i].Name.Equals(reader.GetName(j), StringComparison.OrdinalIgnoreCase))
                        {
                            dicProperties.Add(p[i], j);
                            break;
                        }
                    }
                }

                while (reader.Read())
                {
                    // 给计数器加1，这样第一行的索引就是1，而不是0
                    currentIndex++;

                    if (currentIndex >= startRowIndex && currentIndex <= endRowIndex)
                    {
                        // 如果记录在获取范围内，在列表中添加一条记录
                        T item = new T();

                        foreach (PropertyInfo property in dicProperties.Keys)
                        {
                            object obj = reader.GetValue(dicProperties[property]);
                            if (obj == System.DBNull.Value || obj == null)
                            {
                                if (property.PropertyType.IsPrimitive)
                                {
                                    // 如果属性为基础类型，则设置默认值
                                    obj = GetTypeDefaultValue(property.PropertyType);
                                }
                                else
                                {
                                    obj = null;
                                }
                            }
                            property.SetValue(item, obj, null);
                        }

                        //dtReturn.Rows.Add(dr);
                        lstItem.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw;
            }
            finally
            {
                // 一定要关闭DataReader，以释放被占用的数据库连接资源
                if (reader != null)
                {
                    reader.Dispose();
                }
            }
            // 计算记录数与页数量
            recordCount = currentIndex;
            int.TryParse(Math.Ceiling(recordCount * 1d / pageSize).ToString(), out pageCount);

            return lstItem;
        }

        /// <summary>
        /// 获取实体列表内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNumber">当前页码，计数器从1开始</param>
        /// <returns></returns>
        public BindingList<T> ExecuteEntityList<T>(string tableName, string fieldList, string whereClause, string orderBy, int pageSize, int pageNumber) where T : new()
        {
            BuildQuery(tableName, fieldList, whereClause, orderBy, pageSize, pageNumber);
            return ExecuteEntityList<T>();
        }

        /// <summary>
        /// 获取实体列表内容，此方法会导致查询结果的全部扫描，如果查询结果数较大，性能会受到影响。
        /// 如果不需要总记录数，请使用BindingList<T> ExecuteEntityList<T>(int, int)方法以提高性能
        /// 对于查询结果数较大的分页，建议先通过select count(1)查询获取总记录数后，再通过BindingList<T> ExecuteEntityList<T>(int,int)方法获取指定页的数据。
        /// 这样会避免扫描全部数据而带来性能影响。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNumber">当前页码，计数器从1开始</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns></returns>
        public BindingList<T> ExecuteEntityList<T>(string tableName, string fieldList, string whereClause, string orderBy, int pageSize, int pageNumber, out int recordCount, out int pageCount) where T : new()
        {
            recordCount = GetRecordCount(tableName, whereClause);
            int.TryParse(Math.Ceiling(recordCount * 1d / pageSize).ToString(), out pageCount);

            return ExecuteEntityList<T>(tableName, fieldList, whereClause, orderBy, pageSize, pageNumber);
        }

        /// <summary>
        /// 获取执行结果的第一条记录
        /// </summary>
        /// <returns>执行结果的第一条实体记录</returns>
        public T ExecuteEntity<T>() where T : new()
        {
            IDataReader reader = null;

            try
            {
                // 设置命令的事务
                if (_DbTransaction != null && _DbCommand.Transaction != _DbTransaction)
                    _DbCommand.Transaction = _DbTransaction;

                // 执行DBCommand，返回DataReader对象
                reader = _DbCommand.ExecuteReader();

                // 获取实体的所有属性
                Dictionary<PropertyInfo, int> dicProperties = new Dictionary<PropertyInfo, int>();
                System.Reflection.PropertyInfo[] p = typeof(T).GetProperties();
                for (int i = 0; i < p.Length; i++)
                {
                    for (int j = 0; j < reader.FieldCount; j++)
                    {
                        // 字段名和属性名不区分大小写
                        if (p[i].Name.Equals(reader.GetName(j), StringComparison.OrdinalIgnoreCase))
                        {
                            dicProperties.Add(p[i], j);
                            break;
                        }
                    }
                }

                if (reader.Read())
                {
                    // 在列表中添加一条记录
                    T item = new T();

                    foreach (PropertyInfo property in dicProperties.Keys)
                    {
                        object obj = reader.GetValue(dicProperties[property]);
                        if (obj == System.DBNull.Value || obj == null)
                        {
                            if (property.PropertyType.IsPrimitive)
                            {
                                // 如果属性为基础类型，则设置默认值
                                obj = GetTypeDefaultValue(property.PropertyType);
                            }
                            else
                            {
                                obj = null;
                            }
                        }
                        property.SetValue(item, obj, null);
                    }

                    //dtReturn.Rows.Add(dr);
                    return item;
                }
                else
                {
                    return default(T);
                }
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw;
            }
            finally
            {
                // 一定要关闭DataReader，以释放被占用的数据库连接资源
                if (reader != null)
                {
                    reader.Dispose();
                }
            }
        }

        /// <summary>
        /// 根据表名、字段列表、查询条件和排序方式，根据数据库系统类型，自动返回适合该数据库系统的分页查询Sql语句。
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldList">字段列表</param>
        /// <param name="whereClause">WHERE条件（请不要加WHERE子句）</param>
        /// <param name="orderBy">排序字段列表（不要加Order BY子句）</param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        public void BuildQuery(string tableName, string fieldList, string whereClause, string orderBy, int pageSize, int pageNumber)
        {
            #region POSTGRESQL数据库需要在表名外面加双引号
            if (DataProvider == DataProvider.PostgreSQL)
            {
                // POSTGRESQL数据库需要在表名外面加双引号
                if (!tableName.StartsWith("\""))
                    tableName = "\"" + tableName;
                if (!tableName.EndsWith("\""))
                    tableName += "\"";
            }
            #endregion

            #region 判断用户在whereClause前是不是加了WHERE，如果是，则替换掉
            if (whereClause.Trim().StartsWith("where ", StringComparison.OrdinalIgnoreCase))
            {
                whereClause = whereClause.Trim().Substring(6);
            }
            #endregion

            #region 判断OrderBy前是不是加了order by，如果是，则去掉
            if (orderBy.Trim().StartsWith("order by ", StringComparison.OrdinalIgnoreCase))
            {
                orderBy = orderBy.Trim().Substring(9);
            }
            #endregion

            int startIndex = (pageNumber - 1) * pageSize + 1;
            int endIndex = startIndex + pageSize - 1;

            if (DataProvider == DataProvider.SqlServer)
            {
                #region SQL Server数据库时（需要Sql Server 2005以上版本才支持此语法）
                CommandText = @"SELECT " + fieldList + @" FROM
(
	SELECT *,ROW_NUMBER() OVER (ORDER BY " + orderBy + @") AS RowNumber FROM " + tableName + " NOLOCK";

                if (!string.IsNullOrEmpty(whereClause.Trim()))
                {
                    CommandText += " WHERE " + whereClause;
                }

                CommandText += @") AS TempTable
WHERE (RowNumber >= @Start) AND (RowNumber <= @End)";
                SetParameter("Start", startIndex, SqlDbType.Int);
                SetParameter("End", endIndex, SqlDbType.Int);
                #endregion
            }
            else if (DataProvider == DataProvider.MySql)
            {
                #region MySql数据库时
                CommandText = @"SELECT " + fieldList + " FROM " + tableName + " NOLOCK";

                if (!string.IsNullOrEmpty(whereClause.Trim()))
                {
                    CommandText += " WHERE " + whereClause;
                }

                if (!string.IsNullOrEmpty(orderBy))
                {
                    CommandText += " ORDER BY " + orderBy;
                }
                CommandText += " LIMIT " + (pageSize * (pageNumber - 1)).ToString() + "," + pageSize.ToString();
                #endregion
            }
            else if (DataProvider == DataProvider.PostgreSQL)
            {
                #region PostgreSQL数据库时
                CommandText = @"SELECT " + fieldList + " FROM " + tableName + " NOLOCK";

                if (!string.IsNullOrEmpty(whereClause.Trim()))
                {
                    CommandText += " WHERE " + whereClause;
                }

                if (!string.IsNullOrEmpty(orderBy))
                {
                    CommandText += " ORDER BY " + orderBy;
                }
                CommandText += " LIMIT " + pageSize.ToString() + " OFFSET " + ((pageNumber - 1) * pageSize).ToString();
                #endregion
            }
            else
            {
                #region 其它数据库类型，不一定兼容
                CommandText = "SELECT TOP " + pageSize.ToString() + " " + fieldList + " FROM " + tableName + " A NOLOCK WHERE NOT EXISTS(SELECT 1 FROM(";
                CommandText += "SELECT TOP " + ((pageSize - 1) * pageSize).ToString() + " " + orderBy + " FROM " + tableName + " NOLOCK ";
                CommandText += "WHERE " + whereClause + " ORDER BY " + orderBy;
                CommandText += ") AS B WHERE ";

                string[] arrPrimaryKeys = orderBy.Split(',');
                for (int i = 0; i < arrPrimaryKeys.Length; i++)
                {
                    CommandText += "A." + arrPrimaryKeys[i].Trim() + "=B." + arrPrimaryKeys[i].Trim();
                    if (i < arrPrimaryKeys.Length - 1)
                    {
                        CommandText += " AND ";
                    }
                }
                CommandText += ") AND " + whereClause + " ORDER BY " + orderBy;
                #endregion
            }
        }

        /// <summary>
        /// 根据查询
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldList"></param>
        /// <param name="whereClause"></param>
        /// <returns></returns>
        public int GetRecordCount(string tableName, string whereClause)
        {
            #region POSTGRESQL数据库需要在表名外面加双引号
            if (DataProvider == DataProvider.PostgreSQL)
            {
                // POSTGRESQL数据库需要在表名外面加双引号
                if (!tableName.StartsWith("\""))
                    tableName = "\"" + tableName;
                if (!tableName.EndsWith("\""))
                    tableName += "\"";
            }
            #endregion

            #region 判断用户在whereClause前是不是加了WHERE，如果是，则替换掉
            if (whereClause.Trim().StartsWith("where ", StringComparison.OrdinalIgnoreCase))
            {
                whereClause = whereClause.Trim().Substring(6);
            }
            #endregion

            CommandText = "SELECT COUNT(1) FROM " + tableName + " NOLOCK";
            if (whereClause.Trim() != "")
            {
                CommandText += " WHERE " + whereClause;
            }
            int recordCount = int.Parse(ExecuteScalar().ToString());

            return recordCount;
        }

        /// <summary>
        /// 字符串转换方法
        /// </summary>
        /// <param name="t">数据类型</param>
        /// <returns></returns>
        private object GetTypeDefaultValue(Type t)
        {
            if (t == typeof(System.Boolean))
                return false;
            else if (t == typeof(System.Byte))
                return (System.Byte)0;
            else if (t == typeof(System.SByte))
                return (System.SByte)0;
            else if (t == typeof(System.Int16))
                return (System.Int16)0;
            else if (t == typeof(System.UInt16))
                return (System.UInt16)0;
            else if (t == typeof(System.Int32))
                return (System.Int32)0;
            else if (t == typeof(System.UInt32))
                return (System.UInt32)0;
            else if (t == typeof(System.Int64))
                return (System.Int64)0;
            else if (t == typeof(System.UInt64))
                return (System.UInt64)0;
            else if (t == typeof(System.IntPtr))
                return (System.IntPtr)0;
            else if (t == typeof(System.UIntPtr))
                return (System.UIntPtr)0;
            else if (t == typeof(System.Char))
                return (System.Char)0;
            else if (t == typeof(System.Double))
                return (System.Double)0;
            else if (t == typeof(System.Single))
                return (System.Single)0;
            else
                return 0;
        }
        #endregion

        #region 事物操作
        /// <summary>
        /// 开始事务
        /// </summary>
        public void BeginTransaction()
        {
            if (_DbTransaction != null)
                throw new Exceptions.DbException("当前上下文存在未提交或未回滚的事物，不允许在同一个上下文中嵌套事物。如需事物嵌套，请新建上下文连接！");
            _DbTransaction = _DbConnection.BeginTransaction();
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void CommitTransaction()
        {
            if (_DbTransaction == null)
                throw new Exceptions.DbException("事物不存在，或事物已提交/回滚！");
            _DbTransaction.Commit();
            _DbTransaction.Dispose();
            _DbTransaction = null;
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollbackTransaction()
        {
            if (_DbTransaction != null)
            {
                _DbTransaction.Rollback();
                _DbTransaction.Dispose();
                _DbTransaction = null;
            }
        }
        #endregion

        #region 与数据库结构相关的方法
        /// <summary>
        /// 根据数据库中的所有表
        /// </summary>
        /// <returns>数据库中所有的表</returns>
        public DataTable GetTables()
        {
            return _DbConnection.GetSchema(SqlClientMetaDataCollectionNames.Tables, new string[] { null, null, null, "BASE TABLE" });
        }

        /// <summary>
        /// 获取数据中所有的视图
        /// </summary>
        /// <returns>数据库中所有视图列表</returns>
        public DataTable GetViews()
        {
            return _DbConnection.GetSchema(SqlClientMetaDataCollectionNames.Views);
        }

        /// <summary>
        /// 获取数据库中指定表的所有字段
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>表的字段列表</returns>
        public DataTable GetColumns(string tableName)
        {
            return _DbConnection.GetSchema(SqlClientMetaDataCollectionNames.Columns, new string[] { null, null, tableName });
        }

        public DataTable GetPrimaryKeyColumns(string tableName)
        {
            DataTable dtPrimaryKeyColumns = new DataTable();
            dtPrimaryKeyColumns.Columns.Add("COLUMN_NAME", typeof(string));

            switch (DataProvider)
            {
                case DataProvider.SqlServer:
                    break;
                case DataProvider.MySql:
                    break;
                case DataProvider.PostgreSQL:
                    break;
            }

            return dtPrimaryKeyColumns;
        }
        #endregion
    }
}
