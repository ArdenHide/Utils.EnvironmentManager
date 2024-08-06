using System;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace EnvironmentManager.Static
{
    public static class EnvManager
    {
        private static Core.EnvManager _envManager = new Core.EnvManager();

        public static void Initialize(IConfigurationProvider? config = null, ILogger<Core.EnvManager>? logger = null)
        {
            _envManager = new Core.EnvManager(config, logger);
        }

        public static object Get(Type type, string variableName, bool raiseException = false)
        {
            return _envManager.Get(type, variableName, raiseException);
        }

        public static T Get<T>(string variableName, bool raiseException = false)
        {
            return _envManager.Get<T>(variableName, raiseException);
        }

        public static object GetRequired(Type type, string variableName)
        {
            return _envManager.GetRequired(type, variableName);
        }

        public static T GetRequired<T>(string variableName)
        {
            return _envManager.GetRequired<T>(variableName);
        }
    }
}
