using ProtoBuf;

namespace ControllerServer
{
    [ProtoContract]
    public sealed class TaskCreationResult
    {
        [ProtoMember(1)]
        public uint GroupNumber;
    }
}
