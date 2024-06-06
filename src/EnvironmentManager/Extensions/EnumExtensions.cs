using System;

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
        /// <typeparam name="TResponse">The type of the environment value to retrieve.</typeparam>
        /// <param name="key">The enum key associated with the environment value.</param>
        /// <param name="raiseException">Specifies whether to throw an exception if the environment value is not found.</param>
        /// <param name="envManager">Optional. An instance of <see cref="EnvManager"/> to use for retrieving environment values. If not provided, a default instance is used.</param>
        /// <returns>The environment value cast to the specified type.</returns>
        public static TResponse GetEnvironmentValue<TResponse>(this Enum key, bool raiseException = false, EnvManager? envManager = null)
        {
            envManager ??= _envManager;
            return envManager.GetEnvironmentValue<TResponse>(key.ToString(), raiseException);
        }

        /// <summary>
        /// Retrieves a string environment value associated with the specified enum key.
        /// </summary>
        /// <param name="key">The enum key associated with the environment value.</param>
        /// <param name="raiseException">Specifies whether to throw an exception if the environment value is not found.</param>
        /// <param name="envManager">Optional. An instance of <see cref="EnvManager"/> to use for retrieving environment values. If not provided, a default instance is used.</param>
        /// <returns>The environment value as a string.</returns>
        public static string GetEnvironmentValue(this Enum key, bool raiseException = false, EnvManager? envManager = null)
        {
            return key.GetEnvironmentValue<string>(raiseException, envManager);
        }
    }
}