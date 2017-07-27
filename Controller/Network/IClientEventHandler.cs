using System.Net.Sockets;

namespace ControllerServer
{
	public interface IClientEventHandler
	{
		void OnClientConnected(Socket client);
		void OnClientDisconnected(Socket client);
		void OnDataReceived(Socket client, byte[] data, int bytesRead);
	}
}
