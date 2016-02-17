using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;

namespace Aidaijia.API.Common
{
   public class ThreadPoolControl
    {
        public static void Execute(WaitCallback threadDelegate)
        {

            try
            {
                ThreadPool.QueueUserWorkItem(threadDelegate);
            }
            catch (Exception ex)
            {
                LogControl.WriteError("《线程池错误》===》" + ex.Message);
            }
        }
        public static int GetMinThreads()
        {
            int workerThreads, completionPortThreads;
            ThreadPool.GetMinThreads(out workerThreads, out completionPortThreads);
            return workerThreads;
        }
        public static int GetMaxThreads()
        {
            int workerThreads, completionPortThreads;
            ThreadPool.GetMaxThreads(out workerThreads, out completionPortThreads);
            return workerThreads;
        }
        public static int GetAvailableThreads()
        {
            int workerThreads, completionPortThreads;
            ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
            return workerThreads;
        }
        public static void SetMaxThreads(int workerThreads, int completionPortThreads)
        {
            ThreadPool.SetMaxThreads(workerThreads, completionPortThreads);
        }
        public static void SetMinThreads(int workerThreads, int completionPortThreads)
        {
            ThreadPool.SetMinThreads(workerThreads, completionPortThreads);
        }

        public static void Init()
        {
            SetMaxThreads(Convert.ToInt32(ConfigurationManager.AppSettings["MaxWorkerNum"]), Convert.ToInt32(ConfigurationManager.AppSettings["MaxCompletNum"]));
        }
    }
}
