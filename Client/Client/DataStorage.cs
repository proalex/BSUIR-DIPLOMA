using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    public static class DataStorage
    {
        private static bool _connected = false;
        private static TCPClient _client;
        private static ControllerClient _controllerClient;
        private static Dictionary<Type, object> _data = new Dictionary<Type, object>();
        private static Dictionary<Type, EventWaitHandle> _dataUpdateNotifiers = new Dictionary<Type, EventWaitHandle>();
        public static bool Connected { get { return _connected; } }

        static DataStorage()
        {
            _controllerClient = ControllerClient.Instance;
            _client = new TCPClient(Config.Host, Config.Port, _controllerClient);
        }

        public static async Task ConnectAsync()
        {
            await _client.ConnectAsync();

            Exception ex = _controllerClient.GetLastException();

            if (ex != null)
            {
                throw ex;
            }

            _connected = true;
        }

        public static async Task RequestData(ClientOpcode opcode)
        {
            if (!OpcodesBinding.RequestResponseOpcodes.ContainsKey(opcode))
            {
                throw new ArgumentException("RequestResponseOpcodes doesn`t contain the response type");
            }

            Packet packet = new Packet(opcode, new byte[0]);

            Type responseType = OpcodesBinding.RequestResponseOpcodes[opcode];

            if (responseType != typeof(EmptyResponse))
            {
                if (!_dataUpdateNotifiers.ContainsKey(responseType))
                {
                    _dataUpdateNotifiers[responseType] = new EventWaitHandle(false, EventResetMode.ManualReset);
                }

                _dataUpdateNotifiers[responseType].Reset();
            }

            try
            {
                await _controllerClient.SendAsync(_client, packet);
            }
            catch (SocketException ex)
            {
                _connected = false;
                _client.Disconnect();
                throw ex;
            }

            if (responseType != typeof(EmptyResponse))
            {
                bool isSignaled = await AsyncFactory.FromWaitHandle(
                    _dataUpdateNotifiers[responseType],
                    TimeSpan.FromMilliseconds(Config.DataTimeout));

                if (!isSignaled)
                {
                    throw new TimeoutException("Server didn`t respont in time");
                }
            }
        }

        public static async Task RequestData<T>(T request)
        {
            if (request == null)
            {
                throw new NullReferenceException("request is null");
            }
            if (!OpcodesBinding.RequestResponseTypes.ContainsKey(typeof(T)))
            {
                throw new ArgumentException("RequestResponseTypes doesn`t contain the response type");
            }

            Type responseType = OpcodesBinding.RequestResponseTypes[typeof(T)];

            Packet packet = PacketBuilder.Build(request);

            if (responseType != typeof(EmptyResponse))
            {
                if (!_dataUpdateNotifiers.ContainsKey(responseType))
                {
                    _dataUpdateNotifiers[responseType] = new EventWaitHandle(false, EventResetMode.ManualReset);
                }

                _dataUpdateNotifiers[responseType].Reset();
            }

            try
            {
                await _controllerClient.SendAsync(_client, packet);
            }
            catch (SocketException ex)
            {
                _connected = false;
                _client.Disconnect();
                throw ex;
            }

            if (responseType != typeof(EmptyResponse))
            {
                bool isSignaled = await AsyncFactory.FromWaitHandle(
                    _dataUpdateNotifiers[responseType],
                    TimeSpan.FromMilliseconds(Config.DataTimeout));

                if (!isSignaled)
                {
                    throw new TimeoutException("Server didn`t respont in time");
                }
            }
        }

        public static T GetData<T>()
        {
            if (!_data.ContainsKey(typeof(T)))
            {
                return default(T);
            }

            return (T)_data[typeof(T)];
        }

        public static void UpdateData(Type type, object data)
        {
            if (type == typeof(EmptyResponse))
            {
                return;
            }

            lock (_data)
            {
                _data[type] = data;

                if (_dataUpdateNotifiers.ContainsKey(type))
                {
                    _dataUpdateNotifiers[type].Set();
                }
            }
        }
    }
}
