using ProtoBuf;

namespace ControllerServer
{
    [ProtoContract]
    public sealed class DeleteProfileRequest
    {
        [ProtoMember(1)]
        public string Name;
    }
}
