using System;
using System.Collections.Generic;

namespace Generator
{
    public static class TaskStorage
    {
        private static Dictionary<uint, TaskGroup> _taskGroups = new Dictionary<uint, TaskGroup>();
        private static int _currentLoad;
        
        public static bool AddTaskGroup(TaskGroup group, uint groupNumber)
        {
            if (group == null)
            {
                throw new NullReferenceException("group is null");
            }

            if (_taskGroups.ContainsKey(groupNumber))
            {
                throw new ArgumentException("Incorrect groupNumber");
            }

            if (_currentLoad + group.VirtualUsers > Config.VirtualUsers)
            {
                return false;
            }

            lock (_taskGroups)
            {
                _taskGroups[groupNumber] = group;
                _currentLoad += group.VirtualUsers;
            }

            group.Activate();
            return true;
        }

        public static bool UpdateTaskGroup(uint groupNumber, int users)
        {
            if (users < 0)
            {
                throw new ArgumentException("users < 0");
            }

            if (!_taskGroups.ContainsKey(groupNumber))
            {
                throw new ArgumentException("Incorrect groupNumber");
            }

            var group = _taskGroups[groupNumber];

            if (_currentLoad + users - group.VirtualUsers > Config.VirtualUsers)
            {
                return false;
            }

            _currentLoad += users - group.VirtualUsers;
            group.VirtualUsers = users;

            if (users == 0)
            {
                _taskGroups.Remove(groupNumber);
            }

            return true;
        }

        public static bool ContainsTask(uint groupNumber)
        {
            return _taskGroups.ContainsKey(groupNumber);
        }
    }
}
