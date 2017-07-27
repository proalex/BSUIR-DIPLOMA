using ProtoBuf;

namespace Client
{
    [ProtoContract]
    public sealed class DeleteProfileRequest
    {
        [ProtoMember(1)]
        public string Name;
    }
}
