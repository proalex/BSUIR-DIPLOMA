using System;
using System.Net.Sockets;

namespace ControllerServer
{
	public interface IServerEventHandler
	{
		void OnListen(Socket _listener);
		void OnExceptionOccured(Exception ex);
	}
}
