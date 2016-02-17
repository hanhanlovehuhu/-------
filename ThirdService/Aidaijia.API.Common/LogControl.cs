using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Aidaijia.API.Common
{
   public class LogControl
    {
        private static object errorOb, noticeOb, infoOb, traceOb, moneyOb, sqlOb;
        private static bool NoticeLogFlag, InfoLogFlag, MoneyLogFlag, ErrorLogFlag, TraceLogFlag, SqlLogFlag;

        public static bool IsClosed { get; set; }
        static LogControl()
        {
            IsClosed = false;
            errorOb = new object();
            noticeOb = new object();
            infoOb = new object();
            traceOb = new object();
            moneyOb = new object();
            sqlOb = new object();
            var dic = System.AppDomain.CurrentDomain.BaseDirectory + "Log";
            if (!System.IO.Directory.Exists(dic))
                System.IO.Directory.CreateDirectory(dic);

            NoticeLogFlag = System.Configuration.ConfigurationManager.AppSettings["NoticeLogFlag"].ToString().ToLower() == "on";
            InfoLogFlag = System.Configuration.ConfigurationManager.AppSettings["InfoLogFlag"].ToString().ToLower() == "on";
            MoneyLogFlag = System.Configuration.ConfigurationManager.AppSettings["MoneyLogFlag"].ToString().ToLower() == "on";
            ErrorLogFlag = System.Configuration.ConfigurationManager.AppSettings["ErrorLogFlag"].ToString().ToLower() == "on";
            TraceLogFlag = System.Configuration.ConfigurationManager.AppSettings["TraceLogFlag"].ToString().ToLower() == "on";
            SqlLogFlag = System.Configuration.ConfigurationManager.AppSettings["SqlLogFlag"].ToString().ToLower() == "on";
        }
        public static void WriteError(string errorInfo)
        {
            if (!IsClosed && ErrorLogFlag)
                ThreadPoolControl.Execute((ob) =>
                {
                    lock (errorOb)
                    {
                        FileStream fs = new FileStream(System.AppDomain.CurrentDomain.BaseDirectory + "Log\\" + DateTime.Now.ToString("yyyy-MM-dd") + "ErrorLog.txt", FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                        StreamWriter m_streamWriter = new StreamWriter(fs);
                        m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
                        m_streamWriter.WriteLine(DateTime.Now.ToString() + Environment.NewLine + errorInfo + Environment.NewLine);
                        m_streamWriter.Flush();
                        m_streamWriter.Close();
                    }
                });
        }

        public static void WriteNotice(string info)
        {
            if (!IsClosed && NoticeLogFlag)
                ThreadPoolControl.Execute((ob) =>
                {
                    lock (noticeOb)
                    {
                        FileStream fs = new FileStream(System.AppDomain.CurrentDomain.BaseDirectory + "Log\\" + DateTime.Now.ToString("yyyy-MM-dd") + "NoticeLog.txt", FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                        StreamWriter m_streamWriter = new StreamWriter(fs);
                        m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
                        m_streamWriter.WriteLine(DateTime.Now.ToString() + Environment.NewLine + info + Environment.NewLine);
                        m_streamWriter.Flush();
                        m_streamWriter.Close();
                    }
                });

        }

        public static void WriteInfo(string info)
        {
            if (!IsClosed && InfoLogFlag)
               ThreadPoolControl.Execute((ob) =>
                {
                    lock (infoOb)
                    {
                        FileStream fs = new FileStream(System.AppDomain.CurrentDomain.BaseDirectory + "Log\\" + DateTime.Now.ToString("yyyy-MM-dd") + "InfoLog.txt", FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                        StreamWriter m_streamWriter = new StreamWriter(fs);
                        m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
                        m_streamWriter.WriteLine(DateTime.Now.ToString() + Environment.NewLine + info + Environment.NewLine);
                        m_streamWriter.Flush();
                        m_streamWriter.Close();
                    }
                });

        }

        public static void WriteTrace(string info)
        {
            if (!IsClosed && TraceLogFlag)
                ThreadPoolControl.Execute((ob) =>
                {
                    lock (traceOb)
                    {
                        FileStream fs = new FileStream(System.AppDomain.CurrentDomain.BaseDirectory + "Log\\" + DateTime.Now.ToString("yyyy-MM-dd") + "TraceLog.txt", FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                        StreamWriter m_streamWriter = new StreamWriter(fs);
                        m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
                        m_streamWriter.WriteLine(DateTime.Now.ToString() + Environment.NewLine + info + Environment.NewLine);
                        m_streamWriter.Flush();
                        m_streamWriter.Close();
                    }
                });

        }

        public static void WriteMoneyInfo(string info)
        {
            if (!IsClosed && MoneyLogFlag)
                ThreadPoolControl.Execute((ob) =>
                {
                    lock (moneyOb)
                    {
                        FileStream fs = new FileStream(System.AppDomain.CurrentDomain.BaseDirectory + "Log\\" + DateTime.Now.ToString("yyyy-MM-dd") + "MoneyInfoLog.txt", FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                        StreamWriter m_streamWriter = new StreamWriter(fs);
                        m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
                        m_streamWriter.WriteLine(DateTime.Now.ToString() + Environment.NewLine + info + Environment.NewLine);
                        m_streamWriter.Flush();
                        m_streamWriter.Close();
                    }
                });

        }

        public static void WriteSqlEx(string info)
        {
            if (!IsClosed && SqlLogFlag)
                ThreadPoolControl.Execute((ob) =>
                {
                    lock (sqlOb)
                    {
                        FileStream fs = new FileStream(System.AppDomain.CurrentDomain.BaseDirectory + "Log\\" + DateTime.Now.ToString("yyyy-MM-dd") + "SqlExLog.txt", FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                        StreamWriter m_streamWriter = new StreamWriter(fs);
                        m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
                        m_streamWriter.WriteLine(DateTime.Now.ToString() + Environment.NewLine + info + Environment.NewLine);
                        m_streamWriter.Flush();
                        m_streamWriter.Close();
                    }
                });

        }
    }
}
