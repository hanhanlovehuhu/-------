using Aidaijia.API.BLL;
using Aidaijia.API.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Aidaijia.API
{
    /// <summary>
    /// CustomerServer 的摘要说明
    /// </summary>
    public class CustomerServer : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (!string.IsNullOrEmpty(context.Request.QueryString["method"]))
            {
                switch (context.Request.QueryString["method"])
                {
                    case "InsertCustomer":
                        InsertCustomer(context);
                        break;                   
                    default:
                        break;
                }
            }           
        }
        private void InsertCustomer(HttpContext context)
        {
            try
            {
                if (!string.IsNullOrEmpty(context.Request.QueryString["customer"]))
                {
                    string error = "";
                    var entity = JSONHelper.ParseFormByJson<InsertCustomer>(context.Request.QueryString["customer"]);                   
                    if (entity != null&&!string.IsNullOrEmpty(entity.CellPhone))
                    {
                       bool isInsert= CustomerBLL.InsertCustomer(entity,out error);
                       if (isInsert)
                       {
                           context.Response.Write("1");
                       }                        
                    }
                    else
                    {
                        context.Response.Write("0");
                    }
                }
            }
            catch (Exception)
            {
                context.Response.Write("0");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}