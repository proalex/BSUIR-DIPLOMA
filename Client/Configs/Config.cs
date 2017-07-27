using System.Configuration;

namespace Client
{
    public static class Config
    {
        public static readonly string Host =
            ConfigurationManager.AppSettings["Host"];

        public static readonly int Port =
            int.Parse(ConfigurationManager.AppSettings["Port"]);

        public static readonly uint DataTimeout =
            uint.Parse(ConfigurationManager.AppSettings["DataTimeout"]);
    }
}
