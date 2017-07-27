using ProtoBuf;
using System.Collections.Generic;

namespace ControllerServer
{
    [ProtoContract]
    public struct GenData
    {
        [ProtoMember(1)]
        public int Users;
        [ProtoMember(2)]
        public int ActiveUsers;
    }

    [ProtoContract]
    public sealed class ActiveGenerators
    {
        [ProtoMember(1)]
        public List<GenData> Generators;
    }
}
