using System.Threading;

namespace Generator
{
    class Bootstrap
    {
        static void Main(string[] args)
        {
            ControllerClient controllerClient = ControllerClient.Instance;
            TCPClient client = new TCPClient(Config.Host, Config.Port, controllerClient);

            client.Connect();

            do
            {
                Thread.Sleep(100);
            } while (!controllerClient.Connected && controllerClient.LastException == null);

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            controllerClient.SendAsync(client, PacketBuilder.Build(new RegGenerator() { VirtualUsers = Config.VirtualUsers }));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            if (controllerClient.LastException == null)
            {
                do
                {
                    Thread.Sleep(100);
                } while (controllerClient.Connected && controllerClient.LastException == null);
            }
        }
    }
}
