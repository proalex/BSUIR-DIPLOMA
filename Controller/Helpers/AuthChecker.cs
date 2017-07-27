using System;
using System.Security.Cryptography;
using System.Linq;

namespace ControllerServer
{
    public static class AuthChecker
    {
        private static SHA512 hasher = SHA512.Create();
        public static bool CheckToken(byte[] token, byte[] salt, string passwordStr)
        {
            if (token == null)
            {
                throw new NullReferenceException("authToken is null");
            }
            if (salt == null)
            {
                throw new NullReferenceException("salt is null");
            }
            if (passwordStr == null)
            {
                throw new NullReferenceException("password is null");
            }

            byte[] password = Converter.StringToByteArray(passwordStr);
            byte[] concat = new byte[password.Length + salt.Length];

            password.CopyTo(concat, 0);
            salt.CopyTo(concat, password.Length);
            return hasher.ComputeHash(concat).SequenceEqual(token);
        }
    }
}
