using System;
using AutoMapper;
using EnvironmentManager.Core;
using Microsoft.Extensions.Logging;

namespace EnvironmentManager.Static
{
    public static class EnvManager
    {
        public static IEnvManager Manager { get; private set; } = new Core.EnvManager();

        public static void Initialize(IEnvManager envManager)
        {
            Manager = envManager;
        }

        public static void Initialize(IConfigurationProvider? config = null, ILogger<IEnvManager>? logger = null)
        {
            Manager = new Core.EnvManager(config, logger);
        }

        public static object Get(Type type, string variableName, bool raiseException = false)
        {
            return Manager.Get(type, variableName, raiseException);
        }

        public static T Get<T>(string variableName, bool raiseException = false)
        {
            return Manager.Get<T>(variableName, raiseException);
        }

        public static object GetRequired(Type type, string variableName)
        {
            return Manager.GetRequired(type, variableName);
        }

        public static T GetRequired<T>(string variableName)
        {
            return Manager.GetRequired<T>(variableName);
        }
    }
}
