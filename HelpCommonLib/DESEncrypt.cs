
using System;
using System.Text;
using System.Security.Cryptography;

namespace HelpCommonLib
{

    /// <summary>
    /// DES加密 / 解密 8密钥
    /// </summary>
    public class DESEncrypt
    {

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Text">字符串</param>
        /// <returns>string</returns>
        public static string Encrypt(string Text)
        {
            return Encrypt(Text, "Amos..Li");
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Text">字符串</param>
        /// <param name="sKey">密钥</param>
        /// <returns>string</returns>
        public static string Encrypt(string Text, string sKey)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray;
                inputByteArray = Encoding.Default.GetBytes(Text);
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                StringBuilder ret = new StringBuilder();
                foreach (byte b in ms.ToArray())
                {
                    ret.AppendFormat("{0:X2}", b);
                }
                return ret.ToString();
            }
            catch
            {
                throw new Exception("Encrypt Error!");
            }
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Text">字符串</param>
        /// <returns>string</returns>
        public static string Decrypt(string Text)
        {
            return Decrypt(Text, "Amos..Li");
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="Text">字符串</param>
        /// <param name="sKey">密钥</param>
        /// <returns>string</returns>
        public static string Decrypt(string Text, string sKey)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                int len;
                len = Text.Length / 2;
                byte[] inputByteArray = new byte[len];
                int x, i;
                for (x = 0; x < len; x++)
                {
                    i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                    inputByteArray[x] = (byte)i;
                }
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Encoding.Default.GetString(ms.ToArray());
            }
            catch
            {
                throw new Exception("Decrypt Error!");
            }
        }

    }
}
