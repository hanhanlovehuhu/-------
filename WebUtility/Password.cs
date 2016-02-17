using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;
using System.Web.Security;

namespace WebUtility
{
    public class Password
    {
        public static string EncryptMd5(string password)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(password, FormsAuthPasswordFormat.MD5.ToString());
        }

        public static string EncryptSha1(string passwrod)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(passwrod, FormsAuthPasswordFormat.SHA1.ToString());
        }
        public static string GetMD5(string s, string _input_charset)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(s));
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }
    }
}
