using System;

namespace ControllerServer
{
    public static class GeneratorHandler
    {
        public static void HandleRegistration(Client client, object data)
        {
            if (client.Authenticated)
            {
                return;
            }

            RegGenerator info = (RegGenerator)data;
            Generator generator = new Generator(client.Socket, (int)info.VirtualUsers);

            client.Generator = generator;
            ServerState.AddGenerator(generator);
            Console.WriteLine("Added new generator {0}:{1} (VirtualUsers = {2})", client.IP, client.Port, generator.VirtualUsers);
        }
    }
}
