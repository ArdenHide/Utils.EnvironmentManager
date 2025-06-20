using System;
using AutoMapper;
using EnvironmentManager.Core;
using Microsoft.Extensions.Logging;

namespace EnvironmentManager.Static
{
    /// <summary>
    /// Provides a static interface for managing environment variables with type conversion and logging capabilities.
    /// </summary>
    public static class EnvManager
    {
        /// <summary>
        /// Gets the current <see cref="IEnvManager"/> instance used for managing environment variables.
        /// </summary>
        public static IEnvManager Manager { get; private set; } = new Core.EnvManager();

        /// <summary>
        /// Initializes the <see cref="Manager"/> with the specified <see cref="IEnvManager"/> instance.
        /// </summary>
        /// <param name="envManager">The <see cref="IEnvManager"/> instance to use for managing environment variables.</param>
        public static void Initialize(IEnvManager envManager)
        {
            Manager = envManager;
        }

        /// <summary>
        /// Initializes the <see cref="Manager"/> with a new instance of <see cref="Core.EnvManager"/> using the specified configuration and logger.
        /// </summary>
        /// <param name="config">The AutoMapper <see cref="IConfigurationProvider"/> used to configure mappings. If <see langword="null"/>, the default configuration is used.</param>
        /// <param name="logger">The logger used for logging operations. If <see langword="null"/>, a <see cref="Microsoft.Extensions.Logging.Abstractions.NullLogger{T}"/> instance is used.</param>
        public static void Initialize(IConfigurationProvider? config = null, ILogger<IEnvManager>? logger = null)
        {
            Manager = new Core.EnvManager(config, logger);
        }

        /// <inheritdoc cref="IEnvManager.Get(string, bool)"/>
        public static string Get(string variableName, bool raiseException = false)
        {
            return Manager.Get(variableName, raiseException);
        }

        /// <inheritdoc cref="IEnvManager.Get(Type, string, bool)"/>
        public static object Get(Type type, string variableName, bool raiseException = false)
        {
            return Manager.Get(type, variableName, raiseException);
        }

        /// <inheritdoc cref="IEnvManager.Get{T}(string, bool)"/>
        public static T Get<T>(string variableName, bool raiseException = false)
        {
            return Manager.Get<T>(variableName, raiseException);
        }

        /// <inheritdoc cref="IEnvManager.GetOrDefault(string, string)"/>
        public static string GetOrDefault(string variableName, string defaultValue)
        {
            return Manager.GetOrDefault(variableName, defaultValue);
        }

        /// <inheritdoc cref="IEnvManager.GetOrDefault(Type, string, object)"/>
        public static object GetOrDefault(Type type, string variableName, object defaultValue)
        {
            return Manager.GetOrDefault(type, variableName, defaultValue);
        }

        /// <inheritdoc cref="IEnvManager.GetOrDefault{T}(string, T)"/>
        public static T GetOrDefault<T>(string variableName, T defaultValue)
        {
            return Manager.GetOrDefault<T>(variableName, defaultValue);
        }

        /// <inheritdoc cref="IEnvManager.GetRequired(string)"/>
        public static string GetRequired(string variableName)
        {
            return Manager.GetRequired(variableName);
        }

        /// <inheritdoc cref="IEnvManager.GetRequired(Type, string)"/>
        public static object GetRequired(Type type, string variableName)
        {
            return Manager.GetRequired(type, variableName);
        }

        /// <inheritdoc cref="IEnvManager.GetRequired{T}(string)"/>
        public static T GetRequired<T>(string variableName)
        {
            return Manager.GetRequired<T>(variableName);
        }
    }
}
