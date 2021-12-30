using System;
using System.Security.Cryptography;
using System.Text;

namespace JWTWebToken.Server.Utilities
{
    public class Utility
    {
        public static string MaakHash(string password)
        {
            var provider = MD5.Create();
            string prefix = "X0r8S@vx0aPmra";
            byte[] bytes = provider.ComputeHash(Encoding.UTF32.GetBytes(prefix + password));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}
