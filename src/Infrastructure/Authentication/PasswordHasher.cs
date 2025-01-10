using System.Security.Cryptography;
using System.Text;
using Domain.Authentication;

namespace Infrastructure.Authentication;

public sealed class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        
        byte[] hashBytes = SHA256.HashData(passwordBytes);
        
        return Convert.ToHexString(hashBytes);
    }
}
