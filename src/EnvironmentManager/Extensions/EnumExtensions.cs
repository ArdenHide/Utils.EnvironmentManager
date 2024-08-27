using System;
using System.Reflection;
using EnvironmentManager.Core;
using EnvironmentManager.Attributes;

namespace EnvironmentManager.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="Enum"/> types to retrieve environment variables using an <see cref="IEnvManager"/>.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Retrieves the environment variable associated with the given enum value, using the type specified in the <see cref="EnvironmentVariableAttribute"/>.
        /// </summary>
        /// <param name="key">The enum value representing the environment variable.</param>
        /// <param name="envManager">The <see cref="IEnvManager"/> to use for retrieving the environment variable. If <see langword="null"/>, the static <see cref="Static.EnvManager.Manager"/> will be used.</param>
        /// <returns>The environment variable value as a <see langword="dynamic"/> object, or the <see langword="default"/> value if the variable is not found.</returns>
        /// <remarks>
        /// The type to which the environment variable should be converted is specified by the <see cref="EnvironmentVariableAttribute"/> applied to the enum field.
        /// If the attribute is not present, the environment variable is treated as a string, and the environment variable is considered optional by default.
        /// </remarks>
        public static dynamic Get(this Enum key, IEnvManager? envManager = null)
        {
            var attribute = GetAttribute(key);
            return envManager == null
                ? Static.EnvManager.Get(attribute?.Type ?? typeof(string), key.ToString(), attribute?.IsRequired ?? false)
                : envManager.Get(attribute?.Type ?? typeof(string), key.ToString(), attribute?.IsRequired ?? false);
        }

        /// <summary>
        /// Retrieves the environment variable associated with the given enum value, converted to the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="key">The enum value representing the environment variable.</param>
        /// <param name="type">The <see cref="Type"/> to which the environment variable should be converted.</param>
        /// <param name="envManager">The <see cref="IEnvManager"/> to use for retrieving the environment variable. If <see langword="null"/>, the static <see cref="Static.EnvManager.Manager"/> will be used.</param>
        /// <returns>The environment variable value as an <see cref="object"/>, or the <see langword="default"/> value if the variable is not found.</returns>
        /// <remarks>
        /// If the <see cref="EnvironmentVariableAttribute"/> is applied to the enum field, its type is ignored in favor of the specified <paramref name="type"/>.
        /// If <see cref="EnvironmentVariableAttribute.IsRequired"/> is not explicitly set to <see langword="true"/>, the environment variable is considered optional by default.
        /// </remarks>
        public static object Get(this Enum key, Type type, IEnvManager? envManager = null)
        {
            var attribute = GetAttribute(key);
            return envManager == null
                ? Static.EnvManager.Get(type, key.ToString(), attribute?.IsRequired ?? false)
                : envManager.Get(type, key.ToString(), attribute?.IsRequired ?? false);
        }

        /// <summary>
        /// Retrieves the environment variable associated with the given enum value, converted to the specified type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The target type to which the environment variable should be converted.</typeparam>
        /// <param name="key">The enum value representing the environment variable.</param>
        /// <param name="envManager">The <see cref="IEnvManager"/> to use for retrieving the environment variable. If <see langword="null"/>, the static <see cref="Static.EnvManager.Manager"/> will be used.</param>
        /// <returns>The environment variable value as an object of type <typeparamref name="T"/>, or the <see langword="default"/> value if the variable is not found.</returns>
        /// <remarks>
        /// If the <see cref="EnvironmentVariableAttribute"/> is applied to the enum field, its type is ignored in favor of the specified type <typeparamref name="T"/>.
        /// If <see cref="EnvironmentVariableAttribute.IsRequired"/> is not explicitly set to <see langword="true"/>, the environment variable is considered optional by default.
        /// </remarks>
        public static T Get<T>(this Enum key, IEnvManager? envManager = null)
        {
            var attribute = GetAttribute(key);
            return envManager == null
                ? Static.EnvManager.Get<T>(key.ToString(), attribute?.IsRequired ?? false)
                : envManager.Get<T>(key.ToString(), attribute?.IsRequired ?? false);
        }

        /// <summary>
        /// Retrieves the required environment variable associated with the given enum value, using the type specified in the <see cref="EnvironmentVariableAttribute"/>.<br/>
        /// Throws an exception if the variable is not found.
        /// </summary>
        /// <param name="key">The enum value representing the environment variable.</param>
        /// <param name="envManager">The <see cref="IEnvManager"/> to use for retrieving the environment variable. If <see langword="null"/>, the static <see cref="Static.EnvManager.Manager"/> will be used.</param>
        /// <returns>The environment variable value as a <see langword="dynamic"/> object.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the environment variable is not found.</exception>
        /// <remarks>
        /// The type to which the environment variable should be converted is specified by the <see cref="EnvironmentVariableAttribute"/> applied to the enum field.
        /// If the attribute is not present, the environment variable is treated as a string.
        /// </remarks>
        public static dynamic GetRequired(this Enum key, IEnvManager? envManager = null)
        {
            var attribute = GetAttribute(key);
            return envManager == null
                ? Static.EnvManager.GetRequired(attribute?.Type ?? typeof(string),key.ToString())
                : envManager.GetRequired(attribute?.Type ?? typeof(string), key.ToString());
        }

        /// <summary>
        /// Retrieves the required environment variable associated with the given enum value, converted to the specified <paramref name="type"/>.<br/>
        /// Throws an exception if the variable is not found.
        /// </summary>
        /// <param name="key">The enum value representing the environment variable.</param>
        /// <param name="type">The <see cref="Type"/> to which the environment variable should be converted.</param>
        /// <param name="envManager">The <see cref="IEnvManager"/> to use for retrieving the environment variable. If <see langword="null"/>, the static <see cref="Static.EnvManager.Manager"/> will be used.</param>
        /// <returns>The environment variable value as an <see cref="object"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the environment variable is not found.</exception>
        public static object GetRequired(this Enum key, Type type, IEnvManager? envManager = null)
        {
            return envManager == null
                ? Static.EnvManager.GetRequired(type, key.ToString())
                : envManager.GetRequired(type, key.ToString());
        }

        /// <summary>
        /// Retrieves the required environment variable associated with the given enum value, converted to the specified type <typeparamref name="T"/>.<br/>
        /// Throws an exception if the variable is not found.
        /// </summary>
        /// <typeparam name="T">The target type to which the environment variable should be converted.</typeparam>
        /// <param name="key">The enum value representing the environment variable.</param>
        /// <param name="envManager">The <see cref="IEnvManager"/> to use for retrieving the environment variable. If <see langword="null"/>, the static <see cref="Static.EnvManager.Manager"/> will be used.</param>
        /// <returns>The environment variable value as an object of type <typeparamref name="T"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the environment variable is not found.</exception>
        public static T GetRequired<T>(this Enum key, IEnvManager? envManager = null)
        {
            return envManager == null
                ? Static.EnvManager.GetRequired<T>(key.ToString())
                : envManager.GetRequired<T>(key.ToString());
        }

        /// <summary>
        /// Retrieves the environment variable associated with the given enum value, converted to the specified type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The target type to which the environment variable should be converted.</typeparam>
        /// <param name="key">The enum value representing the environment variable.</param>
        /// <param name="type">The <see cref="Type"/> to which the environment variable should be converted.</param>
        /// <param name="raiseException">If <see langword="true"/>, throws an exception if the environment variable is not found; otherwise, returns the <see langword="default"/> value of <typeparamref name="T"/>.</param>
        /// <param name="envManager">The <see cref="IEnvManager"/> to use for retrieving the environment variable. If <see langword="null"/>, the static <see cref="Static.EnvManager.Manager"/> will be used.</param>
        /// <returns>The environment variable value as an object of type <typeparamref name="T"/>, or the <see langword="default"/> value if the variable is not found and <paramref name="raiseException"/> is <see langword="false"/>.</returns>
        internal static T GetInternal<T>(this Enum key, bool raiseException = false, IEnvManager? envManager = null)
        {
            return envManager == null
                ? Static.EnvManager.Get<T>(key.ToString(), raiseException)
                : envManager.Get<T>(key.ToString(), raiseException);
        }

        /// <summary>
        /// Retrieves the <see cref="EnvironmentVariableAttribute"/> associated with the given enum value.
        /// </summary>
        /// <param name="key">The enum value representing the environment variable.</param>
        /// <returns>The <see cref="EnvironmentVariableAttribute"/> associated with the given enum value, or <see langword="null"/> if no attribute is found.</returns>
        internal static EnvironmentVariableAttribute? GetAttribute(Enum key)
        {
            return key.GetType().GetField(key.ToString())?.GetCustomAttribute<EnvironmentVariableAttribute>();
        }
    }
}