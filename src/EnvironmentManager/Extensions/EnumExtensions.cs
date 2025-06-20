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
        /// Retrieves the environment variable associated with the given enum value, converted to the <see cref="string"/> type.
        /// </summary>
        /// <param name="key">The enum value representing the environment variable.</param>
        /// <param name="envManager">The <see cref="IEnvManager"/> to use for retrieving the environment variable. If <see langword="null"/>, the static <see cref="Static.EnvManager.Manager"/> will be used.</param>
        /// <returns>The environment variable value as an <see cref="string"/>, or the <see langword="null"/> value if the variable is not found.</returns>
        /// <remarks>
        /// If <see cref="EnvironmentVariableAttribute.IsRequired"/> is not explicitly set to <see langword="true"/>, the environment variable is considered optional by default.
        /// </remarks>
        public static string Get(this Enum key, IEnvManager? envManager = null)
        {
            var attribute = GetAttribute(key);
            var manager = GetEnvManager(envManager);
            return manager.Get(key.ToString(), attribute?.IsRequired ?? false);
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
            var manager = GetEnvManager(envManager);
            return manager.Get(type, key.ToString(), attribute?.IsRequired ?? false);
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
            var manager = GetEnvManager(envManager);
            return manager.Get<T>(key.ToString(), attribute?.IsRequired ?? false);
        }

        /// <summary>
        /// Retrieves the environment variable associated with the given enum value and converts it to the <see cref="string"/> type;
        /// if the variable is missing or empty, returns the supplied <paramref name="defaultValue"/>.
        /// </summary>
        /// <param name="key">The enum value representing the environment variable.</param>
        /// <param name="defaultValue">The fallback value to return when the variable is not found or its value is <see langword="null"/> / empty.</param>
        /// <param name="envManager">The <see cref="IEnvManager"/> to use for retrieving the environment variable. If <see langword="null"/>, the static <see cref="Static.EnvManager.Manager"/> will be used.</param>
        /// <returns>The environment-variable value as a <see cref="string"/>, or the provided <paramref name="defaultValue"/> when the variable is not present or invalid.</returns>
        /// <remarks>The <see cref="EnvironmentVariableAttribute.IsRequired"/> flag is ignored: the variable is always treated as optional, because a default value is supplied.</remarks>
        public static string GetOrDefault(this Enum key, string defaultValue, IEnvManager? envManager = null)
        {
            var manager = GetEnvManager(envManager);
            return manager.GetOrDefault(key.ToString(), defaultValue);
        }

        /// <summary>
        /// Retrieves the environment variable associated with the given enum value and converts it to the specified <paramref name="type"/>;
        /// if the variable is missing or cannot be converted, returns the supplied <paramref name="defaultValue"/>.
        /// </summary>
        /// <param name="key">The enum value representing the environment variable.</param>
        /// <param name="type">The target <see cref="Type"/> for conversion.</param>
        /// <param name="defaultValue">The fallback value to return when the variable is not found or conversion fails.</param>
        /// <param name="envManager">The <see cref="IEnvManager"/> to use for retrieving the environment variable. If <see langword="null"/>, the static <see cref="Static.EnvManager.Manager"/> will be used.</param>
        /// <returns>An object of the specified <paramref name="type"/>, or the provided <paramref name="defaultValue"/> when the variable is not present or cannot be converted.</returns>
        /// <remarks>
        /// The <see cref="EnvironmentVariableAttribute"/> applied to the enum field (if any) is ignored in favour of the explicit <paramref name="type"/> parameter.
        /// <see cref="EnvironmentVariableAttribute.IsRequired"/> is ignored because a default value is available.
        /// </remarks>
        public static object GetOrDefault(this Enum key, Type type, object defaultValue, IEnvManager? envManager = null)
        {
            var manager = GetEnvManager(envManager);
            return manager.GetOrDefault(type, key.ToString(), defaultValue);
        }

        /// <summary>
        /// Retrieves the environment variable associated with the given enum value and converts it to the specified type <typeparamref name="T"/>;
        /// if the variable is missing or cannot be converted, returns the supplied <paramref name="defaultValue"/>.
        /// </summary>
        /// <typeparam name="T">The target type for conversion.</typeparam>
        /// <param name="key">The enum value representing the environment variable.</param>
        /// <param name="defaultValue">The fallback value to return when the variable is not found or conversion fails.</param>
        /// <param name="envManager">The <see cref="IEnvManager"/> to use for retrieving the environment variable. If <see langword="null"/>, the static <see cref="Static.EnvManager.Manager"/> will be used.</param>
        /// <returns>An object of type <typeparamref name="T"/>, or the provided <paramref name="defaultValue"/> when the variable is not present or cannot be converted.</returns>
        /// <remarks>
        /// The <see cref="EnvironmentVariableAttribute"/> applied to the enum field (if any) is ignored in favour of the generic type <typeparamref name="T"/>.  
        /// <see cref="EnvironmentVariableAttribute.IsRequired"/> is ignored because a default value is supplied.
        /// </remarks>
        public static T GetOrDefault<T>(this Enum key, T defaultValue, IEnvManager? envManager = null)
        {
            var manager = GetEnvManager(envManager);
            return manager.GetOrDefault<T>(key.ToString(), defaultValue);
        }

        /// <summary>
        /// Retrieves the required environment variable associated with the given enum value, converted to the <see cref="string"/> type.<br/>
        /// Throws an exception if the variable is not found.
        /// </summary>
        /// <param name="key">The enum value representing the environment variable.</param>
        /// <param name="envManager">The <see cref="IEnvManager"/> to use for retrieving the environment variable. If <see langword="null"/>, the static <see cref="Static.EnvManager.Manager"/> will be used.</param>
        /// <returns>The environment variable value as an <see cref="string"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the environment variable is not found.</exception>
        public static string GetRequired(this Enum key, IEnvManager? envManager = null)
        {
            var manager = GetEnvManager(envManager);
            return manager.GetRequired(key.ToString());
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
            var manager = GetEnvManager(envManager);
            return manager.GetRequired(type, key.ToString());
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
            var manager = GetEnvManager(envManager);
            return manager.GetRequired<T>(key.ToString());
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

        /// <summary>
        /// Determines the appropriate <see cref="IEnvManager"/> to use for environment variable retrieval.
        /// </summary>
        /// <param name="envManager">An optional custom <see cref="IEnvManager"/> provided by the user. If <see langword="null"/>, the default static manager will be used.</param>
        /// <returns>The <see cref="IEnvManager"/> to use, either the provided custom manager or the default static manager.</returns>
        /// <remarks>
        /// This method centralizes the logic for determining which <see cref="IEnvManager"/> instance to use, reducing redundancy and improving maintainability of the environment retrieval methods.
        /// </remarks>
        internal static IEnvManager GetEnvManager(IEnvManager? envManager)
        {
            return envManager ?? Static.EnvManager.Manager;
        }
    }
}