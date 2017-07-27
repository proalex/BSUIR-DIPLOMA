using ProtoBuf;

namespace ControllerServer
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
