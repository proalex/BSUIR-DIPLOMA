using ProtoBuf;

namespace ControllerServer
{
    [ProtoContract]
    public sealed class RegGenerator
    {
        [ProtoMember(1)]
        public uint VirtualUsers;
    }
}
