using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ControllerServer
{
    public enum ServerOpcode
    {
        AUTH_RESPONSE,
        AUTH_SALT,
        UPDATE_GENERATORS,
        ADD_TASK,
        UPDATE_TASK,
        UPDATE_TASKS,
        CREATION_RESULT,
        TASK_INFO,
        SAVE_PROFILE,
        PROFILES,
        REPORTS
    }

    public enum ClientOpcode
    {
        AUTH_REQUEST,
        SALT_REQUEST,
        LOGOUT_REQUEST,
        REGISTER_GENERATOR,
        ADD_TASK,
        TASK_INFO,
        SAVE_PROFILE,
        DELETE_PROFILE
    }

    public delegate void Handler(Client state, object data);

    public struct HandleData
    {
        public Handler handler;
        public Type dataType;
    }

    public static class OpcodesBinding
    {
        private static Dictionary<ClientOpcode, HandleData> _handlers = new Dictionary<ClientOpcode, HandleData>();
        private static Dictionary<Type, ServerOpcode> _opcodes = new Dictionary<Type, ServerOpcode>();

        public static ReadOnlyDictionary<ClientOpcode, HandleData> Handlers
        {
            get
            {
                return new ReadOnlyDictionary<ClientOpcode, HandleData>(_handlers);
            }
        }

        public static ReadOnlyDictionary<Type, ServerOpcode> Opcodes
        {
            get
            {
                return new ReadOnlyDictionary<Type, ServerOpcode>(_opcodes);
            }
        }

        static OpcodesBinding()
        {
            _handlers.Add(ClientOpcode.AUTH_REQUEST, new HandleData()
            {
                handler = AuthHandler.HandleAuthRequest, dataType = typeof(AuthData)
            });

            _handlers.Add(ClientOpcode.SALT_REQUEST, new HandleData()
            {
                handler = AuthHandler.HandleSaltRequest
            });

            _handlers.Add(ClientOpcode.LOGOUT_REQUEST, new HandleData()
            {
                handler = AuthHandler.HandleLogoutRequest
            });

            _handlers.Add(ClientOpcode.REGISTER_GENERATOR, new HandleData()
            {
                handler = GeneratorHandler.HandleRegistration, dataType = typeof(RegGenerator)
            });

            _handlers.Add(ClientOpcode.ADD_TASK, new HandleData()
            {
                handler = TaskHandler.HandleAddTask,
                dataType = typeof(TaskData)
            });

            _handlers.Add(ClientOpcode.TASK_INFO, new HandleData()
            {
                handler = TaskHandler.HandleTaskInfo,
                dataType = typeof(RequestTaskInfo)
            });

            _handlers.Add(ClientOpcode.SAVE_PROFILE, new HandleData()
            {
                handler = ProfileHandler.HandleSaveProfile,
                dataType = typeof(SaveProfileRequest)
            });

            _handlers.Add(ClientOpcode.DELETE_PROFILE, new HandleData()
            {
                handler = ProfileHandler.HandleDeleteProfile,
                dataType = typeof(DeleteProfileRequest)
            });

            _opcodes.Add(typeof(AuthResponse), ServerOpcode.AUTH_RESPONSE);
            _opcodes.Add(typeof(AuthSalt), ServerOpcode.AUTH_SALT);
            _opcodes.Add(typeof(ActiveGenerators), ServerOpcode.UPDATE_GENERATORS);
            _opcodes.Add(typeof(AddTask), ServerOpcode.ADD_TASK);
            _opcodes.Add(typeof(UpdateTask), ServerOpcode.UPDATE_TASK);
            _opcodes.Add(typeof(ActiveTasks), ServerOpcode.UPDATE_TASKS);
            _opcodes.Add(typeof(TaskCreationResult), ServerOpcode.CREATION_RESULT);
            _opcodes.Add(typeof(TaskInfoClient), ServerOpcode.TASK_INFO);
            _opcodes.Add(typeof(SaveProfileResponse), ServerOpcode.SAVE_PROFILE);
            _opcodes.Add(typeof(Profiles), ServerOpcode.PROFILES);
            _opcodes.Add(typeof(Reports), ServerOpcode.REPORTS);
        }
    }
}
