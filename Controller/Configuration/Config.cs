using System.Configuration;

namespace ControllerServer
{
    public static class Config
    {
        public static readonly string Host =
            ConfigurationManager.AppSettings["Host"];

        public static readonly int Port =
            int.Parse(ConfigurationManager.AppSettings["Port"]);

        public static readonly byte MaxAuthAttempts =
            byte.Parse(ConfigurationManager.AppSettings["MaxAuthAttempts"]);

        public static readonly int TaskUpdateInverval =
            int.Parse(ConfigurationManager.AppSettings["TaskUpdateInverval"]);
    }
}
