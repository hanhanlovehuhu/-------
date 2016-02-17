using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace DataAccess.V2.DbConfig
{
    /// <summary>
    /// Alogrim包括了对称和非对称的加解密方法。
    /// 简化了加解密数据的操作。
    /// 同时它支持自定义对称加密算法。
    /// </summary>
    internal class Alogrim
    {
        /// <summary>
        /// 枚举微软所提供的对称加密方法
        /// </summary>
        internal enum SymmProvEnum : int
        {
            DES,		//DES算法
            RC2,		//RC2算法
            Rijndael,	//Rijndael算法
            TripleDES	//TripleDES算法
        }

        //  加密算法的变量
        private SymmetricAlgorithm objCryptoService;

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="NetSelected">初始化的对称加密算法</param>
        internal Alogrim(SymmProvEnum NetSelected)
        {
            switch (NetSelected)
            {
                case SymmProvEnum.DES:
                    // 使用DES算法
                    objCryptoService = new DESCryptoServiceProvider();
                    break;
                case SymmProvEnum.RC2:
                    // 使用RC2算法
                    objCryptoService = new RC2CryptoServiceProvider();
                    break;
                case SymmProvEnum.Rijndael:
                    // 使用Rijndael算法
                    objCryptoService = new RijndaelManaged();
                    break;
                case SymmProvEnum.TripleDES:
                    // 使用TripleDES算法
                    objCryptoService = new TripleDESCryptoServiceProvider();
                    break;
            }
            _Key = objCryptoService.Key;
            _IV = objCryptoService.IV;
        }

        /// <remarks>
        /// 使用指定的对称加密算法提供商提供的加密算法类
        /// Constructor for using a customized SymmetricAlgorithm class.
        /// </remarks>
        internal Alogrim(SymmetricAlgorithm ServiceProvider)
        {
            // 初始化加密对象
            objCryptoService = ServiceProvider;
        }
        #endregion

        #region 向量和密钥
        /// <summary>
        /// DES加密密钥变量
        /// </summary>
        private byte[] _Key;

        /// <summary>
        /// DES加密密钥属性
        /// </summary>
        internal string Key
        {
            get
            {
                return System.Text.Encoding.Default.GetString(_Key);
            }
            set
            {
                string strTemp = value;
                if (strTemp.Length * 8 > objCryptoService.LegalKeySizes[0].MaxSize)
                {
                    // 如果密钥长度太长得话，截
                    strTemp = strTemp.Substring(0, objCryptoService.LegalKeySizes[0].MaxSize / 8);
                }
                if (strTemp.Length * 8 < objCryptoService.LegalKeySizes[0].MinSize)
                {
                    // 如果密钥长度太短得的话，在右边补空格
                    strTemp = strTemp.PadRight(objCryptoService.LegalKeySizes[0].MinSize / 8);
                }
                if (objCryptoService.LegalKeySizes[0].SkipSize != 0 && (strTemp.Length * 8) % objCryptoService.LegalKeySizes[0].SkipSize != 0)
                {
                    // 如果密钥长度不是密钥长度间隔单位的倍数，则补空格。
                    strTemp = strTemp.PadRight(objCryptoService.LegalKeySizes[0].MaxSize / 8);
                }

                _Key = System.Text.Encoding.Default.GetBytes(strTemp);
                objCryptoService.Key = _Key;
            }
        }

        /// <summary>
        /// DES加密向量变量
        /// </summary>
        private byte[] _IV;

        /// <summary>
        /// DES加密向量属性
        /// </summary>
        internal string IV
        {
            get
            {
                return System.Text.Encoding.Default.GetString(_IV);
            }
            set
            {
                string strTemp = value;
                if (value.Length * 8 > objCryptoService.BlockSize)
                {
                    // 如果向量长度大于加密操作块的话，截
                    strTemp = value.Substring(0, objCryptoService.BlockSize / 8);
                }
                if (value.Length * 8 < objCryptoService.BlockSize)
                {
                    // 如果向量长度小于加密操作块的话，在右边补空格
                    strTemp = value.PadRight(objCryptoService.BlockSize / 8);
                }
                _IV = System.Text.Encoding.Default.GetBytes(strTemp);
                objCryptoService.IV = _IV;
            }
        }
        #endregion

        /// <summary>
        /// 使用指定的加密算法、用随机密钥加密字符串
        /// </summary>
        /// <param name="Source">要加密的字符串</param>
        /// <returns>加密结果</returns>
        internal string Encrypting(string Source)
        {
            // 创建内存中的数据流
            System.IO.MemoryStream objMemoryStream = new System.IO.MemoryStream();

            // 创建加密器
            ICryptoTransform objEncryptor = objCryptoService.CreateEncryptor();

            // 创建加密转换文件流的加密流
            CryptoStream objCryptoStream = new CryptoStream(objMemoryStream, objEncryptor, CryptoStreamMode.Write);

            StreamWriter writer = new StreamWriter(objCryptoStream);
            writer.Write(Source);
            writer.Flush();
            objCryptoStream.FlushFinalBlock();

            // 获取输出
            byte[] bytOut = new byte[objMemoryStream.Length];
            objMemoryStream.Position = 0;
            objMemoryStream.Read(bytOut, 0, bytOut.Length);

            string strDest = System.Convert.ToBase64String(bytOut);

            objCryptoService.Clear();
            objMemoryStream.Close();
            writer.Close();

            return strDest;
        }

        /// <summary>
        /// 使用预定的对称加密算法、随机的密钥解密字符串
        /// </summary>
        /// <param name="Source">要解密的字符串</param>
        /// <returns>解密结果</returns>
        internal string Decrypting(string Source)
        {
            byte[] bytIn = Convert.FromBase64String(Source);

            // 创建内存流
            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytIn, 0, bytIn.Length, false, true);

            // 创建加密器
            ICryptoTransform encrypto = objCryptoService.CreateDecryptor();

            // 创建解密转换流的解密流
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);

            // 从解密流读取数据
            System.IO.StreamReader sr = new System.IO.StreamReader(cs);
            return sr.ReadToEnd();
        }


        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="SourceStream">源流</param>
        /// <param name="EncryptorStream">加密流</param>
        internal void Encrypting(Stream SourceStream, Stream EncryptorStream)
        {
            // 创建加密器
            ICryptoTransform objEncryptor = objCryptoService.CreateEncryptor();

            // 创建加密转换文件流的加密流
            CryptoStream objCryptoStream = new CryptoStream(EncryptorStream, objEncryptor, CryptoStreamMode.Write);

            byte[] bytSource = new byte[SourceStream.Length];
            SourceStream.Position = 0;
            SourceStream.Read(bytSource, 0, bytSource.Length);

            objCryptoStream.Write(bytSource, 0, bytSource.Length);
            objCryptoStream.FlushFinalBlock();
        }


        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="EncryptorStream">加密流</param>
        /// <param name="SourceStream">解密流</param>
        /// <returns>布尔值</returns>
        internal void Decrypting(Stream EncryptorStream, Stream SourceStream)
        {
            // 创建加密器
            ICryptoTransform objDecryptor = objCryptoService.CreateDecryptor();

            // 创建加密转换文件流的加密流
            CryptoStream objCryptoStream = new CryptoStream(EncryptorStream, objDecryptor, CryptoStreamMode.Read);

            StreamReader reader = new StreamReader(objCryptoStream);
            StreamWriter writer = new StreamWriter(SourceStream);
            writer.Write(reader.ReadToEnd());
            writer.Flush();
        }
    }
}
