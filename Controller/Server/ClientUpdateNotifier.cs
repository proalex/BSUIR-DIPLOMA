using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ControllerServer
{
    public static class ClientUpdateNotifier
    {
        public static void Notify(Task serverTask)
        {
            ControllerServer controller = ControllerServer.Instance;
            EventWaitHandle[] waiters = new EventWaitHandle[2];

            waiters[0] = ServerState.GeneratorsUpdated;
            waiters[1] = ServerState.TasksUpdated;

            while (!serverTask.IsCompleted)
            {
                int operation = WaitHandle.WaitAny(waiters);

                if (operation == 0)
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

                    controller.BroadcastToClients(PacketBuilder.Build(new ActiveGenerators()
                    {
                        Generators = generators
                    }));
                }
                else if (operation == 1)
                {
                    List<TaskInfo> tasks = new List<TaskInfo>();

                    foreach (var task in ServerState.Tasks)
                    {
                        if (task.State == TaskState.RUNNING || task.State == TaskState.CREATED)
                        {
                            tasks.Add(new TaskInfo()
                            {
                                GroupNumber = task.TaskGroup,
                                Owner = task.Owner,
                                Loading = (byte)(((double)task.VirtualUsers / ServerState.InfCapacity) * 100)
                            });
                        }
                    }

                    controller.BroadcastToClients(PacketBuilder.Build(new ActiveTasks()
                    {
                        Tasks = tasks
                    }));
                }
            }
        }
    }
}
