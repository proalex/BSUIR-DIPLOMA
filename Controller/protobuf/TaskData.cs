using ProtoBuf;
using System.Collections.Generic;

namespace ControllerServer
{
    public enum TestingStrategy
    {
        INCREASING,
        DECREASING,
    }

    [ProtoContract]
    public sealed class TaskData
    {
        [ProtoMember(1)]
        public List<string> _URLs;
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
    }
}
