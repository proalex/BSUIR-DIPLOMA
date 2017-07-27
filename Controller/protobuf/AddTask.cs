using ProtoBuf;
using System.Collections.Generic;

namespace ControllerServer
{
    [ProtoContract]
    public sealed class AddTask
    {
        [ProtoMember(1)]
        public uint TaskGroup;
        [ProtoMember(2)]
        public int VirtualUsers;
        [ProtoMember(3)]
        public int Timeout;
        [ProtoMember(4)]
        public int RequestDuration;
        [ProtoMember(5)]
        public string[] URLs;
    }
}
