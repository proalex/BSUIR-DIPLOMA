using ProtoBuf;
using System.ComponentModel;

namespace ControllerServer
{
    [ProtoContract]
    public sealed class AuthResponse
    {
        [ProtoMember(1)]
        public bool Authenticated;
        [ProtoMember(2), DefaultValue(255)]
        public byte AttemptsLeft = 255;
    }
}
