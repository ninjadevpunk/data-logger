using Core.Models;
using System.Diagnostics;

namespace Data.Utilities
{
    [Obsolete("Temporary Alpha compatibility. Remove before Beta.")]
    internal static class LegacyPasswordCompatibility
    {
        internal static bool VerifyLegacyPassword(ACCOUNT account, string password)
        {
            try
            {
                string hashedPassword = Extensions.SaltedSHA256Hash(password, account.accountID.ToString());
                return hashedPassword == account.Password;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception near VerifyLegacyPassword(account,password): {ex.Message}");
                return false;
            }
        }
    }
}
