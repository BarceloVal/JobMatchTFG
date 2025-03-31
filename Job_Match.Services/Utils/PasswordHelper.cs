using Job_Match.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Job_Match.Services.Utils
{
    public static class PasswordHelper
    {
        private static readonly PasswordHasher<m_user> _hasher = new();

        public static string Hash(string password)
        {
            return _hasher.HashPassword(new m_user(), password);
        }

        public static bool Verify(string password, string hashedPassword)
        {
            var result = _hasher.VerifyHashedPassword(new m_user(), hashedPassword, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}
