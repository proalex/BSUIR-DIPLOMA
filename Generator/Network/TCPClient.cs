using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Generator
{
    public sealed class TCPClient
    {
        private Socket _server;
        private IPEndPoint _remoteEp;
        private IClientEventHandler _eventHandler;

        public TCPClient(string host, int port, IClientEventHandler eventHandler)
        {
            IPAddress ipv4Addresses = Array.Find(
                    Dns.GetHostEntry(string.Empty).AddressList,
                    a => a.AddressFamily == AddressFamily.InterNetwork);
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Config.Host);
            _remoteEp = new IPEndPoint(ipv4Addresses, Config.Port);
            _eventHandler = eventHandler;
        }

        public void Connect()
        {
            try
            {
                _server = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);
                _server.BeginConnect(_remoteEp, new AsyncCallback(OnConnected), null);
            }
            catch (Exception ex)
            {
                _eventHandler.OnExceptionOccured(ex);
            }
        }

        public void Disconnect()
        {
            if (_server.Connected)
            {
                _server.Close();
            }
        }

        private void OnConnected(IAsyncResult result)
        {
            byte[] buffer = new byte[1024];

            try
            {
                _server.EndConnect(result);
                _eventHandler.OnConnected(_server);

                if (_server.Connected)
                {
                    _server.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(OnDataReceived), buffer);
                }
                else
                {
                    _eventHandler.OnDisconnected(_server);
                }
            }
            catch (Exception ex)
            {
                _eventHandler.OnExceptionOccured(ex);
            }
        }

        public async Task SendAsync(byte[] data, int size)
        {
            IAsyncResult result = _server.BeginSend(data, 0, size, 0, OnDataSent, null);

            try
            {
                await Task.Factory.FromAsync(result, _server.EndSend);
            }
            catch (SocketException)
            {
            }
        }

        private void OnDataSent(IAsyncResult result) { }

        private void OnDataReceived(IAsyncResult result)
        {
            byte[] buffer = (byte[])result.AsyncState;
            int bytesRead;

            try
            {
                bytesRead = _server.EndReceive(result);
            }
            catch (SocketException)
            {
                _eventHandler.OnDisconnected(_server);
                return;
            }

            _eventHandler.OnDataReceived(_server, buffer, bytesRead);

            if (_server.Connected)
            {
                _server.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(OnDataReceived), buffer);
            }
            else
            {
                _eventHandler.OnDisconnected(_server);
            }
        }
    }
}
