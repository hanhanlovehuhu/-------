using Aidaijia.API.Common;
using Aidaijia.API.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
namespace Aidaijia.API.DAL
{
  public  class CustomerDal
    {
        private SqlHelper helper;

        public CustomerDal()
       {
           helper = new SqlHelper("AiDaiJiaConStr");
       }
        public Customer GetCustomerByCellPhone(string cellPhone)
      {
          try
          {
              string sSql = string.Format(@"Select d.amount,d1.Cellphone,d1.CustomerName,d1.CustomerType,d1.CustomerId From D_CustomerInfo (nolock) d
                   left join
                    D_CustomerInfo (nolock) d1
                    on d.Cellphone=case when  isnull(d1.ParentPhone,'0')='0' then d1.Cellphone else d1.ParentPhone end
                   where d1.Cellphone='{0}' ", cellPhone);
              //插入订单
              var dt = helper.GetDataTable(sSql);
              if (dt != null && dt.Rows.Count > 0)
              {
                  Customer entity = new Customer();
                  entity.Amount = Convert.ToDecimal(dt.Rows[0]["Amount"] == DBNull.Value ? 0 : dt.Rows[0]["Amount"]);
                  entity.CellPhone =dt.Rows[0]["CellPhone"]==DBNull.Value?"": dt.Rows[0]["CellPhone"].ToString();
                  entity.CustomerName =dt.Rows[0]["CustomerName"]==DBNull.Value?"": dt.Rows[0]["CustomerName"].ToString();
                  entity.CustomerType = Convert.ToInt32(dt.Rows[0]["CustomerType"] == DBNull.Value ? 0 : dt.Rows[0]["CustomerType"]);
                  entity.CustomerId =dt.Rows[0]["CustomerId"]==DBNull.Value?"": dt.Rows[0]["CustomerId"].ToString();                
                  return entity;
              }
              else
              {
                  return null;
              }
          }
          catch (Exception ex)
          {
              LogControl.WriteError("查询用户失败|Error:" + ex.Message);
              return null;
          }

      }

        public bool InsertProblemRecord(ProblemRecord entity)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"
                  Insert Into [D_ProblemRecord](DriverName, Ucode, CustomerName, CustomerPhone,
                                    Record, Remark, State, QuestName, DisposeName, AddTime, RecordTitle, 
                                    ProvinceId, CityId, IsDelete)
                  Values('','',@CustomerName,@CustomerPhone,
                                    @Record, '',2, @QuestName, '',getdate(),@RecordTitle, 
                                    @ProvinceId,@CityId,0)");
                SqlParameter[] para = new SqlParameter[] {                   
                    new SqlParameter("@CustomerName", entity.CustomerName),
                    new SqlParameter("@CustomerPhone", entity.CustomerPhone),
                    new SqlParameter("@Record", entity.Record),
                    new SqlParameter("@QuestName", entity.QuestName),
                    new SqlParameter("@RecordTitle", entity.RecordTitle),
                    new SqlParameter("@ProvinceId", entity.CityId),
                    new SqlParameter("@CityId", entity.CityId)};
                var value = helper.ExecuteCommand(strSql.ToString(),para);
                return value > 0;
            }
            catch (Exception ex)
            {
                LogControl.WriteError(string.Format("插入历史记录失败|Error:{0}", ex.Message));
                return false;
            }
        }

        public bool InsertCustomer(InsertCustomer entity, out string error)
        {
            error = "";
            try
            {
                var paras = new SqlParameter[] { new SqlParameter("@CellPhone",SqlDbType.NVarChar,20),           
              new SqlParameter("@CustomerName", SqlDbType.NVarChar,20),            
              new SqlParameter("@CreateUser", SqlDbType.NVarChar,20),           
              new SqlParameter("@NewRecomendCode", SqlDbType.NVarChar,50),                 
              new SqlParameter("@ErrorMsg", SqlDbType.NVarChar,100)
            };
                paras[0].Value = entity.CellPhone;
                paras[1].Value = string.IsNullOrEmpty(entity.CustomerName) ? "" : entity.CustomerName;
                paras[2].Value = string.IsNullOrEmpty(entity.CreateUser) ? "sysSocketApi" : entity.CreateUser;
                paras[3].Value = string.IsNullOrEmpty(entity.NewRecomendCode) ? "" : entity.NewRecomendCode;
                paras[4].Direction = ParameterDirection.Output;
                //插入订单                 
                var value = helper.ExecuteCommandProc("gsp_InsertCustomerInfo", paras);
                if (value >= 0)
                {
                    return true;
                }
                else
                {
                    error = paras[4] == null ? "" : paras[4].Value == null ? "" : paras[4].Value.ToString();
                    return false;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                LogControl.WriteError("InsertCustomer插入用户信息失败|Error:" + ex.Message);
                return false;
            }

        }
    }
}
