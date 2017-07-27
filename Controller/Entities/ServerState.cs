using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

namespace ControllerServer
{
    public static class ServerState
    {
        private static readonly List<GeneratorTask> _tasks = new List<GeneratorTask>();
        private static readonly List<Generator> _generators = new List<Generator>();
        private static int _infCapacity = 0;
        private static int _infCapacityReserved = 0;
        private static uint _lastTaskGroupNumber = 1;

        public static readonly EventWaitHandle GeneratorsUpdated;
        public static readonly EventWaitHandle TasksUpdated;

        static ServerState()
        {
            GeneratorsUpdated = new EventWaitHandle(false, EventResetMode.AutoReset);
            TasksUpdated = new EventWaitHandle(false, EventResetMode.AutoReset);
        }

        public static ReadOnlyCollection<GeneratorTask> Tasks
        {
            get
            {
                return _tasks.AsReadOnly();
            }
        }

        public static ReadOnlyCollection<Generator> Generators
        {
            get
            {
                return _generators.AsReadOnly();
            }
        }

        public static int InfCapacity
        {
            get
            {
                return _infCapacity;
            }
        }

        public static int InfCapacityReserved
        {
            get
            {
                return _infCapacityReserved;
            }
        }

        public static void RemoveTask(GeneratorTask task)
        {
            if (task == null)
            {
                throw new NullReferenceException("task is null");
            }

            if (!_tasks.Contains(task))
            {
                throw new ArgumentException("_tasks list doesn`t contain task");
            }

            lock (_generators)
            {
                _infCapacityReserved -= task.VirtualUsers;
            }
        }

        public static bool AddTask(TaskData taskData, string owner, out uint taskGroup, Client client)
        {
            if (taskData == null)
            {
                throw new NullReferenceException("taskData is null");
            }

            if (taskData.Duration < 1)
            {
                throw new ArgumentException("Duration < 1");
            }

            if (taskData.RequestDuration < 0)
            {
                throw new ArgumentException("RequestDuration < 0");
            }

            if (taskData.VirtualUsers < 1)
            {
                throw new ArgumentException("VirtualUsers < 1");
            }

            if (taskData._URLs.Count < 1)
            {
                throw new ArgumentException("URLs is empty");
            }

            if (owner == null)
            {
                throw new NullReferenceException("owner is null");
            }

            if (client == null)
            {
                throw new NullReferenceException("client is null");
            }

            lock (_generators)
            {
                if (taskData.VirtualUsers > _infCapacity - _infCapacityReserved)
                {
                    taskGroup = 0;
                    return false;
                }
                else
                {
                    taskGroup = _lastTaskGroupNumber++;
                    _infCapacityReserved += taskData.VirtualUsers;

                    GeneratorTask task = new GeneratorTask(taskData.VirtualUsers, taskData.Timeout, 
                        taskData.RequestDuration, taskData.Duration, taskData.Strategy, taskData._URLs, taskGroup, owner, client);

                    _tasks.Add(task);
                    TasksUpdated.Set();
                    return true;
                }
            }
        }

        public static void AddGenerator(Generator generator)
        {
            if (generator == null)
            {
                throw new NullReferenceException("generator is null");
            }

            lock (_generators)
            {
                _generators.Add(generator);
                _infCapacity += generator.VirtualUsers;
                GeneratorsUpdated.Set();
            }
        }

        public static void RemoveGenerator(Generator generator)
        {
            lock (_generators)
            {
                if (_generators.Contains(generator))
                {
                    if (generator.Tasks != null)
                    {

                        GeneratorTask[] tasks = new GeneratorTask[generator.Tasks.Keys.Count];

                        generator.Tasks.Keys.CopyTo(tasks, 0);

                        foreach (var task in tasks)
                        {
                            task.Abort();
                        }
                    }

                    _generators.Remove(generator);
                    _infCapacity -= generator.VirtualUsers;
                    GeneratorsUpdated.Set();
                }
            }
        }
    }
}
