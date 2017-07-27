using ProtoBuf;

namespace ControllerServer
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
