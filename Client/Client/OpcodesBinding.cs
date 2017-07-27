using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Client
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

    public static class OpcodesBinding
    {
        private static Dictionary<ServerOpcode, Type> _opcodeTypes = new Dictionary<ServerOpcode, Type>();
        private static Dictionary<Type, Type> _requestResponseTypes = new Dictionary<Type, Type>();
        private static Dictionary<ClientOpcode, Type> _requestResponseOpcodes = new Dictionary<ClientOpcode, Type>();
        private static Dictionary<Type, ClientOpcode> _opcodes = new Dictionary<Type, ClientOpcode>();

        public static ReadOnlyDictionary<ServerOpcode, Type> OpcodeTypes
        {
            get
            {
                return new ReadOnlyDictionary<ServerOpcode, Type>(_opcodeTypes);
            }
        }

        public static ReadOnlyDictionary<Type, Type> RequestResponseTypes
        {
            get
            {
                return new ReadOnlyDictionary<Type, Type>(_requestResponseTypes);
            }
        }

        public static ReadOnlyDictionary<ClientOpcode, Type> RequestResponseOpcodes
        {
            get
            {
                return new ReadOnlyDictionary<ClientOpcode, Type>(_requestResponseOpcodes);
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
            _opcodeTypes[ServerOpcode.AUTH_RESPONSE] = typeof(AuthResponse);
            _opcodeTypes[ServerOpcode.AUTH_SALT] = typeof(AuthSalt);
            _opcodeTypes[ServerOpcode.UPDATE_GENERATORS] = typeof(ActiveGenerators);
            _opcodeTypes[ServerOpcode.UPDATE_TASKS] = typeof(ActiveTasks);
            _opcodeTypes[ServerOpcode.CREATION_RESULT] = typeof(TaskCreationResult);
            _opcodeTypes[ServerOpcode.TASK_INFO] = typeof(TaskInfoClient);
            _opcodeTypes[ServerOpcode.SAVE_PROFILE] = typeof(SaveProfileResponse);
            _opcodeTypes[ServerOpcode.PROFILES] = typeof(Profiles);
            _opcodeTypes[ServerOpcode.REPORTS] = typeof(Reports);

            _requestResponseOpcodes[ClientOpcode.SALT_REQUEST] = typeof(AuthSalt);
            _requestResponseOpcodes[ClientOpcode.LOGOUT_REQUEST] = typeof(EmptyResponse);
            _requestResponseOpcodes[ClientOpcode.DELETE_PROFILE] = typeof(EmptyResponse);

            _requestResponseTypes[typeof(AuthData)] = typeof(AuthResponse);
            _requestResponseTypes[typeof(TaskData)] = typeof(TaskCreationResult);
            _requestResponseTypes[typeof(RequestTaskInfo)] = typeof(TaskInfoClient);
            _requestResponseTypes[typeof(SaveProfileRequest)] = typeof(SaveProfileResponse);
            _requestResponseTypes[typeof(DeleteProfileRequest)] = typeof(EmptyResponse);

            _opcodes.Add(typeof(AuthData), ClientOpcode.AUTH_REQUEST);
            _opcodes.Add(typeof(TaskData), ClientOpcode.ADD_TASK);
            _opcodes.Add(typeof(RequestTaskInfo), ClientOpcode.TASK_INFO);
            _opcodes.Add(typeof(SaveProfileRequest), ClientOpcode.SAVE_PROFILE);
            _opcodes.Add(typeof(DeleteProfileRequest), ClientOpcode.DELETE_PROFILE);
        }
    }
}
