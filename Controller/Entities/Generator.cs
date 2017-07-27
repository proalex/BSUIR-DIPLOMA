using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Sockets;

namespace ControllerServer
{
    public sealed class Generator
    {
        private Socket _socket;
        private int _currentUsers;
        private Dictionary<GeneratorTask, int> _tasks = new Dictionary<GeneratorTask, int>();
        private ControllerServer _server = ControllerServer.Instance;

        public readonly int VirtualUsers;

        public ReadOnlyDictionary<GeneratorTask, int> Tasks
        {
            get
            {
                return new ReadOnlyDictionary<GeneratorTask, int>(_tasks);
            }
        }

        public int CurrentUsers
        {
            get
            {
                return _currentUsers;
            }
        }

        public Generator(Socket socket, int virtualUsers)
        {
            if (virtualUsers < 1)
            {
                throw new ArgumentException("virtualUsers < 1");
            }

            VirtualUsers = virtualUsers;
            _socket = socket;// ?? throw new NullReferenceException("socket is null");
        }

        public bool AddTask(GeneratorTask task, int users)
        {
            if (users < 1)
            {
                throw new ArgumentException("users < 1");
            }

            if (task == null)
            {
                throw new NullReferenceException("task is null");
            }

            lock (this)
            {
                if (users > VirtualUsers - _currentUsers)
                {
                    return false;
                }

                string[] array = new string[task.URLs.Count];

                task.URLs.CopyTo(array, 0);

                Send(new AddTask()
                {
                    TaskGroup = task.TaskGroup,
                    VirtualUsers = users,
                    Timeout = task.Timeout,
                    RequestDuration = task.RequestDuration,
                    URLs = array
                });

                _tasks.Add(task, users);
                _currentUsers += users;
                ServerState.GeneratorsUpdated.Set();
                return true;
            }
        }

        public bool UpdateTask(GeneratorTask task, int users)
        {
            if (users < 1)
            {
                throw new ArgumentException("users < 1");
            }

            lock (this)
            {
                if (!_tasks.ContainsKey(task))
                {
                    throw new ArgumentException("task doesn`t exist in _tasks");
                }

                int usersDiff = users - _tasks[task];

                if (users + usersDiff > VirtualUsers - _currentUsers)
                {
                    return false;
                }

                Send(new UpdateTask()
                {
                    TaskGroup = task.TaskGroup,
                    VirtualUsers = users
                });

                _tasks[task] = users;
                _currentUsers += usersDiff;
                ServerState.GeneratorsUpdated.Set();
                return true;
            }
        }

        public void RemoveTask(GeneratorTask task)
        {
            lock (this)
            {
                if (!_tasks.ContainsKey(task))
                {
                    throw new ArgumentException("task doesn`t exist in _tasks");
                }

                int activeUsers = _tasks[task];

                Send(new UpdateTask()
                {
                    TaskGroup = task.TaskGroup,
                    VirtualUsers = 0
                });

                _tasks.Remove(task);
                _currentUsers -= activeUsers;
                ServerState.GeneratorsUpdated.Set();
            }
        }

        public void Send<T>(T message)
        {
            if (message == null)
            {
                throw new NullReferenceException("message is null");
            }

            Packet packet = PacketBuilder.Build(message);

            _server.Send(_socket, packet);
        }
    }
}
