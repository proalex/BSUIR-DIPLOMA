using System;

namespace ControllerServer
{
    public static class SaltGenerator
    {
        private static Random random = new Random();

        public static byte[] GenerateSalt()
        {
            byte[] salt = new byte[512];

            for (int i = 0; i < 512; i++)
            {
                salt[i] = (byte)random.Next(256);
            }

            return salt;
        }
    }
}
