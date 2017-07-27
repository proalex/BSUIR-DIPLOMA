using ProtoBuf;

namespace Client
{
    [ProtoContract]
    public sealed class SaveProfileRequest
    {
        [ProtoMember(1)]
        public string Name;
        [ProtoMember(2)]
        public TaskData Data;
    }
}
