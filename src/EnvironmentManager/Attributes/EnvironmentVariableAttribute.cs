using System;

namespace EnvironmentManager.Attributes
{
    /// <summary>
    /// Defines metadata for environment variables associated with enum fields.<br/>
    /// This attribute specifies the type to which the environment variable should be cast and whether it is a required variable.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class EnvironmentVariableAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the <see cref="Type"/> to which the environment variable should be cast.<br/>
        /// The default type is <see cref="string"/>.
        /// </summary>
        /// <remarks>
        /// If no type is specified during initialization, the environment variable is treated as a string.
        /// </remarks>
        public Type Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the environment variable is required.<br/>
        /// If required, an exception will be thrown when the environment variable is not found.
        /// </summary>
        /// <remarks>
        /// By default, environment variables are considered optional (i.e., <see cref="IsRequired"/> is set to <see langword="false"/>).
        /// </remarks>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvironmentVariableAttribute"/> class.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to which the environment variable should be cast. Defaults to <see cref="string"/> if not specified.</param>
        /// <param name="isRequired">Specifies whether the environment variable is required. The default is <see langword="false"/>, meaning the variable is optional.</param>
        public EnvironmentVariableAttribute(Type? type = null, bool isRequired = false)
        {
            Type = type ?? typeof(string);
            IsRequired = isRequired;
        }
    }
}
