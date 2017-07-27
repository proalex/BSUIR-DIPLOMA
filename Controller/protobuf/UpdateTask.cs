using ProtoBuf;

namespace ControllerServer
{
    [ProtoContract]
    public sealed class UpdateTask
    {
        [ProtoMember(1)]
        public uint TaskGroup;
        [ProtoMember(2)]
        public int VirtualUsers;
    }
}
