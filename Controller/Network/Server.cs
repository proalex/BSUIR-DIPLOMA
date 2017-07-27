using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ControllerServer
{
	public struct SocketState
	{
		public Socket client;
		public byte[] buffer;
	}

	public sealed class Server
	{
		private Socket _listener;
		private IPEndPoint _localEndPoint;
		private readonly int _port;
		private readonly string _host;
		private ManualResetEvent _acceptDone = new ManualResetEvent(false);
		private IClientEventHandler _clientHandler;
		private IServerEventHandler _severHandler;

		public Server(string host, int port, IClientEventHandler clientHandler, IServerEventHandler severHandler)
		{
            _port = port;
            _host = host;// ?? throw new NullReferenceException("host is null");
            _clientHandler = clientHandler;// ?? throw new NullReferenceException("clientHandler is null");
			_severHandler = severHandler;
			IPAddress ipv4Addresses = Array.Find(
				   Dns.GetHostEntry(string.Empty).AddressList,
				   a => a.AddressFamily == AddressFamily.InterNetwork);
			IPHostEntry ipHostInfo = Dns.GetHostEntry(host);
			_localEndPoint = new IPEndPoint(ipv4Addresses, port);
			_listener = new Socket(AddressFamily.InterNetwork,
				SocketType.Stream, ProtocolType.Tcp);
		}

		public Server(string host, int port, IClientEventHandler clientHandler) : this (host, port, clientHandler, null) { }

		public void Run(CancellationTokenSource cts)
		{
			try
			{
				_listener.Bind(_localEndPoint);
				_listener.Listen(_port);
				_severHandler?.OnListen(_listener);

				while (!cts.IsCancellationRequested)
				{
					bool connectionAccepted = false;

					_acceptDone.Reset();
					_listener.BeginAccept(new AsyncCallback(OnClientConnected), null);

					do
					{
						if (_acceptDone.WaitOne(100))
						{
							connectionAccepted = true;
						}
					} while (!connectionAccepted && !cts.IsCancellationRequested);
				}
			}
			catch (Exception ex)
			{
				_severHandler?.OnExceptionOccured(ex);
			}
			finally
			{
				_listener?.Close();
			}
		}

		public static void Send(Socket client, byte[] data, int size)
		{
            try
            {
                client.BeginSend(data, 0, size, 0, OnDataSent, client);
            }
            catch (SocketException)
            {

            }
		}

		private void OnClientConnected(IAsyncResult result)
		{
			byte[] buffer = new byte[1024];
			Socket client = _listener.EndAccept(result);

			_acceptDone.Set();
			_clientHandler.OnClientConnected(client);

			if (client.Connected)
			{
				SocketState state = new SocketState() { client = client, buffer = buffer };
				client.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(OnDataReceived), state);
			}
			else
			{
				_clientHandler.OnClientDisconnected(client);
			}
		}

		private static void OnDataSent(IAsyncResult result)
		{
			Socket client = (Socket)result.AsyncState;

			try
			{
				client.EndSend(result);
			}
			catch (SocketException)
			{
			}
		}

		private void OnDataReceived(IAsyncResult result)
		{
			SocketState state = (SocketState)result.AsyncState;
			int bytesRead;

			try
			{
				bytesRead = state.client.EndReceive(result);
			}
			catch (SocketException)
			{
				_clientHandler.OnClientDisconnected(state.client);
				return;
			}

			_clientHandler.OnDataReceived(state.client, state.buffer, bytesRead);

			if (state.client.Connected)
			{
				state.client.BeginReceive(state.buffer, 0, state.buffer.Length, 0, new AsyncCallback(OnDataReceived), state);
			}
			else
			{
				_clientHandler.OnClientDisconnected(state.client);
			}
		}
	}
}
