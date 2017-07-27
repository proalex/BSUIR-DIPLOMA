using ProtoBuf;
using System;
using System.IO;

namespace Generator
{
    public static class PacketHandler
    {
        public static void Handle(ServerOpcode opcode, byte[] packet)
        {
            HandleData handleData = OpcodesBinding.Handlers[opcode];
            Type type = handleData.dataType;
            object data = Serializer.Deserialize(type, new MemoryStream(packet));

            handleData.handler(data);
        }
    }
}
