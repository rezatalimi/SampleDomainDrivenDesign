using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;
using System.Text.RegularExpressions;

namespace Sample.Commons.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static bool IsComplexPassword(this string value)
        {
            if (value.IsNullOrEmpty()) return false;

            bool hasNumber = Regex.IsMatch(value, @"\d");
            bool hasSpecialChar = Regex.IsMatch(value, @"[!@#$%^&*(),.?""{}|<>]");
            bool hasUpperCase = Regex.IsMatch(value, @"[A-Z]");
            bool hasLowerCase = Regex.IsMatch(value, @"[a-z]");

            return hasNumber && hasSpecialChar && hasUpperCase && hasLowerCase;
        }

        public static bool IsValidToken(this string value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            value = value.Trim();
            if (value.Length > 100 | value.Length < 30) return false;
            return Regex.IsMatch(value, @"^[a-zA-Z0-9-]+$", RegexOptions.None);
        }

        public static string GetHashPassword(this string value, string salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(salt);

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: value,
                salt: bytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
            return hashed;
        }

        public static bool IsHash(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            if (IsPbkdf2Hash(input)) return true;
            if (IsMd5(input)) return true;
            if (IsSha1(input)) return true;
            if (IsSha256(input)) return true;
            if (IsBcrypt(input)) return true;

            return false;
        }

        public static bool IsPbkdf2Hash(string input)
        {
            if (input.Length != 44)
                return false;

            string base64Pattern = @"^[A-Za-z0-9\+/]+={0,2}$";
            if (!Regex.IsMatch(input, base64Pattern))
                return false;

            try
            {
                byte[] decodedBytes = Convert.FromBase64String(input);
                return decodedBytes.Length == 32;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private static bool IsMd5(this string input)
        {
            return input.Length == 32 && Regex.IsMatch(input, @"^[a-fA-F0-9]{32}$");
        }

        private static bool IsSha1(this string input)
        {
            return input.Length == 40 && Regex.IsMatch(input, @"^[a-fA-F0-9]{40}$");
        }

        private static bool IsSha256(this string input)
        {
            return input.Length == 64 && Regex.IsMatch(input, @"^[a-fA-F0-9]{64}$");
        }

        private static bool IsBcrypt(this string input)
        {
            return input.Length == 60 && Regex.IsMatch(input, @"^\$2[ayb]\$\d{1,2}\$[./A-Za-z0-9]{53}$");
        }
    }
}
