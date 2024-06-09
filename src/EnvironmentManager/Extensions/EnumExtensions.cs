using System;
using System.Reflection;
using EnvironmentManager.Attributes;

namespace EnvironmentManager.Extensions
{
    /// <summary>
    /// Provides extension methods for Enums to work with environment settings, using attributes to specify type and requirement status.
    /// </summary>
    public static class EnumExtensions
    {
        private static readonly EnvManager _envManager = new EnvManager();

        /// <summary>
        /// Retrieves an environment value associated with the specified enum key.<br/>
        /// Utilizes <see cref="EnvironmentVariableAttribute"/> to determine the type and requirement of the environment variable.<br/>
        /// If no attribute is found, defaults to a string type and not required.
        /// </summary>
        /// <param name="key">The enum key associated with the environment value.</param>
        /// <param name="envManager">Optional. An instance of <see cref="EnvManager"/> to use for retrieving environment values. If not provided, a default instance is used.</param>
        /// <returns>The environment value cast to the type specified in the <see cref="EnvironmentVariableAttribute"/>, or to string if no attribute is set.</returns>
        public static object Get(this Enum key, EnvManager? envManager = null)
        {
            var attribute = key.GetType().GetField(key.ToString())?.GetCustomAttribute<EnvironmentVariableAttribute>();

            var targetType = attribute?.Type ?? typeof(string);
            var raiseException = attribute?.IsRequired ?? false;

            envManager ??= _envManager;
            return envManager.GetEnvironmentValue(targetType, key.ToString(), raiseException);
        }
    }
}