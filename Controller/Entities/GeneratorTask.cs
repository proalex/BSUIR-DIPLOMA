using ControllerServer.Database;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace ControllerServer
{
    [ProtoContract]
    public struct Point
    {
        [ProtoMember(1)]
        public int x;
        [ProtoMember(2)]
        public int y;
    }

    public enum TaskState
    {
        CREATED,
        RUNNING,
        FINISHED,
        ABORTED
    }

    public sealed class GeneratorTask
    {
        private TaskState _state;
        private double _currentUsers;
        private double _deltaUsers;
        private readonly List<string> _URLs;
        private readonly List<Point> _points = new List<Point>();
        private Stopwatch _watch = new Stopwatch();
        private int _updateCount;
        private Client _client;

        public readonly int VirtualUsers;
        public readonly int Timeout;
        public readonly int RequestDuration;
        public readonly int Duration;
        public readonly uint TaskGroup;
        public readonly TestingStrategy Strategy;
        public readonly string Owner;
        
        public ReadOnlyCollection<string> URLs
        {
            get
            {
                return _URLs.AsReadOnly();
            }
        }

        public ReadOnlyCollection<Point> Points
        {
            get
            {
                return _points.AsReadOnly();
            }
        }

        public TaskState State
        {
            get
            {
                return _state;
            }
        }

        public long RunningTime
        {
            get
            {
                if (_watch == null)
                {
                    return 0;
                }
                else
                {
                    return _watch.ElapsedMilliseconds;
                }
            }
        }

        public int UpdateCount
        {
            get
            {
                return _updateCount;
            }
        }

        public GeneratorTask(int virtualUsers, int timeout, int requestDuration, 
            int duration, TestingStrategy strategy, List<string> URLs, uint taskGroup, string owner, Client client)
        {
            _URLs = URLs;// ?? throw new NullReferenceException("URLs is null");

            if (_URLs.Count == 0)
            {
                throw new ArgumentException("URLs is empty");
            }

            VirtualUsers = virtualUsers;
            Timeout = timeout;
            RequestDuration = requestDuration;
            Duration = duration;
            Strategy = strategy;
            TaskGroup = taskGroup;
            Owner = owner;// ?? throw new NullReferenceException("owner is null");
            _client = client;// ?? throw new NullReferenceException("client is null");
        }

        public void Start()
        {
            lock (this)
            {
                if (State == TaskState.CREATED)
                {
                    int result = 0;

                    try
                    {
                        result = (int)RequestMaker.MakeWebRequest(Timeout, URLs[0]);
                    }
                    catch (Exception)
                    {
                        result = -1;
                    }

                    if (result == -1)
                    {
                        Console.WriteLine("Task with groupNumber {0} was aborted (web-server hasn`t responded in time)");
                        _state = TaskState.ABORTED;
                        ServerState.RemoveTask(this);
                        ServerState.TasksUpdated.Set();
                        return;
                    }

                    _state = TaskState.RUNNING;

                    switch (Strategy)
                    {
                        case TestingStrategy.INCREASING:
                            _deltaUsers = (double)VirtualUsers / (Duration * 60 / Config.TaskUpdateInverval);
                            _currentUsers += _deltaUsers;
                            break;
                        case TestingStrategy.DECREASING:
                            _deltaUsers = -(double)VirtualUsers / (Duration * 60 / Config.TaskUpdateInverval);
                            _currentUsers = VirtualUsers;
                            break;
                    }

                    TaskDistributor.Distribute(this, (int)_currentUsers);
                    _watch.Start();
                }
            }
        }

        private void SaveResult()
        {
            using (var db = new DatabaseContainer())
            {
                int max = db.UrlsSet.Count() > 0 ? db.UrlsSet.Max(u => u.Group) : 0;
                var queryId = from u in db.UsersSet
                             where u.Email == Owner
                             select u.Id;

                if (queryId.Count() != 1)
                {
                    return;
                }

                int userId = queryId.First();

                foreach (var url in _URLs)
                {
                    db.UrlsSet.Add(new Urls()
                    {
                        Group = max + 1,
                        Url = url
                    });
                }

                Database.Reports report = new Database.Reports()
                {
                    Duration = Duration,
                    RequestDuration = RequestDuration,
                    Strategy = (byte)Strategy,
                    Time = DateTime.Now,
                    Timeout = Timeout,
                    UrlGroup = max + 1,
                    UserId = userId,
                    VirtualUsers = VirtualUsers
                };

                db.ReportsSet.Add(report);

                db.SaveChanges();

                foreach (var point in Points)
                {
                    db.PointsSet.Add(new Database.Points()
                    {
                        ReportId = report.Id,
                        X = point.x,
                        Y = point.y
                    });
                }

                db.SaveChanges();
            }
        }

        public void Abort()
        {
            lock(this)
            {
                _state = TaskState.ABORTED;
                TaskDistributor.Distribute(this, 0);
                ServerState.RemoveTask(this);
                ServerState.TasksUpdated.Set();
                Console.WriteLine("Task with groupNumber {0} was aborted (generator disconnected).", TaskGroup);
            }
        }

        public void Update()
        {
            lock (this)
            {
                if (_state == TaskState.RUNNING && RunningTime >= Config.TaskUpdateInverval * 1000 * _updateCount)
                {
                    _updateCount++;

                    if ((int)_currentUsers != (int)(_currentUsers + _deltaUsers))
                    {
                        long result;

                        try
                        {
                            result = RequestMaker.MakeWebRequest(Timeout, URLs[0]);
                        }
                        catch (Exception)
                        {
                            result = -1;
                        }

                        Console.WriteLine("Task with groupNumber {0} was updated (latency {1}).", TaskGroup, result);

                        _points.Add(new Point()
                        {
                            x = (int)_currentUsers,
                            y = (int)result
                        });
                    }

                    if (RunningTime >= Duration * 60 * 1000)
                    {
                        TaskDistributor.Distribute(this, 0);
                        _currentUsers = 0;
                        _state = TaskState.FINISHED;
                        Console.WriteLine("Task with groupNumber {0} has finished.", TaskGroup);
                        ServerState.TasksUpdated.Set();
                        ServerState.RemoveTask(this);
                        SaveResult();
                        ReportsHandler.SendReports(_client);
                    }
                    else
                    {
                        _currentUsers += _deltaUsers;
                        TaskDistributor.Distribute(this, (int)_currentUsers);
                    }
                }
            }
        }
    }
}
