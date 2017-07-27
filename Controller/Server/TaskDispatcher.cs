using System.Threading;
using System.Threading.Tasks;

namespace ControllerServer
{
    public static class TaskDispatcher
    {
        public static void Dispatch(Task serverTask)
        {
            while (!serverTask.IsCompleted)
            {
                long minSleep = Config.TaskUpdateInverval * 1000;

                foreach (var task in ServerState.Tasks)
                {
                    if (task.State == TaskState.CREATED)
                    {
                        Task.Run(() => task.Start());
                    }
                    else if (task.State == TaskState.RUNNING && task.RunningTime != 0 && 
                        task.RunningTime > task.UpdateCount * Config.TaskUpdateInverval * 1000)
                    {
                        Task.Run(() => task.Update());
                    }
                    else if (task.State == TaskState.RUNNING && task.RunningTime != 0)
                    {
                        long tillUpdate = task.UpdateCount * Config.TaskUpdateInverval * 1000 - task.RunningTime;

                        if (tillUpdate < minSleep)
                        {
                            minSleep = tillUpdate;
                        }
                    }
                }

                Thread.Sleep((int)minSleep);
            }
        }
    }
}
