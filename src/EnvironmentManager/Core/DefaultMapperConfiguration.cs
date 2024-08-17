using AutoMapper;

namespace EnvironmentManager.Core
{
    public static class DefaultMapperConfiguration
    {
        public static readonly IConfigurationProvider DefaultConfiguration = new MapperConfiguration(
            configure: DefaultConfigurationExpressions.DefaultConfiguration
        );
    }
}
