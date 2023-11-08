using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace Game
{
    public static class EncryptionHelper
    {
        private const string aseKey = "hAC8hM9f36N5Zwbz";
        private const string md5_key = "abdoes9JDKk32kkD";



        public static string AesEncrypt(string plainText,string key)
        {
            try
            {
                byte[] AES_KEY = Encoding.UTF8.GetBytes(key);
                Aes aesAlg = Aes.Create();
                aesAlg.Key = AES_KEY;
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                byte[] encryptedBytes = null;
                using (var msEncrypt = new System.IO.MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new System.IO.StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encryptedBytes = msEncrypt.ToArray();
                    }
                }
                return Convert.ToBase64String(encryptedBytes);
            }
            catch
            {

            }

            return "";
        }

        public static string AesDecrypt(string encryptedText,string key)
        {
            try
            {
                byte[] AES_KEY = Encoding.UTF8.GetBytes(key);
                byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);

                Aes aesAlg = Aes.Create();

                aesAlg.Key = AES_KEY;
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                string plainText = null;
                using (var msDecrypt = new System.IO.MemoryStream(cipherTextBytes))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new System.IO.StreamReader(csDecrypt))
                        {
                            plainText = srDecrypt.ReadToEnd();
                        }
                    }
                }
                return plainText;
            }
            catch
            {

            }

            return "";
        }

        public static string Md5(string plainText)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] dataBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] keyBytes = Encoding.UTF8.GetBytes(md5_key);

                byte[] combinedBytes = new byte[dataBytes.Length + keyBytes.Length];
                dataBytes.CopyTo(combinedBytes, 0);
                keyBytes.CopyTo(combinedBytes, dataBytes.Length);

                byte[] hashBytes = md5.ComputeHash(combinedBytes);

                // Convert the byte array to a hexadecimal string
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}