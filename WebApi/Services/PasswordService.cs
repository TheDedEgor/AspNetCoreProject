using System.Security.Cryptography;
using System.Text;

namespace WebApi.Services;

public class PasswordService
{
    public string GetPasswordHash(string password)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        var hashString = new StringBuilder();
        foreach (var x in hash)
        {
            hashString.Append(x.ToString("X2"));
        }

        return hashString.ToString();
    }
}