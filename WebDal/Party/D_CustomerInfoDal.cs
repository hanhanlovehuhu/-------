using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using WebModel;
using WebUtility;

namespace WebDal
{
    public class D_CustomerInfoDal
    {
        /// <summary>
        /// 获取客户种子数
        /// </summary>
        /// <returns></returns>
        public int SelectSeqClent()
        {
            AdjDBObject adjdbobject = new AdjDBObject();
            try
            {
                string cmdtext = "INSERT INTO D_Seq DEFAULT VALUES SELECT @@IDENTITY";
                adjdbobject.GetSqlStringCommand(cmdtext);
                return int.Parse(adjdbobject.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        /// <summary>
        /// 根据电话号码获取客户实体
        /// </summary>
        /// <param name="phone">电话号码</param>
        /// <returns></returns>
        public CustomerInfoEntity GetClientInfoByPhone(string phone)
        {
            CustomerInfoEntity tuijian = null;
           // AdjDBObject dbObject = new AdjDBObject("NewAidaijia");
            AdjDBObject adjDbObject = new AdjDBObject();
            string cmdtext = "select * from D_CustomerInfo WITH(NOLOCK) where cellphone=@phone";
            adjDbObject.GetSqlStringCommand(cmdtext);
            adjDbObject.AddInParameter("@phone", DbType.String, phone);
            using (IDataReader datareader = adjDbObject.ExecuteReader())
            {
                List<CustomerInfoEntity> reList = GlobalFunction.GetEntityList<CustomerInfoEntity>(datareader);
                if (reList != null)
                {
                    tuijian = reList[0];
                }
            }
            return tuijian;
        }

        /// <summary>
        /// 生成用户对应的推荐码
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string GenerateCustomerRecommendCode(int seq)
        {
            string returnStr = string.Empty;
            Number number = new Number("23456789QWERTYUPASDFGHJKZXCVBNM");
            string code = number.ToString(seq);
            int count = code.Length;
            string buStr = string.Empty;
            for (int i = 0; i < 6 - count; i++)
            {
                buStr += @"A";
            }
            returnStr = buStr + code;
            return returnStr;
        }

        /// <summary>
        /// 添加新用户
        /// </summary>
        /// <returns></returns>
        public bool AddCustomerInfo(CustomerInfoEntity accident)
        {
            bool isok = true;
            AdjDBObject adjdbobject = new AdjDBObject();
            try
            {
                string cmdtext =
                    "INSERT INTO dbo.D_CustomerInfo( CustomerId ,CustomerName ,RecommendCode ,Cellphone ,Email ,HomeAddress ,CustomerType ,AccountType ,IsNewCustomer ,LastCalledAddress ,Amount ,BusinessmanId ,create_time ,create_user,ParentPhone ) VALUES(@CustomerId ,@CustomerName ,@RecommendCode ,@Cellphone, @Email, @HomeAddress, @CustomerType ,@AccountType ,@IsNewCustomer, @LastCalledAddress, @Amount ,@BusinessmanId ,@create_time, @create_user,@ParentPhone )";

                adjdbobject.GetSqlStringCommand(cmdtext);
                adjdbobject.AddInParameter("@CustomerId", DbType.String, accident.CustomerId);
                adjdbobject.AddInParameter("@CustomerName", DbType.String, accident.CustomerName);
                adjdbobject.AddInParameter("@RecommendCode", DbType.String, accident.RecommendCode);
                adjdbobject.AddInParameter("@Cellphone", DbType.String, accident.Cellphone);
                adjdbobject.AddInParameter("@ParentPhone", DbType.String, accident.ParentPhone);
                adjdbobject.AddInParameter("@Email", DbType.String, "");
                adjdbobject.AddInParameter("@HomeAddress", DbType.String, accident.HomeAddress);
                adjdbobject.AddInParameter("@CustomerType", DbType.Int32, accident.CustomerType);
                adjdbobject.AddInParameter("@AccountType", DbType.Int32, accident.AccountType);
                adjdbobject.AddInParameter("@IsNewCustomer", DbType.Int32, accident.IsNewCustomer);
                adjdbobject.AddInParameter("@LastCalledAddress", DbType.String, accident.LastCalledAddress);
                adjdbobject.AddInParameter("@Amount", DbType.Decimal, accident.Amount);
                adjdbobject.AddInParameter("@BusinessmanId", DbType.Int32, accident.BusinessmanId);
                adjdbobject.AddInParameter("@create_time", DbType.DateTime, DateTime.Now);
                adjdbobject.AddInParameter("@create_user", DbType.String, "ThirdPartyAPI");
              adjdbobject.Execute();
            }
            catch (Exception ex)
            {
                isok = false;
            }
            return isok;
        }





    }
}
