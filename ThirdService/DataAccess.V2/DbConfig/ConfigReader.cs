using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Threading;

namespace DataAccess.V2.DbConfig
{
    /// <summary>
    /// 从XML配置文件中读取数据库配置节
    /// </summary>
    internal class ConfigReader
    {
        private string _IV = "*d:iQ,S&";
        private Dictionary<string, Module> _ModuleList = new Dictionary<string, Module>();
        private ConfigReader()
        {
            // 读取配置文件，存放到字典中
            string strAppDomainDirPath = System.AppDomain.CurrentDomain.BaseDirectory;
            string strMainConfigPath = System.IO.Path.Combine(strAppDomainDirPath, @"config\application.config");

            XmlDocument xmlConfig = new XmlDocument();
            try
            {
                xmlConfig.Load(strMainConfigPath);
            }
            catch (Exception ex)
            {
                throw new Exceptions.DbException("加载主配置文件 application.config 时失败，错误信息：" + System.Environment.NewLine + ex.Message);
            }

            foreach (XmlNode nodeModule in xmlConfig.DocumentElement.ChildNodes)
            {
                // 循环取模块
                if (nodeModule.Name != "module")
                {
                    continue;
                }

                // 判断该节点是否存在
                if (nodeModule.Attributes == null || nodeModule.Attributes["name"] == null)
                {
                    throw new Exceptions.DbException("application.config配置文件错误！module必须包含name属性。");
                }

                if (nodeModule.Attributes["dbConfigFileName"] == null || string.IsNullOrEmpty(nodeModule.Attributes["dbConfigFileName"].Value))
                {
                    // 如果节点中不包含dbConfigFileName属性，则忽略此节点
                    continue;
                }

                string strModuleName = nodeModule.Attributes["name"].Value;
                string strFileName = nodeModule.Attributes["dbConfigFileName"].Value;
                string strModuleFileName = System.IO.Path.Combine(strAppDomainDirPath, @"config\" + strFileName);

                // 判断模块名是否重复
                if (_ModuleList.ContainsKey(strModuleName))
                {
                    throw new Exceptions.DbException("模块名" + strModuleName + "重复，请检查DataAccess.config配置文件！");
                }

                // 检查数据库配置文件是否存在
                if (!System.IO.File.Exists(strModuleFileName))
                {
                    throw new Exceptions.DbException("数据库配置文件 " + strFileName + " 不存在，请检查配置文件！");
                }

                Module module = new Module();

                XmlDocument xmlModuleConfig = new XmlDocument();
                try
                {
                    xmlModuleConfig.Load(strModuleFileName);
                }
                catch (Exception ex)
                {
                    throw new Exceptions.DbException("加载数据库配置文件 " + strFileName + " 时失败，错误信息：" + System.Environment.NewLine + ex.Message);
                }
                #region 初始化数据库连接串列表
                // 判断配置文件中有没有包含Connections节点，以及Connection子节点，没有就抛出异常。
                if (xmlModuleConfig.DocumentElement.SelectNodes("connectionList/connection").Count == 0)
                {
                    throw new Exceptions.DbException(strFileName + " 配置文件中，没有为模块" + strModuleName + "配置数据库连接！");
                }

                string strConnectionString;
                string strKey;

                foreach (XmlNode nodeConnection in xmlModuleConfig.DocumentElement.SelectNodes("connectionList/connection"))
                {
                    if (nodeConnection.Attributes == null || nodeConnection.Attributes["name"] == null || nodeConnection.Attributes["connectionString"] == null)
                    {
                        throw new Exceptions.DbException("配置文件 " + strFileName + " 错误！connection必须包含name和connectionString属性。");
                    }

                    // 从Xml文件中读取数据库连接列表
                    string strName = nodeConnection.Attributes["name"].Value;

                    // 判断数据库连接是否重复
                    if (module.ConnectionList.ContainsKey(strName))
                    {
                        throw new Exceptions.DbException(strFileName + " 配置文件中，数据库连接 " + strName + " 重复，请检查配置文件！");
                    }

                    Connection dbConnection = new Connection();
                    // 获取连接串的加密密钥
                    strKey = nodeConnection.Attributes["key"] == null ? string.Empty : nodeConnection.Attributes["key"].Value;
                    if (strKey != "")
                    {
                        // 如果配置了密钥，说明连接串是加密的，则再判断是只对密码加密还是整串加密

                        // 获取加密后的密码
                        string strEncryptedPassword = nodeConnection.Attributes["encryptedPassword"] == null ? string.Empty : nodeConnection.Attributes["encryptedPassword"].Value;
                        if (strEncryptedPassword != "")
                        {
                            // 加密后的密码，如果不为空，表示只对密码加密，则解密也只对密码解密
                            strConnectionString = nodeConnection.Attributes["connectionString"].Value + ";Password=" + Decrypt(strEncryptedPassword, strKey);
                        }
                        else
                        {
                            // 如果没有设置加密后的密码，则表示对整个连接串加密
                            strConnectionString = Decrypt(nodeConnection.Attributes["connectionString"].Value, strKey);
                        }
                    }
                    else
                    {
                        // 如果没配置密钥，连接串未加密
                        strConnectionString = nodeConnection.Attributes["connectionString"].Value;
                    }

                    dbConnection.ConnectionString = strConnectionString;

                    string strDataProvider = nodeConnection.Attributes["dataProvider"] == null ? string.Empty : nodeConnection.Attributes["dataProvider"].Value;
                    switch (strDataProvider.ToLower())
                    {
                        case "sqlserver":
                        default:
                            dbConnection.Provider = DataProvider.SqlServer;
                            break;
                        case "sqlserverce":
                            dbConnection.Provider = DataProvider.SqlServerCE;
                            break;
                        case "mysql":
                            dbConnection.Provider = DataProvider.MySql;
                            break;
                        case "oledb":
                            dbConnection.Provider = DataProvider.OleDb;
                            break;
                        case "odbc":
                            dbConnection.Provider = DataProvider.Odbc;
                            break;
                        case "postgresql":
                            dbConnection.Provider = DataProvider.PostgreSQL;
                            break;
                        //case "firebird":
                        //    dbConnection.Provider = DataProvider.Firebird;
                        //    break;
                        //case "oracle":
                        //    dbConnection.Provider = DataProvider.Oracle;
                        //    break;
                        //case "sqlite":
                        //    dbConnection.Provider = DataProvider.SQLite;
                        //    break;
                        //case "berkeleydb":
                        //    dbConnection.Provider = DataProvider.BerkeleyDb;
                        //    break;
                    }

                    // 日志
                    bool errorLog;
                    bool.TryParse(nodeConnection.Attributes["errorLog"] == null ? "false" : nodeConnection.Attributes["errorLog"].Value, out errorLog);
                    string errorLogPath = nodeConnection.Attributes["errorLogPath"] == null ? string.Empty : nodeConnection.Attributes["errorLogPath"].Value;

                    if (string.IsNullOrEmpty(errorLogPath))
                    {
                        // 如果没设置日志路径，则设置为不保存
                        errorLog = false;
                    }
                    try
                    {
                        if (errorLogPath.IndexOf(':') < 0)
                        {
                            // 设置了相对路径
                            errorLogPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, errorLogPath);
                        }
                        if (!System.IO.Directory.Exists(errorLogPath))
                        {
                            // 判断日志目录是否存在，不存在则创建之
                            System.IO.Directory.CreateDirectory(errorLogPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        // 如果上面操作任何一个发生异常，则表示当前连接不记录日志
                        errorLog = false;
                        errorLogPath = "";
                    }

                    dbConnection.ErrorLog = errorLog;
                    dbConnection.ErrorLogPath = errorLogPath;

                    module.ConnectionList.Add(strName, dbConnection);
                }
                #endregion

                #region 初始化逻辑数据库列表
                foreach (XmlNode nodeLogicDb in xmlModuleConfig.DocumentElement.SelectNodes("logicDbList/logicDb"))
                {
                    if (nodeLogicDb.Attributes == null || nodeLogicDb.Attributes["name"] == null || nodeLogicDb.Attributes["defaultConnection"] == null)
                    {
                        throw new Exceptions.DbException(strFileName + " 配置文件错误！logicDb必须包含name和defaultConnection属性。");
                    }

                    string strLogicDbName = nodeLogicDb.Attributes["name"].Value;
                    // 判断逻辑数据库名是否重复
                    if (module.LogicDbList.ContainsKey(strLogicDbName))
                    {
                        throw new Exceptions.DbException("逻辑数据库" + strLogicDbName + "重复，请检查配置文件！");
                    }

                    string defaultConnection = nodeLogicDb.Attributes["defaultConnection"].Value;
                    if (!module.ConnectionList.ContainsKey(defaultConnection))
                    {
                        throw new Exceptions.DbException(strFileName + " 配置文件错误！逻辑数据库 " + strLogicDbName + " 的默认数据库连接 " + defaultConnection + " 在连接列表中不存在，请检查配置文件。");
                    }

                    LogicDb logicDb = new LogicDb();
                    logicDb.DefaultConnection = module.ConnectionList[defaultConnection];

                    // 循环读取逻辑数据库
                    foreach (XmlNode nodeTable in nodeLogicDb.ChildNodes)
                    {
                        //判断该节点是否存在
                        //if (nodeTable.Attributes == null || nodeTable.Attributes["Name"] == null)
                        //{
                        //    continue;
                        //}

                        // 当节点不是table的时候，跳出进行下次循环
                        if (nodeTable != null && !"table".Equals(nodeTable.Name))
                        {
                            continue;
                        }
                        if (nodeTable.Attributes == null || nodeTable.Attributes["name"] == null || string.IsNullOrEmpty(nodeTable.Attributes["name"].Value)
                            || nodeTable.Attributes["connection"] == null || string.IsNullOrEmpty(nodeTable.Attributes["connection"].Value)
                            )
                        {
                            throw new Exceptions.DbException(strFileName + " 配置文件错误！table必须包含name和connection属性。");
                        }

                        // 循环读取逻辑数据库下的表
                        Table table = new Table();

                        string strTableName = nodeTable.Attributes["name"].Value;
                        string strConnection = nodeTable.Attributes["connection"].Value;
                        string strAction = string.Empty;
                        string strTableKey = strTableName;
                        if (nodeTable.Attributes["action"] != null)
                        {
                            strAction = nodeTable.Attributes["action"].Value;
                            strTableKey += "__" + strAction;
                        }

                        if (logicDb.TableList.ContainsKey(strTableKey))
                        {
                            throw new Exceptions.DbException(strFileName + " 配置文件错误！逻辑数据库 " + strLogicDbName + " 的 " + strTableName + " 表的" + strAction + "配置重复，请检查配置文件！");
                        }

                        if (!module.ConnectionList.ContainsKey(strConnection))
                        {
                            throw new Exceptions.DbException(strFileName + " 配置文件错误！逻辑数据库 " + strLogicDbName + " 的 " + strTableName + " 表的" + strAction + "默认数据库连接 " + strConnection + " 在连接列表中不存在，请检查配置文件。");
                        }

                        table.Connection = module.ConnectionList[strConnection];

                        logicDb.TableList.Add(strTableKey, table);
                    }

                    module.LogicDbList.Add(strLogicDbName, logicDb);
                }
                #endregion

                _ModuleList.Add(strModuleName, module);
            }
        }

        /// <summary>
        /// 获取模块中的某个数据库连接
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <param name="dbName">数据库名称</param>
        /// <returns>相应的数据库连接</returns>
        internal Connection GetConnectionString(string moduleName, string dbName)
        {
            if (!_ModuleList.ContainsKey(moduleName))
            {
                throw new Exceptions.DbException("模块名 " + moduleName + " 不存在，请检查配置文件！");
            }
            Module module = _ModuleList[moduleName];

            if (!module.ConnectionList.ContainsKey(dbName))
            {
                throw new Exceptions.DbException("数据库连接" + dbName + "不存在，请检查配置文件！");
            }
            Connection connection = module.ConnectionList[dbName];

            return connection;
        }

        internal Connection GetConnectionString(string moduleName, string logicDbName, string tableName, OperationType action)
        {
            if (!_ModuleList.ContainsKey(moduleName))
            {
                throw new Exceptions.DbException("模块 " + moduleName + " 不存在，请检查配置文件！");
            }
            Module module = _ModuleList[moduleName];

            if (!module.LogicDbList.ContainsKey(logicDbName))
            {
                throw new Exceptions.DbException("逻辑数据库 " + logicDbName + " 不存在，请检查配置文件！");
            }
            LogicDb logicDb = module.LogicDbList[logicDbName];

            // 获取字典中名称为tableName+action的节点
            if (logicDb.TableList.ContainsKey(tableName + "__" + action.ToString()))
            {
                return logicDb.TableList[tableName + "__" + action.ToString()].Connection;
            }
            else
            {
                if (logicDb.TableList.ContainsKey(tableName))
                {
                    // 如果没有指定操作类型的节点，则默认取未设定操作类型的节点
                    return logicDb.TableList[tableName].Connection;
                }
                else
                {
                    return logicDb.DefaultConnection;
                }
            }
        }

        private static Mutex _Mutex = new Mutex();
        private static ConfigReader _DbConfigReader;
        private static System.IO.FileSystemWatcher watcher;

        /// <summary>
        /// 解密连接字符串
        /// </summary>
        /// <param name="source">加密串</param>
        /// <param name="key">密钥</param>
        /// <returns>解密后的连接串</returns>
        private string Decrypt(string source, string key)
        {
            Alogrim alogrim = new Alogrim(Alogrim.SymmProvEnum.TripleDES);
            alogrim.Key = key;
            alogrim.IV = _IV;
            return alogrim.Decrypting(source);
        }

        internal static ConfigReader Create()
        {
            if (_DbConfigReader != null)
                return _DbConfigReader;

            if (watcher == null)
            {
                watcher = new System.IO.FileSystemWatcher();
                watcher.Path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config");
                watcher.Filter = "*.config";
                watcher.Changed += new System.IO.FileSystemEventHandler(watcher_Changed);
                watcher.IncludeSubdirectories = true;
                watcher.EnableRaisingEvents = true;
            }

            _Mutex.WaitOne();
            if (_DbConfigReader == null)
                _DbConfigReader = new ConfigReader();
            _Mutex.ReleaseMutex();

            return _DbConfigReader;
        }

        static void watcher_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            _DbConfigReader = null;
        }

        //public static void Reload()
        //{
        //    _DbConfigReader = null;
        //}
    }
}
