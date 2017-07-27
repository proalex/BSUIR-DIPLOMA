using System.Configuration;

namespace Generator
{
    public static class Config
    {
        public static readonly string Host =
            ConfigurationManager.AppSettings["Host"];

        public static readonly int Port =
            int.Parse(ConfigurationManager.AppSettings["Port"]);

        public static readonly uint VirtualUsers =
            uint.Parse(ConfigurationManager.AppSettings["VirtualUsers"]);
    }
}
