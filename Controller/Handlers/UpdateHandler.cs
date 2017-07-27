using System.Collections.Generic;

namespace ControllerServer
{
    public static class UpdateHandler
    {
        public static void UpdateInfo(Client client)
        {
            List<GenData> generators = new List<GenData>();

            foreach (var generator in ServerState.Generators)
            {
                generators.Add(new GenData
                {
                    Users = generator.VirtualUsers,
                    ActiveUsers = generator.CurrentUsers
                });
            }

            client.Send(new ActiveGenerators()
            {
                Generators = generators
            });
        }
    }
}
