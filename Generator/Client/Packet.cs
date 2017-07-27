using System;

namespace Generator
{
    public sealed class Packet
    {
        private ClientOpcode _opcode;
        private byte[] _data;

        public ClientOpcode Opcode
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

        public Packet(ClientOpcode opcode, byte[] data)
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
