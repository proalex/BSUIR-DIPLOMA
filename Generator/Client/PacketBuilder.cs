using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;

namespace Generator
{
    public static class PacketBuilder 
    {
        static PacketBuilder()
        {
        }

        public static Packet Build<T>(T data)
        {
            var opcodes = OpcodesBinding.Opcodes;

            if (!opcodes.ContainsKey(data.GetType()))
            {
                throw new ArgumentException("Type doesn`t exist in _opcodes");
            }

            MemoryStream stream = new MemoryStream();

            Serializer.Serialize(stream, data);
            return new Packet(opcodes[data.GetType()], stream.ToArray());
        }
    }
}
