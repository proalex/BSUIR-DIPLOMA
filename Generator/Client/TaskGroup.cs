using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Generator
{
    public class TaskGroup
    {
        private int _virtualUsers;
        private bool _activate;
        private readonly List<string> _URLs = new List<string>();
        private readonly List<CancellationTokenSource> _tokens = new List<CancellationTokenSource>();

        public readonly int Timeout;
        public readonly int RequestDuration;

        public ReadOnlyCollection<string> URLs
        {
            get
            {
                return _URLs.AsReadOnly();
            }
        }

        public int VirtualUsers
        {
            get
            {
                return _virtualUsers;
            }
            set
            {
                lock (this)
                {
                    if (!_activate)
                    {
                        _virtualUsers = value;
                        return;
                    }

                    if (value < 0)
                    {
                        throw new ArgumentException("VirtualUsers property can`t be lower than zero");
                    }
                    else if (_virtualUsers > value)
                    {
                        int diff = _virtualUsers - value;

                        for (int i = _virtualUsers - 1; i > _virtualUsers - diff - 1; i--)
                        {
                            _tokens[i].Cancel();
                        }

                        _tokens.RemoveRange(_virtualUsers - diff, diff);
                        _virtualUsers = value;
                    }
                    else if (_virtualUsers < value)
                    {
                        int diff = value - _virtualUsers;

                        for (int i = _virtualUsers; i < value; i++)
                        {
                            _tokens.Add(new CancellationTokenSource());
                            Task.Run(() => TaskPerformer.Perform(_tokens[i], _URLs.ToArray(), Timeout, RequestDuration));
                        }

                        _virtualUsers = value;
                    }
                }
            }
        }

        public TaskGroup(int virtualUsers, int timeout, int requestDuration, List<string> URLs)
        {
            if (virtualUsers < 1)
            {
                throw new ArgumentException("virtualUsers < 1");
            }
            if (timeout == 0)
            {
                throw new ArgumentException("timeout is zero");
            }

            _virtualUsers = virtualUsers;
            Timeout = timeout;
            _URLs = URLs;// ?? throw new NullReferenceException("URLs is null");
        }

        public void Activate()
        {
            lock (this)
            {
                if (_activate)
                {
                    return;
                }

                for (int i = 0; i < _virtualUsers; i++)
                {
                    _tokens.Add(new CancellationTokenSource());
                    Task.Run(() => TaskPerformer.Perform(_tokens[i], _URLs.ToArray(), Timeout, RequestDuration));
                }

                _activate = true;
            }
        }
    }
}
