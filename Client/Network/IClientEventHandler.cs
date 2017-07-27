using System;
using System.Net.Sockets;

namespace Client
{
    public interface IClientEventHandler
    {
        void OnConnected(Socket server);
        void OnDisconnected(Socket server);
        void OnExceptionOccured(Exception ex);
        void OnDataReceived(Socket server, byte[] data, int bytesRead);
    }
}
