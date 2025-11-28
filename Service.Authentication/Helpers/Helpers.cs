using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace Service.Authorization;

public static class Helpers
{
    public static string RandomString(int len = 32)
    {
        var b = RandomNumberGenerator.GetBytes(len);
        return WebEncoders.Base64UrlEncode(b);
    }

    public static bool VerifyPkce(string storedChallenge, string? method, string verifier)
    {
        if (string.IsNullOrEmpty(storedChallenge)) return false;
        if (method?.ToUpperInvariant() == "S256")
        {
            using var sha = SHA256.Create();
            var vBytes = Encoding.ASCII.GetBytes(verifier);
            var hash = sha.ComputeHash(vBytes);
            var calc = WebEncoders.Base64UrlEncode(hash);
            return calc == storedChallenge;
        }

        return verifier == storedChallenge;
    }   
}