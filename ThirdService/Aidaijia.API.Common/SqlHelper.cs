using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Aidaijia.API.Common
{
    public class SqlHelper
    {
        private string connectionString;
        public SqlHelper(string conName)
        {
            connectionString = ConfigurationManager.ConnectionStrings[conName].ConnectionString;
        }
        public int ExecuteCommand(string safeSql)
        {

            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(safeSql, connection);
                    int result = cmd.ExecuteNonQuery();
                    return result;
                }
                catch (Exception ex)
                {
                   LogControl.WriteSqlEx("SqlEx====>" + safeSql + Environment.NewLine + ex.Message);

                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return -1;

        }

        public int ExecuteCommandProc(string sqlName, SqlParameter[] values, int tryCount = 0)
        {

            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand(sqlName, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        //新增返回值
                        cmd.Parameters.Add(new SqlParameter("@Return", SqlDbType.Int));
                        if (values != null)
                        {
                            cmd.Parameters.AddRange(values);
                        }
                        cmd.Parameters["@Return"].Direction = ParameterDirection.ReturnValue;
                        connection.Open();
                        cmd.ExecuteNonQuery();
                        int r = cmd.Parameters["@Return"] == null ? -1 : Convert.ToInt32(cmd.Parameters["@Return"].Value);
                        cmd.Parameters.Clear();
                        return r;
                    }                   
                }
                catch (Exception ex)
                {
                      LogControl.WriteSqlEx("SqlEx Retry====>" + sqlName + Environment.NewLine + ex.Message);
                      return -1;                   
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }               
            }
        }

        public int ExecuteCommandProc(string sqlName, List<SqlParameter> values)
        {

            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sqlName, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();
                    foreach (var sqlParameter in values)
                    {
                        cmd.Parameters.Add(sqlParameter);
                    }

                    return cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                   LogControl.WriteSqlEx("SqlEx====>" + sqlName + Environment.NewLine + ex.Message);

                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }


            return -1;

        }

        public int ExecuteCommandProc(string sqlName, List<SqlParameter> values, ref SqlParameter outPara)
        {

            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sqlName, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();
                    foreach (var par in values)
                    {
                        cmd.Parameters.Add(par);
                    }
                    cmd.Parameters.Add(outPara);
                    cmd.Parameters[outPara.ParameterName].Direction = ParameterDirection.Output;
                    return cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                   LogControl.WriteSqlEx("SqlEx====>" + sqlName + Environment.NewLine + ex.Message);
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }


            return -1;

        }
        public int ExecuteCommand(string sql, SqlParameter[] value)
        {

            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    cmd.Parameters.AddRange(value);
                    int result = cmd.ExecuteNonQuery();
                    return result;
                }
                catch (Exception ex)
                {
                   LogControl.WriteSqlEx("SqlEx====>" + sql + Environment.NewLine + ex.Message);

                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return -1;



        }
        public int ExecuteCommand(string sql, List<SqlParameter> value)
        {
            SqlCommand cmd = null;
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    cmd = new SqlCommand(sql, connection);
                    foreach (var par in value)
                    {
                        cmd.Parameters.Add(par);
                    }
                    int result = cmd.ExecuteNonQuery();
                    return result;
                }
                catch (Exception ex)
                {
                    if (cmd != null)
                        LogControl.WriteSqlEx("SqlEx====>" + cmd.CommandText + Environment.NewLine + ex.Message);
                    else
                        LogControl.WriteSqlEx("SqlEx====>" + sql + Environment.NewLine + ex.Message);
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return -1;



        }
        public DataTable GetDataTable(string safeSql)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand(safeSql, connection);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    return ds.Tables[0];

                }
                catch (Exception ex)
                {
                   LogControl.WriteSqlEx("SqlEx====>" + safeSql + Environment.NewLine + ex.Message);

                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return null;



        }
        public DataTable GetDataTableStoredProcedure(string storeName, SqlParameter[] values)
        {


            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand(storeName, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(values);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    return ds.Tables[0];
                }
                catch (Exception ex)
                {
                   LogControl.WriteSqlEx("SqlEx====>" + storeName + Environment.NewLine + ex.Message);

                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return null;
        }

        public T GetModel<T>(string safeSql)
            where T : new()
        {

            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    var model = new T();
                    var table = GetDataTable(safeSql);
                    if (table != null && table.Rows.Count > 0)
                    {
                        var row = table.Rows[0];
                        foreach (var property in model.GetType().GetProperties())
                        {
                            if (property.CanWrite)
                                property.SetValue(model, Convert.ChangeType(row[property.Name], property.PropertyType), null);
                        }
                    }
                    return model;
                }
                catch (Exception ex)
                {
                  
                   LogControl.WriteSqlEx("SqlEx====>" + safeSql + Environment.NewLine + ex.Message);

                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return default(T);
        }



    }
}
