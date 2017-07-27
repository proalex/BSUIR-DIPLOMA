using ProtoBuf;

namespace Generator
{
    [ProtoContract]
    public sealed class RegGenerator
    {
        [ProtoMember(1)]
        public uint VirtualUsers;
    }
}
