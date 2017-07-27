using System;
using System.Collections.Generic;

namespace Generator
{
    public static class TaskHandler
    {
        public static void HandleAddTask(object data)
        {
            AddTask task = (AddTask)data;

            if (task.RequestDuration < 0)
            {
                Console.WriteLine("Task addition with group number {0} was rejected", task.TaskGroup);
                return;
            }

            if (task.Timeout < 0)
            {
                Console.WriteLine("Task addition with group number {0} was rejected", task.TaskGroup);
                return;
            }

            if (task.URLs.Length < 1)
            {
                Console.WriteLine("Task addition with group number {0} was rejected", task.TaskGroup);
                return;
            }

            if (TaskStorage.ContainsTask(task.TaskGroup))
            {
                Console.WriteLine("Task addition with group number {0} was rejected", task.TaskGroup);
                return;
            }

            if (!TaskStorage.AddTaskGroup(new TaskGroup(task.VirtualUsers, task.Timeout,
                task.RequestDuration, new List<string>(task.URLs)), task.TaskGroup))
            {
                Console.WriteLine("Task addition with group number {0} was rejected", task.TaskGroup);
            }

            Console.WriteLine("Task with group number {0} was added", task.TaskGroup);
        }

        public static void HandleUpdateTask(object data)
        {
            UpdateTask task = (UpdateTask)data;

            if (!TaskStorage.ContainsTask(task.TaskGroup) || task.VirtualUsers < 0)
            {
                Console.WriteLine("Task update with group number {0} was rejected", task.TaskGroup);
                return;
            }

            if(!TaskStorage.UpdateTaskGroup(task.TaskGroup, task.VirtualUsers))
            {
                Console.WriteLine("Task update with group number {0} was rejected", task.TaskGroup);
                return;
            }


            if (task.VirtualUsers > 0)
            {
                Console.WriteLine("Task with group number {0} was updated", task.TaskGroup);
            }
            else
            {
                Console.WriteLine("Task with group number {0} was finished", task.TaskGroup);
            }
        }
    }
}
