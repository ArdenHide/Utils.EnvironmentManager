using System;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace EnvironmentManager.Core
{
    public class EnvManager : IEnvManager
    {
        public IMapper Mapper { get; }
        public ILogger<IEnvManager> Logger { get; }

        public EnvManager(IConfigurationProvider? config = null, ILogger<IEnvManager>? logger = null)
        {
            config?.AssertConfigurationIsValid();
            Mapper = new Mapper(config ?? DefaultMapperConfiguration.DefaultConfiguration);
            Logger = logger ?? NullLogger<IEnvManager>.Instance;
        }

        public object Get(Type type, string variableName, bool raiseException = false)
        {
            return GetInternal<object>(type, variableName, raiseException);
        }

        public T Get<T>(string variableName, bool raiseException = false)
        {
            return GetInternal<T>(typeof(T), variableName, raiseException);
        }

        public object GetRequired(Type type, string variableName)
        {
            return GetInternal<object>(type, variableName, true);
        }

        public T GetRequired<T>(string variableName)
        {
            return GetInternal<T>(typeof(T), variableName, true);
        }

        internal T GetInternal<T>(Type type, string variableName, bool raiseException)
        {
            var envValue = Environment.GetEnvironmentVariable(variableName);
            return ConvertEnvironmentValueInternal<T>(type, variableName, envValue, raiseException);
        }

        internal T ConvertEnvironmentValueInternal<T>(Type targetType, string variableName, string? envValue, bool raiseException)
        {
            if (string.IsNullOrWhiteSpace(envValue) && raiseException)
            {
                throw new ArgumentNullException(nameof(envValue), $"Environment variable '{variableName}' is null or empty.");
            }

            if (string.IsNullOrWhiteSpace(envValue) && !raiseException)
            {
                Logger.LogWarning("Environment variable '{VariableName}' is null or empty. Trying return default value.", variableName);
                return default!;
            }

            try
            {
                return (T)Mapper.Map(envValue, typeof(string), targetType);
            }
            catch (Exception ex)
            {
                if (raiseException)
                {
                    throw new InvalidCastException($"Failed to convert environment variable '{variableName}' to type '{targetType}'.", ex);
                }

                Logger.LogError("Failed to convert environment variable '{VariableName}' to type '{Type}'. Trying return default value.", variableName, targetType);

                return default!;
            }
        }
    }
}
