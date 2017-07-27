using ProtoBuf;
using System;
using System.IO;

namespace ControllerServer
{
    public static class PacketHandler
	{
		public static void Handle(ClientState state, ClientOpcode opcode, byte[] packet)
		{
            object data = null;
            
            if (OpcodesBinding.Handlers[opcode].dataType != null)
            {
                data = Serializer.Deserialize(OpcodesBinding.Handlers[opcode].dataType, new MemoryStream(packet));
            }

            OpcodesBinding.Handlers[opcode].handler(state.client, data);
		}
	}
}
