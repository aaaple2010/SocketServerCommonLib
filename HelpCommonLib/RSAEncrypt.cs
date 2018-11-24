
using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace HelpCommonLib
{
    /// <summary>
    /// RSA加密 / 解密
    /// </summary>
    public class RSAEncrypt
    {

        #region 密钥
        /// <summary>
        /// 产生公钥和私钥
        /// </summary>
        public static void GetKey()
        {
            //RSA必须是一个对象，产生公钥和私钥
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                using (StreamWriter strwriter = new StreamWriter("PrivateKey.xml"))
                {
                    // ToXmlString中 true 表示同时包含 RSA 公钥和私钥；false 表示仅包含公钥。
                    DataCache.SetCache("PrivateKey", RSA.ToXmlString(true));
                    strwriter.WriteLine(RSA.ToXmlString(true));
                }
                using (StreamWriter strwriter = new StreamWriter("PublicKey.xml"))
                {
                    DataCache.SetCache("PublicKey", RSA.ToXmlString(false));
                    strwriter.WriteLine(RSA.ToXmlString(false));
                }
            }
        }
        #endregion

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Text">字符串</param>
        /// <returns>string</returns>
        public static string Encrypt(string Text)
        {
            return Encrypt(DataCache.GetCache("PublicKey").ToString(), Text);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="sKey">密钥</param>
        /// <param name="Text">字符串</param>
        /// <returns>string</returns>
        public static string Encrypt(string sKey, string Text)
        {
            try
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(sKey);
                byte[] cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(Text), false);
                return Convert.ToBase64String(cipherbytes);
            }
            catch
            {
                throw new Exception("Encrypt Error!");
            }
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="Text">字符串</param>
        /// <returns>string</returns>
        public static string Decrypt(string Text)
        {
            return Decrypt(DataCache.GetCache("PrivateKey").ToString(), Text);
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="privatekey"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string Decrypt(string sKey, string Text)
        {
            try
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                byte[] cipherbytes;
                rsa.FromXmlString(sKey);
                cipherbytes = rsa.Decrypt(Convert.FromBase64String(Text), false);
                return Encoding.UTF8.GetString(cipherbytes);
            }
            catch
            {
                throw new Exception("Decrypt Error!");
            }
        }
    }
}
