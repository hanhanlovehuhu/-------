using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Services.Description;
using Microsoft.CSharp;

namespace WebUtility
{
    /// <summary>
    ///动态调用WebService
    /// </summary>
    public class DlWebService
    {

        /// <summary>
        /// 获取WebService类型
        /// </summary>
        /// <param name="wsUrl">WebService地址</param>
        /// <returns></returns>
        private static string GetWsClassName(string wsUrl)
        {
            string[] parts = wsUrl.Split('/');
            string[] pps = parts[parts.Length - 1].Split('.');

            return pps[0];
        }

        /// <summary>
        /// 调用WebService
        /// </summary>
        /// <param name="wsUrl">WebService地址</param>
        /// <param name="methodName">方法名称</param>
        /// <param name="args">参数列表</param>
        /// <returns></returns>
        public static object InvokeWebService(string wsUrl, string methodName, object[] args)
        {
            return InvokeWebService(wsUrl, null, methodName, args);
        }

        /// <summary>
        /// 调用WebService
        /// </summary>
        /// <param name="wsUrl">WebService地址</param>
        /// <param name="className">类名</param>
        /// <param name="methodName">方法名称</param>
        /// <param name="args">参数列表</param>
        /// <returns></returns>
        public static object InvokeWebService(string wsUrl, string className, string methodName, object[] args)
        {
            string @namespace = "EnterpriseServerBase.WebService.DynamicWebCalling";
            if ((className == null) || (className == ""))
            {
                className = GetWsClassName(wsUrl);
            }

            try
            {
                //获取WSDL
                WebClient wc = new WebClient();
                Stream stream = wc.OpenRead(wsUrl + "?wsdl");
                ServiceDescription sd = ServiceDescription.Read(stream);
                ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
                sdi.AddServiceDescription(sd, "", "");
                CodeNamespace cn = new CodeNamespace(@namespace);

                //生成客户端代理类代码
                CodeCompileUnit ccu = new CodeCompileUnit();
                ccu.Namespaces.Add(cn);
                sdi.Import(cn, ccu);
                CSharpCodeProvider csc = new CSharpCodeProvider();
                ICodeCompiler icc = csc.CreateCompiler();

                //设定编译参数
                CompilerParameters cplist = new CompilerParameters();
                cplist.GenerateExecutable = false;
                cplist.GenerateInMemory = true;
                cplist.ReferencedAssemblies.Add("System.dll");
                cplist.ReferencedAssemblies.Add("System.XML.dll");
                cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
                cplist.ReferencedAssemblies.Add("System.Data.dll");

                //编译代理类
                CompilerResults cr = icc.CompileAssemblyFromDom(cplist, ccu);
                if (true == cr.Errors.HasErrors)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach (System.CodeDom.Compiler.CompilerError ce in cr.Errors)
                    {
                        sb.Append(ce.ToString());
                        sb.Append(System.Environment.NewLine);
                    }
                    throw new Exception(sb.ToString());
                }

                //生成代理实例，并调用方法
                System.Reflection.Assembly assembly = cr.CompiledAssembly;
                Type t = assembly.GetType(@namespace + "." + className, true, true);
                object obj = Activator.CreateInstance(t);
                System.Reflection.MethodInfo mi = t.GetMethod(methodName);

                return mi.Invoke(obj, args);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message, new Exception(ex.InnerException.StackTrace));
            }
        }
    }
}
