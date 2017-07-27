using ProtoBuf;

namespace Client
{
    [ProtoContract]
    public sealed class AuthSalt
    {
        [ProtoMember(1)]
        public byte[] Salt;
    }
}
