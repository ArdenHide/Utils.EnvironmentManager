using System;
using AutoMapper;
using System.Linq.Expressions;

namespace EnvironmentManager
{
    public class EnvManagerMappingConfigurator
    {
        private readonly MapperConfigurationExpression configExpression = new MapperConfigurationExpression();

        public EnvManagerMappingConfigurator()
        {
            AddDefaultMappings();
        }

        private void AddDefaultMappings()
        {
            CreateMapFor(x => x);
            CreateMapFor(x => decimal.Parse(x));
            CreateMapFor(x => double.Parse(x));
            CreateMapFor(x => float.Parse(x));
            CreateMapFor(x => int.Parse(x));
            CreateMapFor(x => uint.Parse(x));
            CreateMapFor(x => long.Parse(x));
            CreateMapFor(x => ulong.Parse(x));
            CreateMapFor(x => short.Parse(x));
            CreateMapFor(x => ushort.Parse(x));
            CreateMapFor(x => byte.Parse(x));
            CreateMapFor(x => sbyte.Parse(x));
        }

        public EnvManagerMappingConfigurator CreateMapFor<T>(Expression<Func<string, T>> conversionExpression)
        {
            configExpression.CreateMap<string, T>().ConvertUsing(conversionExpression);
            return this;
        }

        public MapperConfiguration Build() => new MapperConfiguration(configExpression);
    }
}