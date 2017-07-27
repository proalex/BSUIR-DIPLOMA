using ProtoBuf;
using System;
using System.IO;

namespace ControllerServer
{
    public static class PacketBuilder 
    {
        public static Packet Build<T>(T data)
        {
            if (!OpcodesBinding.Opcodes.ContainsKey(data.GetType()))
            {
                throw new ArgumentException("Type doesn`t exist in Opcodes");
            }

            MemoryStream stream = new MemoryStream();

            Serializer.Serialize(stream, data);
            return new Packet(OpcodesBinding.Opcodes[data.GetType()], stream.ToArray());
        }
    }
}
