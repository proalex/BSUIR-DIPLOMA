using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Generator
{
    public enum ServerOpcode
    {
        AUTH_RESPONSE,
        AUTH_SALT,
        UPDATE_GENERATORS,
        ADD_TASK,
        UPDATE_TASK,
        UPDATE_TASKS
    }

    public enum ClientOpcode
    {
        AUTH_REQUEST,
        SALT_REQUEST,
        LOGOUT_REQUEST,
        REGISTER_GENERATOR
    }

    public struct HandleData
    {
        public Handler handler;
        public Type dataType;
    }

    public delegate void Handler(object data);

    public static class OpcodesBinding
    {
        private static Dictionary<ServerOpcode, HandleData> _handlers = new Dictionary<ServerOpcode, HandleData>();
        private static Dictionary<Type, ClientOpcode> _opcodes = new Dictionary<Type, ClientOpcode>();
        public static ReadOnlyDictionary<ServerOpcode, HandleData> Handlers
        {
            get
            {
                return new ReadOnlyDictionary<ServerOpcode, HandleData>(_handlers);
            }
        }

        public static ReadOnlyDictionary<Type, ClientOpcode> Opcodes
        {
            get
            {
                return new ReadOnlyDictionary<Type, ClientOpcode>(_opcodes);
            }
        }

        static OpcodesBinding()
        {
            _opcodes.Add(typeof(RegGenerator), ClientOpcode.REGISTER_GENERATOR);

            _handlers.Add(ServerOpcode.ADD_TASK, new HandleData()
            {
                handler = TaskHandler.HandleAddTask,
                dataType = typeof(AddTask)
            });

            _handlers.Add(ServerOpcode.UPDATE_TASK, new HandleData()
            {
                handler = TaskHandler.HandleUpdateTask,
                dataType = typeof(UpdateTask)
            });
        }
    }
}
