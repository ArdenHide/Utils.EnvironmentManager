using System;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace EnvironmentManager
{
    public class EnvManager
    {
        private readonly IMapper mapper;
        private readonly ILogger<EnvManager> logger;

        public EnvManager(ILogger<EnvManager>? logger = null)
            : this(new EnvManagerMappingConfigurator().Build(), logger)
        { }

        public EnvManager(IConfigurationProvider config, ILogger<EnvManager>? logger = null)
        {
            mapper = new Mapper(config);
            this.logger = logger ?? NullLogger<EnvManager>.Instance;
        }

        public object GetEnvironmentValue(Type type, string variableName, bool raiseException = false)
        {
            var method = typeof(EnvManager).GetMethod(nameof(GetEnvironmentValue), new[] { typeof(string), typeof(bool) });
            var genericMethod = method!.MakeGenericMethod(type);
            return genericMethod.Invoke(this, new object[] { variableName, raiseException })!;
        }

        public T GetEnvironmentValue<T>(string variableName, bool raiseException = false)
        {
            var envValue = Environment.GetEnvironmentVariable(variableName);

            if (!string.IsNullOrEmpty(envValue))
            {
                return ConvertEnvironmentValue<T>(variableName, envValue, raiseException);
            }

            if (raiseException)
            {
                throw new InvalidOperationException($"Environment variable '{variableName}' is null or empty.");
            }

            logger.LogWarning("Environment variable '{VariableName}' is null or empty.", variableName);

            return default!;
        }

        private T ConvertEnvironmentValue<T>(string variableName, string envValue, bool raiseException)
        {
            try
            {
                return mapper.Map<string, T>(envValue);
            }
            catch (Exception ex)
            {
                if (raiseException)
                {
                    throw new InvalidCastException($"Failed to convert environment variable '{variableName}' to type '{typeof(T)}'.", ex);
                }

                logger.LogError("Failed to convert environment variable '{VariableName}' to type '{Type}'. Returning default value.", variableName, typeof(T));
                return default!;
            }
        }
    }
}
