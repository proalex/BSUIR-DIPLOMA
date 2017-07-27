using ProtoBuf;

namespace ControllerServer
{
    [ProtoContract]
    public sealed class AuthSalt
    {
        [ProtoMember(1)]
        public byte[] Salt;
    }
}
