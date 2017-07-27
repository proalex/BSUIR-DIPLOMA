using ProtoBuf;
using System;
using System.IO;

namespace Client
{
    public delegate void Handler(object data);

    public static class PacketHandler
    {
        public static void Handle(ServerOpcode opcode, byte[] packet)
        {
            Type type = OpcodesBinding.OpcodeTypes[opcode];
            object data = Serializer.Deserialize(type, new MemoryStream(packet));

            DataStorage.UpdateData(type, data);
        }
    }
}
