namespace Torshia.Services
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    using Torshia.Services.Interfaces;

    internal class HashService : IHashService
    {
        public string Hash(string value)
        {
            using (SHA256 hasher = SHA256.Create())
            {
                return BitConverter.ToString(hasher.ComputeHash(Encoding.UTF8.GetBytes(value))).Replace("-", string.Empty);
            }
        }
    }
}