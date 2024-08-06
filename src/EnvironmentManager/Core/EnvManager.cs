using System;
using AutoMapper;
using EnvironmentManager.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace EnvironmentManager.Core
{
    public class EnvManager
    {
        private readonly IMapper _mapper;
        private readonly ILogger<EnvManager> _logger;

        public EnvManager() : this(null, null) { }

        public EnvManager(IConfigurationProvider? config = null, ILogger<EnvManager>? logger = null)
        {
            _mapper = new Mapper(config ?? new MapperConfiguration(DefaultConfigurationProvider.DefaultConfiguration));
            _logger = logger ?? NullLogger<EnvManager>.Instance;
        }

        public object Get(Type type, string variableName, bool raiseException = false)
        {
            var envValue = GetEnvironmentVariable(variableName, raiseException);
            return ConvertEnvironmentValue(type, variableName, envValue, raiseException);
        }

        public T Get<T>(string variableName, bool raiseException = false)
        {
            var envValue = GetEnvironmentVariable(variableName, raiseException);
            return ConvertEnvironmentValue<T>(variableName, envValue, raiseException);
        }

        public object GetRequired(Type type, string variableName)
        {
            var envValue = GetEnvironmentVariable(variableName, true);
            return ConvertEnvironmentValue(type, variableName, envValue, true);
        }

        public T GetRequired<T>(string variableName)
        {
            var envValue = GetEnvironmentVariable(variableName, true);
            return ConvertEnvironmentValue<T>(variableName, envValue, true);
        }

        private string GetEnvironmentVariable(string variableName, bool raiseException)
        {
            var envValue = Environment.GetEnvironmentVariable(variableName);

            if (!string.IsNullOrWhiteSpace(envValue)) return envValue;

            if (raiseException)
            {
                throw new InvalidOperationException($"Environment variable '{variableName}' is null or empty.");
            }

            _logger.LogWarning("Environment variable '{VariableName}' is null or empty. Trying return default value.", variableName);

            return TypeExtensions.GetDefaultValueOrThrow<string>();
        }

        private T ConvertEnvironmentValue<T>(string variableName, string envValue, bool raiseException)
        {
            return ConvertEnvironmentValueInternal<T>(typeof(T), variableName, envValue, raiseException);
        }

        private object ConvertEnvironmentValue(Type type, string variableName, string envValue, bool raiseException)
        {
            return ConvertEnvironmentValueInternal<object>(type, variableName, envValue, raiseException);
        }

        private T ConvertEnvironmentValueInternal<T>(Type targetType, string variableName, string envValue, bool raiseException)
        {
            try
            {
                return (T)_mapper.Map(envValue, typeof(string), targetType);
            }
            catch (Exception ex)
            {
                if (raiseException)
                {
                    throw new InvalidCastException($"Failed to convert environment variable '{variableName}' to type '{targetType}'.", ex);
                }

                _logger.LogError("Failed to convert environment variable '{VariableName}' to type '{Type}'. Trying return default value.", variableName, targetType);
                return TypeExtensions.GetDefaultValueOrThrow<T>();
            }
        }
    }
}
