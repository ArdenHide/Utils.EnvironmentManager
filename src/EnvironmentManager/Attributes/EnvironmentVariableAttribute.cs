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
        /// Gets or sets the Type to which the environment variable should be cast.<br/>
        /// The default type is <see cref="string"/>.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the environment variable is required.<br/>
        /// If required, an exception will be thrown when the environment variable is not found.
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvironmentVariableAttribute"/> class.
        /// </summary>
        /// <param name="type">The Type to which the environment variable should be cast. Defaults to <see cref="string"/> if not specified.</param>
        /// <param name="isRequired">Specifies whether the environment variable is required.</param>
        public EnvironmentVariableAttribute(Type? type = null, bool isRequired = false)
        {
            Type = type ?? typeof(string);
            IsRequired = isRequired;
        }
    }
}
