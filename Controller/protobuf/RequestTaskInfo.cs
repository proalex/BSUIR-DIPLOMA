using ProtoBuf;

namespace ControllerServer
{
    [ProtoContract]
    public sealed class RequestTaskInfo
    {
        [ProtoMember(1)]
        public uint TaskGroup;
    }
}
