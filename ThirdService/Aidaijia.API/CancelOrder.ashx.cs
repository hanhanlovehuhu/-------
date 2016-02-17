using Aidaijia.API.BLL;
using Aidaijia.API.Common;
using Aidaijia.API.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Aidaijia.API
{
    /// <summary>
    /// CancelOrder 的摘要说明
    /// </summary>
    public class CancelOrder : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                Stream s = context.Request.InputStream;
                using (var streamReader = new StreamReader(s, Encoding.UTF8))
                {
                    var json = streamReader.ReadToEnd();
                    var entity = JSONHelper.ParseFormByJson<Order>(json);
                    string sError = "";
                    bool isInsert = OrderBLL.InsertCallCenterOrderInfo(entity, out sError);
                    if (isInsert)
                    {                       
                        ret.Success = true;
                        ret.Message = "";
                    }
                    else
                    {
                        ret.Message = sError;
                    }
                }

            }
            catch (Exception ex)
            {
                LogControl.WriteError("catch" + ex.Message);
                ret.Message = ex.Message;
            }
            finally
            {
                Send(ret, context);
            }
        }
        private void Send(BaseReturn ret, HttpContext context)
        {
            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer =
               new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(BaseReturn));
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                serializer.WriteObject(ms, ret);
                byte[] b = ms.ToArray();
                context.Response.OutputStream.Write(b, 0, b.Length);
                context.Response.Flush();
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