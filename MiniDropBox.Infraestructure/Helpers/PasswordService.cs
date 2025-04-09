using MiniDropBox.Application.Interfaces.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MiniDropBox.Infraestructure.Helpers
{
    public class PasswordService : IPasswordService
    {
        private const int SaltSize = 16; // 128 bits
        private const int KeySize = 32; // 256 bits
        private const int Iterations = 10000;

        public string HashPassword(string password)
        {
            using var rng = RandomNumberGenerator.Create();
            var salt = new byte[SaltSize];
            rng.GetBytes(salt);

            var key = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256).GetBytes(KeySize);
            var hash = new byte[SaltSize + KeySize];
            Buffer.BlockCopy(salt, 0, hash, 0, SaltSize);
            Buffer.BlockCopy(key, 0, hash, SaltSize, KeySize);

            return Convert.ToBase64String(hash);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            var hashBytes = Convert.FromBase64String(hashedPassword);
            var salt = new byte[SaltSize];
            Buffer.BlockCopy(hashBytes, 0, salt, 0, SaltSize);

            var key = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256).GetBytes(KeySize);

            for (int i = 0; i < KeySize; i++)
            {
                if (hashBytes[SaltSize + i] != key[i])
                    return false;
            }
            return true;
        }
    }
}
