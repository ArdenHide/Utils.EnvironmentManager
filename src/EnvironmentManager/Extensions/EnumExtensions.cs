using System;
using System.Reflection;
using EnvironmentManager.Attributes;

namespace EnvironmentManager.Extensions
{
    /// <summary>
    /// Provides extension methods for Enums to work with environment settings.
    /// </summary>
    public static class EnumExtensions
    {
        private static readonly EnvManager _envManager = new EnvManager();

        /// <summary>
        /// Retrieves an environment value associated with the specified enum key.
        /// </summary>
        /// <param name="key">The enum key associated with the environment value.</param>
        /// <param name="envManager">Optional. An instance of <see cref="EnvManager"/> to use for retrieving environment values. If not provided, a default instance is used.</param>
        /// <returns>The environment value cast to the specified type.</returns>
        public static object GetEnvironmentValue(this Enum key, EnvManager? envManager = null)
        {
            var attribute = key.GetType().GetField(key.ToString())?.GetCustomAttribute<EnvironmentVariableAttribute>();

            var targetType = attribute?.Type ?? typeof(string);
            var raiseException = attribute?.IsRequired ?? false;

            envManager ??= _envManager;
            return envManager.GetEnvironmentValue(targetType, key.ToString(), raiseException);
        }
    }
}