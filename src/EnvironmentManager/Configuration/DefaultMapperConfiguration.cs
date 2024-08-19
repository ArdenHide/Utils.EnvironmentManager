using AutoMapper;

namespace EnvironmentManager.Configuration
{
    /// <summary>
    /// Provides the default AutoMapper configuration provider for mapping environment variables to various types.
    /// </summary>
    public static class DefaultMapperConfiguration
    {
        /// <summary>
        /// Gets the default AutoMapper configuration provider, which uses the configuration defined in <see cref="DefaultConfigurationExpressions.DefaultConfiguration"/>.
        /// </summary>
        /// <remarks>
        /// This configuration provider is used to map environment variable strings to common data types, as defined in the default configuration expressions.
        /// </remarks>
        public static readonly IConfigurationProvider DefaultConfiguration = new MapperConfiguration(
            configure: DefaultConfigurationExpressions.DefaultConfiguration
        );
    }
}
