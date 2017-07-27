using ProtoBuf;
using System.Collections.Generic;

namespace Client
{
    [ProtoContract]
    public struct TaskInfo
    {
        [ProtoMember(1)]
        public uint GroupNumber;
        [ProtoMember(2)]
        public string Owner;
        [ProtoMember(3)]
        public byte Loading;
    }

    [ProtoContract]
    public sealed class ActiveTasks
    {
        [ProtoMember(1)]
        public List<TaskInfo> Tasks; 
    }
}
