using System.Collections.Generic;
using System.Linq;

namespace ControllerServer
{
    public static class TaskHandler
    {
        public static void HandleAddTask(Client client, object data)
        {
            if (!client.Authenticated)
            {
                return;
            }

            TaskData task = (TaskData)data;
            uint groupNumber;

            ServerState.AddTask(task, client.Email, out groupNumber, client);

            client.Send(new TaskCreationResult()
            {
                GroupNumber = groupNumber
            });
        }

        public static void HandleTaskInfo(Client client, object data)
        {
            if (!client.Authenticated)
            {
                return;
            }

            RequestTaskInfo info = (RequestTaskInfo)data;
            GeneratorTask task = ServerState.Tasks.ElementAtOrDefault((int)info.TaskGroup - 1);

            if (task == null)
            {
                return;
            }

            if (task.Owner != client.Email)
            {
                return;
            }

            string[] URLs = new string[task.URLs.Count];
            Point[] points = new Point[task.Points.Count];

            task.URLs.CopyTo(URLs, 0);
            task.Points.CopyTo(points, 0);

            client.Send(new TaskInfoClient()
            {
                URLs = new List<string>(URLs),
                VirtualUsers = task.VirtualUsers,
                Timeout = task.Timeout,
                RequestDuration = task.RequestDuration,
                Duration = task.Duration,
                Strategy = task.Strategy,
                Points = new List<Point>(points),
                State = task.State
            });
        }
    }
}
