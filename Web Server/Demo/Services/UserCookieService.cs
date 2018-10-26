namespace Demo.Services
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    using Demo.Services.Interfaces;

    internal class UserCookieService : IUserCookieService
    {
        private const string EncryptKey = "E546C8DF278CD5931069B522E695D4F2";

        public string EncryptUsernameCookie(string username)
        {
            byte[] key = Encoding.UTF8.GetBytes(EncryptKey);

            using (Aes encryptionAlgorithm = Aes.Create())
            {
                Debug.Assert(encryptionAlgorithm != null, nameof(encryptionAlgorithm) + " != null");

                using (ICryptoTransform encryptor = encryptionAlgorithm.CreateEncryptor(key, encryptionAlgorithm.IV))
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream encryptionStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter encryptionStreamReader = new StreamWriter(encryptionStream))
                            {
                                encryptionStreamReader.Write(username);
                            }
                        }

                        byte[] initializationVector = encryptionAlgorithm.IV;
                        byte[] decryptedContent = memoryStream.ToArray();

                        byte[] result = new byte[initializationVector.Length + decryptedContent.Length];

                        Buffer.BlockCopy(initializationVector, 0, result, 0, initializationVector.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, initializationVector.Length, decryptedContent.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
        }

        public string DecryptUsernameCookie(string cookieContent)
        {
            byte[] fullCipher = Convert.FromBase64String(cookieContent);

            byte[] initializationVector = new byte[16];
            byte[] cipher = new byte[16];

            Buffer.BlockCopy(fullCipher, 0, initializationVector, 0, initializationVector.Length);
            Buffer.BlockCopy(fullCipher, initializationVector.Length, cipher, 0, initializationVector.Length);

            byte[] key = Encoding.UTF8.GetBytes(EncryptKey);

            using (Aes encryptionAlgorithm = Aes.Create())
            {
                using (ICryptoTransform decryptor = encryptionAlgorithm.CreateDecryptor(key, initializationVector))
                {
                    using (MemoryStream cipherMemoryStream = new MemoryStream(cipher))
                    {
                        using (CryptoStream decryptionStream = new CryptoStream(cipherMemoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader decryptionStreamReader = new StreamReader(decryptionStream))
                            {
                                return decryptionStreamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }
    }
}