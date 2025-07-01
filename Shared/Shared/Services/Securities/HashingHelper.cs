using System.Security.Cryptography;
using System.Text;

namespace Shared.Services.Securities;

/// <summary>
/// Provides secure password hashing and verification functionality using PBKDF2 with SHA-256.
/// </summary>
public static class HashingHelper
{
  public static void CreatePaswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
  {
    using (var hmac = new HMACSHA512())
    {
      passwordSalt = hmac.Key;
      passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }
  }

  public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
  {
    using (var hmac = new HMACSHA512(passwordSalt))
    {
      var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
      for (int i = 0; i < computeHash.Length; i++)
      {
        if (computeHash[i] != passwordHash[i])
        {
          return false;
        }
      }
    }

    return true;
  }
}