using System;
using AutoMapper;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using EnvironmentManager.Configuration;
using Microsoft.Extensions.Logging.Abstractions;

namespace EnvironmentManager.Core
{
    /// <inheritdoc cref="IEnvManager.Logger"/>
    public class EnvManager : IEnvManager
    {
        /// <inheritdoc cref="IEnvManager.Logger"/>
        public IMapper Mapper { get; }

        /// <inheritdoc cref="IEnvManager.Logger"/>
        public ILogger<IEnvManager> Logger { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvManager"/> class.
        /// </summary>
        /// <param name="config">The AutoMapper <see cref="IConfigurationProvider"/> used to configure mappings. If <see langword="null"/>, the default configuration is used.</param>
        /// <param name="logger">The logger used for logging operations. If <see langword="null"/>, a <see cref="NullLogger{T}"/> instance is used.</param>
        public EnvManager(IConfigurationProvider? config = null, ILogger<IEnvManager>? logger = null)
        {
            config?.AssertConfigurationIsValid();
            Mapper = new Mapper(config ?? DefaultMapperConfiguration.DefaultConfiguration);
            Logger = logger ?? NullLogger<IEnvManager>.Instance;
        }

        /// <inheritdoc cref="IEnvManager.Get(string, bool)"/>
        public string Get(string variableName, bool raiseException = false)
        {
            return GetInternal<string>(typeof(string), variableName, raiseException);
        }

        /// <inheritdoc cref="IEnvManager.Get(Type, string, bool)"/>
        public object Get(Type type, string variableName, bool raiseException = false)
        {
            return GetInternal<object>(type, variableName, raiseException);
        }

        /// <inheritdoc cref="IEnvManager.Get{T}(string, bool)"/>
        public T Get<T>(string variableName, bool raiseException = false)
        {
            return GetInternal<T>(typeof(T), variableName, raiseException);
        }

        /// <inheritdoc cref="IEnvManager.GetOrDefault(string, string)"/>
        public string GetOrDefault(string variableName, string defaultValue)
        {
            return GetInternal<string?>(typeof(string), variableName, false) ?? defaultValue;
        }

        /// <inheritdoc cref="IEnvManager.GetOrDefault(Type, string, object)"/>
        public object GetOrDefault(Type type, string variableName, object defaultValue)
        {
            return GetInternal<object?>(type, variableName, false) ?? defaultValue;
        }

        /// <inheritdoc cref="IEnvManager.GetOrDefault{T}(string, T)"/>
        public T GetOrDefault<T>(string variableName, T defaultValue)
        {
            var value = GetInternal<T>(typeof(T), variableName, false);
            return value == null || EqualityComparer<T>.Default.Equals(value, default!) ? defaultValue : value;
        }

        /// <inheritdoc cref="IEnvManager.GetRequired(string)"/>
        public string GetRequired(string variableName)
        {
            return GetInternal<string>(typeof(string), variableName, true);
        }

        /// <inheritdoc cref="IEnvManager.GetRequired(Type, string)"/>
        public object GetRequired(Type type, string variableName)
        {
            return GetInternal<object>(type, variableName, true);
        }

        /// <inheritdoc cref="IEnvManager.GetRequired{T}(string)"/>
        public T GetRequired<T>(string variableName)
        {
            return GetInternal<T>(typeof(T), variableName, true);
        }

        /// <summary>
        /// Retrieves the environment variable with the specified <paramref name="variableName"/> and converts it to the specified type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The target type to which the environment variable should be converted.</typeparam>
        /// <param name="type">The <see cref="Type"/> to which the environment variable should be converted.</param>
        /// <param name="variableName">The name of the environment variable to retrieve.</param>
        /// <param name="raiseException">If <see langword="true"/>, throws an exception if the environment variable is not found; otherwise, returns the <see langword="default"/> value of <typeparamref name="T"/>.</param>
        /// <returns>An object of type <typeparamref name="T"/>.</returns>
        internal T GetInternal<T>(Type type, string variableName, bool raiseException)
        {
            var envValue = Environment.GetEnvironmentVariable(variableName);
            return ConvertEnvironmentValueInternal<T>(type, variableName, envValue, raiseException);
        }

        /// <summary>
        /// Converts the given environment variable value to the specified type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The target type to which the environment variable should be converted.</typeparam>
        /// <param name="targetType">The <see cref="Type"/> to which the environment variable should be converted.</param>
        /// <param name="variableName">The name of the environment variable to retrieve.</param>
        /// <param name="envValue">The value of the environment variable to convert.</param>
        /// <param name="raiseException">If <see langword="true"/>, throws an exception if the conversion fails; otherwise, returns the <see langword="default"/> value of <typeparamref name="T"/>.</param>
        /// <returns>An object of type <typeparamref name="T"/>, or the <see langword="default"/> value of <typeparamref name="T"/> if the conversion fails and <paramref name="raiseException"/> is <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="envValue"/> is <see langword="null"/> or empty and <paramref name="raiseException"/> is <see langword="true"/>.</exception>
        /// <exception cref="InvalidCastException">Thrown if the conversion of <paramref name="envValue"/> to <typeparamref name="T"/> fails and <paramref name="raiseException"/> is <see langword="true"/>.</exception>
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

                Logger.LogError(ex, "Failed to convert environment variable '{VariableName}' to type '{Type}'. Trying return default value.", variableName, targetType);

                return default!;
            }
        }
    }
}
