using Aidaijia.API.BLL;
using Aidaijia.API.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Aidaijia.API
{
    /// <summary>
    /// 获取与新增评论  html5专用
    /// </summary>
    public class CommentOrder : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //Get请求
            if (!string.IsNullOrEmpty(context.Request.QueryString["ucode"]))
            {               
                try
                {
                    var list = OrderBLL.GetCommentOrderByUcode(context.Request.QueryString["ucode"], string.IsNullOrEmpty(context.Request.QueryString["cellPhone"]) ? "" : context.Request.QueryString["cellPhone"]);
                    if (list.Count > 0)
                    {
                        context.Response.Write(JSONHelper.GetJSON<List<CommentEntity>>(list));
                    }
                    else
                    {
                        context.Response.Write("[]");
                    }                    
                }
                catch (Exception )
                {
                    context.Response.Write("[]");
                }              
            }
            else if (!string.IsNullOrEmpty(context.Request.Form["comment"]))
            {
                try
                {
                    bool isInsert = OrderBLL.InsertComment(JSONHelper.ParseFormByJson<CommentEntity>(context.Request.Form["comment"]));
                    if (isInsert)
                    {
                        context.Response.Write("1");
                    }
                    else
                    {
                        context.Response.Write("0");
                    }
                }
                catch (Exception)
                {
                    context.Response.Write("0");
                }
               
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