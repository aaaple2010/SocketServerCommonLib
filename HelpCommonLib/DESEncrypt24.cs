
using System;
using System.Text;
using System.Security.Cryptography;


namespace HelpCommonLib
{
    /// <summary>
    /// DES加密 / 解密 24密钥
    /// </summary>
    public class DESEncrypt24
    {

        public static string Encrypt(string Text)
        {
            return Encrypt(Text, "wwwwwwwwwww.Amos..Li.com");
        }

        public static string Encrypt(string a_strString, string a_strKey)
        {
            TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(a_strKey);
            DES.Mode = CipherMode.ECB;
            ICryptoTransform DESEncrypt = DES.CreateEncryptor();
            byte[] Buffer = ASCIIEncoding.ASCII.GetBytes(a_strString);
            return Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
        }

        public static string Decrypt(string Text)
        {
            return Decrypt(Text, "wwwwwwwwwww.Amos..Li.com");
        }

        public static string Decrypt(string a_strString, string a_strKey)
        {
            TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(a_strKey);
            DES.Mode = CipherMode.ECB;
            DES.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
            ICryptoTransform DESDecrypt = DES.CreateDecryptor();
            string result = "";
            try
            {
                byte[] Buffer = Convert.FromBase64String(a_strString);
                result = ASCIIEncoding.ASCII.GetString(DESDecrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
            }
            catch
            {

            }
            return result;
        }
    }
}
