using System;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace EnvironmentManager.Core
{
    /// <summary>
    /// Provides methods for managing environment variables.
    /// </summary>
    public interface IEnvManager
    {
        /// <summary>
        /// Gets the <see cref="IMapper"/> instance used for mapping environment variables to strongly-typed objects.
        /// </summary>
        public IMapper Mapper { get; }

        /// <summary>
        /// Gets the <see cref="ILogger{TCategoryName}"/> instance used for logging operations in the environment manager.
        /// </summary>
        public ILogger<IEnvManager> Logger { get; }

        /// <summary>
        /// Retrieves the environment variable with the specified <paramref name="variableName"/> and converts it to the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The target <see cref="Type"/> to which the environment variable should be converted.</param>
        /// <param name="variableName">The name of the environment variable to retrieve.</param>
        /// <param name="raiseException">If <see langword="true"/>, throws an exception if the environment variable is not found; otherwise, returns <see langword="default"/>.</param>
        /// <returns>An object of the specified <paramref name="type"/>, or <see langword="default"/> if the variable is not found and <paramref name="raiseException"/> is <see langword="false"/>.</returns>
        public object Get(Type type, string variableName, bool raiseException = false);

        /// <summary>
        /// Retrieves the environment variable with the specified <paramref name="variableName"/> and converts it to the specified type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The target type to which the environment variable should be converted.</typeparam>
        /// <param name="variableName">The name of the environment variable to retrieve.</param>
        /// <param name="raiseException">If <see langword="true"/>, throws an exception if the environment variable is not found; otherwise, returns the <see langword="default"/> value of <typeparamref name="T"/>.</param>
        /// <returns>An object of type <typeparamref name="T"/>, or the <see langword="default"/> value of <typeparamref name="T"/> if the variable is not found and <paramref name="raiseException"/> is <see langword="false"/>.</returns>
        public T Get<T>(string variableName, bool raiseException = false);

        /// <summary>
        /// Retrieves the required environment variable with the specified <paramref name="variableName"/> and converts it to the specified <paramref name="type"/>.<br/>
        /// Throws an exception if the environment variable is not found.
        /// </summary>
        /// <param name="type">The target <see cref="Type"/> to which the environment variable should be converted.</param>
        /// <param name="variableName">The name of the environment variable to retrieve.</param>
        /// <returns>An object of the specified <paramref name="type"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the environment variable is not found.</exception>
        public object GetRequired(Type type, string variableName);

        /// <summary>
        /// Retrieves the required environment variable with the specified <paramref name="variableName"/> and converts it to the specified type <typeparamref name="T"/>.<br/>
        /// Throws an exception if the environment variable is not found.
        /// </summary>
        /// <typeparam name="T">The target type to which the environment variable should be converted.</typeparam>
        /// <param name="variableName">The name of the environment variable to retrieve.</param>
        /// <returns>An object of type <typeparamref name="T"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the environment variable is not found.</exception>
        public T GetRequired<T>(string variableName);
    }
}
