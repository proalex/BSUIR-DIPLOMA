using System;

namespace ControllerServer
{
    public sealed class Packet
    {
        private ServerOpcode _opcode;
        private byte[] _data;

        public ServerOpcode Opcode
        {
            get
            {
                return _opcode;
            }
        }

        public byte[] Data
        {
            get
            {
                return _data;
            }
        }

        public Packet(ServerOpcode opcode, byte[] data)
        {
            if (data.Length > UInt16.MaxValue)
            {
                throw new ArgumentException("data.Length > max value");
            }

            _opcode = opcode;
            _data = data;// ?? throw new NullReferenceException("data is null");
        }
    }
}
