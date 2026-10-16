using Core.Enums;
using Core.Models;
using System.Security.Cryptography;
using System.Text;

namespace Data.Utilities
{
    internal class Extensions
    {
        private const int Pbkdf2Iterations = 600_000;
        private const int MinimumAcceptedIterations = 600_000;
        private const int MaximumAcceptedIterations = 2_000_000;
        private const int SaltSize = 16;
        private const int HashSize = 32;

        internal static LOG.CATEGORY GetLogCategory(LogContext context)
        {
            return context switch
            {
                LogContext.Qt or LogContext.AndroidStudio => LOG.CATEGORY.CODING,
                LogContext.NOTES => LOG.CATEGORY.NOTES,
                _ => LOG.CATEGORY.CODING,
            };
        }

        internal static string SaltedSHA256Hash(string value, string accountID)
        {
            Encoding enc = Encoding.UTF8;

            string input = value + accountID;
            byte[] result = SHA256.HashData(enc.GetBytes(input));

            StringBuilder hashedStringBuilder = new StringBuilder();
            foreach (byte b in result)
            {
                hashedStringBuilder.Append(b.ToString("x2"));
            }

            return hashedStringBuilder.ToString();
        }


        internal static string HashPassword(string password)
        {
            ArgumentNullException.ThrowIfNull(password);

            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Pbkdf2Iterations, HashAlgorithmName.SHA256, HashSize);

            return $"pbkdf2-sha256${Pbkdf2Iterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
        }

        internal static bool VerifyPassword(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(storedHash))
                return false;

            string[] parts = storedHash.Split('$');

            if (parts.Length != 4 || parts[0] != "pbkdf2-sha256")
                return false;

            if (!int.TryParse(parts[1], out int iterations) || iterations < MinimumAcceptedIterations || iterations > MaximumAcceptedIterations)
                return false;

            try
            {
                byte[] salt = Convert.FromBase64String(parts[2]);
                byte[] expectedHash = Convert.FromBase64String(parts[3]);

                if (salt.Length != SaltSize || expectedHash.Length != HashSize)
                    return false;

                byte[] actualHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, HashSize);

                return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
