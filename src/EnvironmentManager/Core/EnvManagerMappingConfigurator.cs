using System;
using AutoMapper;
using System.Linq.Expressions;

namespace EnvironmentManager.Core
{
    public class EnvManagerMappingConfigurator
    {
        private readonly MapperConfigurationExpression _configExpression = new MapperConfigurationExpression();

        public EnvManagerMappingConfigurator()
        {
            DefaultConfigurationExpressions.DefaultConfiguration(_configExpression);
        }

        public EnvManagerMappingConfigurator CreateMapFor<T>(Expression<Func<string, T>> conversionExpression)
        {
            _configExpression.CreateMap<string, T>().ConvertUsing(conversionExpression);
            return this;
        }
        
        public MapperConfiguration Build() => new MapperConfiguration(_configExpression);
    }
}
