using ControllerServer.Database;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Linq;

namespace ControllerServer
{
    public struct ClientState
	{
		public Client client;
		public byte[] buffer;
		public int bufferSize;
	}

	public sealed class ControllerServer : IClientEventHandler, IServerEventHandler
	{
		private Dictionary<Socket, ClientState> _clients = new Dictionary<Socket, ClientState>();
        private static volatile ControllerServer _instance;
        private static object _syncRoot = new Object();

        private ControllerServer() { }

        public static ControllerServer Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                            _instance = new ControllerServer();
                    }
                }

                return _instance;
            }
        }

        public void OnClientConnected(Socket client)
		{
			IPEndPoint endPoint = client.RemoteEndPoint as IPEndPoint;

			Console.WriteLine("Incoming connection from {0}:{1}.",
				endPoint.Address, endPoint.Port);
			_clients.Add(client, new ClientState() { client = new Client(client) });
		}

		public void OnClientDisconnected(Socket client)
		{
			IPEndPoint endPoint = client.RemoteEndPoint as IPEndPoint;
            Client current = _clients[client].client;

            if (current.Authenticated)
            {
                Console.WriteLine("Connection closed from {0}:{1} ({2}).",
                    endPoint.Address, endPoint.Port, current.Email);
                current.Logout();
            }
            else
            {
                Console.WriteLine("Connection closed from {0}:{1}.",
                    endPoint.Address, endPoint.Port);
            }

            if (current.Generator != null)
            {
                ServerState.RemoveGenerator(current.Generator);
            }

			_clients.Remove(client);
		}

		public void OnDataReceived(Socket client, byte[] data, int bytesRead)
		{
			ClientState state = _clients[client];

			if (state.buffer == null)
			{
				if (bytesRead > 1024)
				{
					state.buffer = new byte[bytesRead * 2];
				}
				else
				{
					state.buffer = new byte[1024];
				}
			}
			else if (state.buffer.Length < state.bufferSize + bytesRead)
			{
				byte[] newBuffer = new byte[(state.bufferSize + bytesRead) * 2];

				Array.Copy(state.buffer, 0, newBuffer, 0, state.bufferSize);
				state.buffer = newBuffer;
			}

			Array.Copy(data, 0, state.buffer, state.bufferSize, bytesRead);
			state.bufferSize += bytesRead;

			while (state.bufferSize > 3)
			{
				ushort size = BitConverter.ToUInt16(state.buffer, 0);
				ushort opcodeNumber = BitConverter.ToUInt16(state.buffer, 2);

				if (!Enum.IsDefined(typeof(ClientOpcode), (ClientOpcode)opcodeNumber))
				{
					IPEndPoint endPoint = client.RemoteEndPoint as IPEndPoint;

					Console.WriteLine("Received unknown opcode from {0}:{1}.",
						endPoint.Address, endPoint.Port);
					client.Close();
					return;
				}

			    ClientOpcode opcode = (ClientOpcode)opcodeNumber;

				if (state.bufferSize >= size + 4)
				{
					byte[] packet = new byte[size];

					Array.Copy(state.buffer, 4, packet, 0, size);
					PacketHandler.Handle(state, opcode, packet);
					Array.Copy(state.buffer, 4 + size, state.buffer, 0, state.bufferSize - size - 4);
					state.bufferSize -= size + 4;
				}
				else
				{
					break;
				}
			}
		}

        public void Send(Socket client, Packet packet)
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
            ServerOpcode opcode = packet.Opcode;
            byte[] buffer = new byte[data.Length + 4];

            Array.Copy(BitConverter.GetBytes((ushort)data.Length), 0, buffer, 0, 2);
            Array.Copy(BitConverter.GetBytes((ushort)opcode), 0, buffer, 2, 2);
            Array.Copy(data, 0, buffer, 4, data.Length);
            Server.Send(client, buffer, buffer.Length);
        }

		public void OnExceptionOccured(Exception ex)
		{
			Console.WriteLine(ex.ToString());
		}

		public void OnListen(Socket _listener)
		{
			IPEndPoint local = _listener.LocalEndPoint as IPEndPoint;

			Console.WriteLine("Listening IP {0} port {1}", local.Address, local.Port);

            using (var db = new DatabaseContainer())
            {
                var query = from u in db.UsersSet
                            where u.Online == true
                            select u;

                if (query.Count() > 0)
                {
                    foreach (var user in query)
                    {
                        user.Online = false;
                    }

                    db.SaveChanges();
                }
            }
		}

        public void BroadcastToClients(Packet packet)
        {
            ClientState[] clients = new ClientState[_clients.Values.Count];

            _clients.Values.CopyTo(clients, 0);

            foreach (var client in clients)
            {
                Client cl = client.client;
                
                if (cl.Authenticated && cl.Generator == null)
                {
                    Send(cl.Socket, packet);
                }
            }
        }
    }
}
