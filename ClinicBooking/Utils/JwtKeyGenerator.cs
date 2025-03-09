using System.Security.Cryptography;
using System.Text;
namespace ClinicBooking.Utils
{


    public static class JwtKeyGenerator
    {
        public static string GenerateKey()
        {
            var keyBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(keyBytes);
            }
          

            return Convert.ToBase64String(keyBytes);
        }
    }
}

