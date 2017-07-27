using ProtoBuf;
using System.Collections.Generic;

namespace ControllerServer
{
    [ProtoContract]
    public sealed class TaskInfoClient
    {
        [ProtoMember(1)]
        public List<string> URLs;
        [ProtoMember(2)]
        public int VirtualUsers;
        [ProtoMember(3)]
        public int Timeout;
        [ProtoMember(4)]
        public int RequestDuration;
        [ProtoMember(5)]
        public int Duration;
        [ProtoMember(6)]
        public TestingStrategy Strategy;
        [ProtoMember(7)]
        public List<Point> Points;
        [ProtoMember(8)]
        public TaskState State;
    }
}
