using ProtoBuf;

namespace Client
{
    [ProtoContract]
    public sealed class TaskCreationResult
    {
        [ProtoMember(1)]
        public uint GroupNumber;
    }
}
