using ProtoBuf;

namespace Client
{
    [ProtoContract]
    public sealed class RequestTaskInfo
    {
        [ProtoMember(1)]
        public uint TaskGroup;
    }
}
