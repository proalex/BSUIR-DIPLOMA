using ProtoBuf;
using System.Collections.Generic;

namespace Client
{
    [ProtoContract]
    public sealed class Profiles
    {
        [ProtoMember(1)]
        public List<SaveProfileRequest> List;
    }
}
