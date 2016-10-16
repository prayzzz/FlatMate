using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace FlatMate.Module.Account.Services
{
    public interface IPasswordService
    {
        string CreateSalt();

        string HashPassword(string salt, string password);

        bool VerifyPassword(string salt, string password, string expectedPassword);
    }

    public class PasswordService : IPasswordService
    {
        public string CreateSalt()
        {
            var salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return Convert.ToBase64String(salt);
        }

        public string HashPassword(string salt, string password)
        {
            var saltAsBytes = Convert.FromBase64String(salt);
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(password, saltAsBytes, KeyDerivationPrf.HMACSHA1, 10000, 32));
        }

        public bool VerifyPassword(string salt, string password, string expectedPassword)
        {
            var currentPassword = HashPassword(salt, password);
            return currentPassword == expectedPassword;
        }
    }
}