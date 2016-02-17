using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;

namespace WebDal
{
    public class DBFactory
    {
        public static Database GetAdjDBObject()
        {
            return DatabaseFactory.CreateDatabase("SQLConnStr");
        }
    }

    public class AdjDBObject
    {
        Database AdjDB;
        DbCommand command;

        public AdjDBObject()
        {
            AdjDB = DBFactory.GetAdjDBObject();
        }

        /// <summary>
        /// SQL
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public void GetSqlStringCommand(string query)
        {
            command = this.AdjDB.GetSqlStringCommand(query);
        }

        /// <summary>
        /// 存储过程
        /// </summary>
        /// <param name="procName"></param>
        /// <returns></returns>
        public void GetStoredProcCommand(string procName)
        {
            command = this.AdjDB.GetStoredProcCommand(procName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public void AddInParameter(string name, DbType type, object value)
        {
            this.AdjDB.AddInParameter(this.command, name, type, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public void AddOutParameter(string name, DbType type, int size)
        {
            this.AdjDB.AddOutParameter(command, name, type, size);
        }

        public void AddReturnValue()
        {
            AdjDB.AddParameter(command, "RetVal", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Current, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader()
        {
            return this.AdjDB.ExecuteReader(command);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet()
        {
            return this.AdjDB.ExecuteDataSet(command);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public int Execute()
        {
            return AdjDB.ExecuteNonQuery(command);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public object GetParameterValue(String name)
        {
            return AdjDB.GetParameterValue(command, name);
        }

        public static T setValue<T>(DataRow dr, string colName)
        {
            return dr[colName] == DBNull.Value ? default(T) : (T)dr[colName];
        }

        public static T setValue<T>(IDataReader dr, string colname)
        {
            return dr[colname] == DBNull.Value ? default(T) : (T)dr[colname];
        }

        public int GetReturnValue()
        {
            return (int)(AdjDB.GetParameterValue(command, "RetVal"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public Object ExecuteScalar()
        {
            return this.AdjDB.ExecuteScalar(command);
        }

        /// <summary>
        /// 修正日期范围
        /// </summary>
        /// <param name="date">修正前日期</param>
        /// <returns>修正后日期</returns>
        public static DateTime? Check1753(DateTime? date)
        {
            if (date != null)
            {
                DateTime date1753 = new DateTime(1753, 1, 1);
                if (date < date1753)
                {
                    return date1753;
                }
                else
                {
                    return date;
                }
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 返回执行结果
        /// </summary>
        /// <returns></returns>
        public object ExecuteProc()
        {
            command.Parameters.Add(new SqlParameter("ReturnValue",
               SqlDbType.Int, 4, ParameterDirection.ReturnValue,
               false, 0, 0, string.Empty, DataRowVersion.Default, null));
            AdjDB.ExecuteNonQuery(command);
            return command.Parameters["ReturnValue"].Value;
        }

    }
}
