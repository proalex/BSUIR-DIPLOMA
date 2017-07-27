using ProtoBuf;

namespace Client
{
    [ProtoContract]
    public sealed class AuthData
    {
        [ProtoMember(1)]
        public string Email;

        [ProtoMember(2)]
        public byte[] Token;
    }
}
