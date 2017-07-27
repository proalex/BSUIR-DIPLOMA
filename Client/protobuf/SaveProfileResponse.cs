﻿using ProtoBuf;

namespace Client
{
    public enum SaveProfileResult
    {
        OK,
        NAME_EXISTS
    }

    [ProtoContract]
    public sealed class SaveProfileResponse
    {
        [ProtoMember(1)]
        public SaveProfileResult Result;
    }
}
