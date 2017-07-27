using System;
using System.Threading;
using System.Threading.Tasks;

namespace ControllerServer
{
    class Bootstrap
	{
		static void Main(string[] args)
		{
            Console.WriteLine("Press any key to stop the server.");

            ControllerServer controller = ControllerServer.Instance;
            CancellationTokenSource token = new CancellationTokenSource();
            Server server = new Server(Config.Host, Config.Port, controller, controller);
            Task serverTask = Task.Run(() => server.Run(token));

            Task.Run(() => ClientUpdateNotifier.Notify(serverTask));
            Task.Run(() => TaskDispatcher.Dispatch(serverTask));
            Console.ReadKey();
            token.Cancel();
            Console.WriteLine("Stopping server...");
            serverTask.Wait();
            Console.WriteLine("Done!");
        }
	}
}
