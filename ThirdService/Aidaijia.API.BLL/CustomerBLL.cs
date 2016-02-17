using Aidaijia.API.DAL;
using Aidaijia.API.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aidaijia.API.BLL
{
   public class CustomerBLL
    {
       static CustomerDal dal = new CustomerDal();
       public static Customer GetCustomerByCellPhone(string cellPhone)
        {
            return dal.GetCustomerByCellPhone(cellPhone);
        }
       public static bool InsertProblemRecord(ProblemRecord entity)
       {
           return dal.InsertProblemRecord(entity);
       }
       public static bool InsertCustomer(InsertCustomer entity, out string error)
       {
           return dal.InsertCustomer(entity,out error);
       }
    }
}
