using System;
using System.Security.Cryptography;
using System.Text;

namespace Game
{
    public static class EncryptionHelper
    {
        private const string key = "hAC8hM9f36N5Zwbz";

        public static string AesEncrypt(string plainText)
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
            catch (Exception ex)
            {

            }

            return "";
        }

        public static string AesDecrypt(string encryptedText)
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
            catch (Exception ex)
            {

            }

            return "";
        }
    }
}