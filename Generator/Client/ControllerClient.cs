using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Generator
{
    public sealed class ControllerClient : IClientEventHandler
    {
        private byte[] _buffer;
        private int _bufferSize = 0;
        private bool _connected = false;
        private static volatile ControllerClient _instance;
        private static object _syncRoot = new Object();
        private Exception _lastException;

        public Exception LastException
        {
            get
            {
                return _lastException;
            }
        }

        public bool Connected
        {
            get
            {
                return _connected;
            }
        }

        private ControllerClient() { }

        public static ControllerClient Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                            _instance = new ControllerClient();
                    }
                }

                return _instance;
            }
        }

        public void OnConnected(Socket server)
        {
            IPEndPoint endPoint = server.RemoteEndPoint as IPEndPoint;

            _connected = true;
            Console.WriteLine("Connected to controller server {0}:{1}.",
                endPoint.Address, endPoint.Port);
        }

        public void OnDataReceived(Socket server, byte[] data, int bytesRead)
        {
            if (_buffer == null)
            {
                if (bytesRead > 1024)
                {
                    _buffer = new byte[bytesRead * 2];
                }
                else
                {
                    _buffer = new byte[1024];
                }
            }
            else if (_buffer.Length < _bufferSize + bytesRead)
            {
                byte[] newBuffer = new byte[(_bufferSize + bytesRead) * 2];

                Array.Copy(_buffer, 0, newBuffer, 0, _bufferSize);
                _buffer = newBuffer;
            }

            Array.Copy(data, 0, _buffer, _bufferSize, bytesRead);
            _bufferSize += bytesRead;

            while (_bufferSize > 3)
            {
                ushort size = BitConverter.ToUInt16(_buffer, 0);
                ushort opcodeNumber = BitConverter.ToUInt16(_buffer, 2);

                if (!Enum.IsDefined(typeof(ServerOpcode), (ServerOpcode)opcodeNumber))
                {
                    server.Close();
                    return;
                }

                ServerOpcode opcode = (ServerOpcode)opcodeNumber;

                if (_bufferSize >= size + 4)
                {
                    byte[] packet = new byte[size];

                    Array.Copy(_buffer, 4, packet, 0, size);
                    PacketHandler.Handle(opcode, packet);
                    Array.Copy(_buffer, 4 + size, _buffer, 0, _bufferSize - size - 4);
                    _bufferSize -= size + 4;
                }
                else
                {
                    break;
                }
            }
        }

        public async Task SendAsync(TCPClient client, Packet packet)
        {
            if (packet == null)
            {
                throw new NullReferenceException("packet is null");
            }
            if (client == null)
            {
                throw new NullReferenceException("client is null");
            }

            byte[] data = packet.Data;
            ClientOpcode opcode = packet.Opcode;
            byte[] buffer = new byte[data.Length + 4];

            Array.Copy(BitConverter.GetBytes((ushort)data.Length), 0, buffer, 0, 2);
            Array.Copy(BitConverter.GetBytes((ushort)opcode), 0, buffer, 2, 2);
            Array.Copy(data, 0, buffer, 4, data.Length);
            await client.SendAsync(buffer, buffer.Length);
        }

        public void OnDisconnected(Socket server)
        {
            IPEndPoint endPoint = server.RemoteEndPoint as IPEndPoint;

            _connected = false;
            Console.WriteLine("Disconnected from controller server {0}:{1}.",
               endPoint.Address, endPoint.Port);
        }

        public void OnExceptionOccured(Exception ex)
        {
            _lastException = ex;
            Console.WriteLine(ex.Message);
        }
    }
}
