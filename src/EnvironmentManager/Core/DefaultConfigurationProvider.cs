using System;
using AutoMapper;
using System.Linq.Expressions;

namespace EnvironmentManager.Core
{
    public static class DefaultConfigurationProvider
    {
        public static readonly Action<IMapperConfigurationExpression> DefaultConfiguration = configExpression =>
        {
            CreateMapFor(configExpression, x => x);
            CreateMapFor(configExpression, x => decimal.Parse(x));
            CreateMapFor(configExpression, x => double.Parse(x));
            CreateMapFor(configExpression, x => float.Parse(x));
            CreateMapFor(configExpression, x => int.Parse(x));
            CreateMapFor(configExpression, x => uint.Parse(x));
            CreateMapFor(configExpression, x => long.Parse(x));
            CreateMapFor(configExpression, x => ulong.Parse(x));
            CreateMapFor(configExpression, x => short.Parse(x));
            CreateMapFor(configExpression, x => ushort.Parse(x));
            CreateMapFor(configExpression, x => byte.Parse(x));
            CreateMapFor(configExpression, x => sbyte.Parse(x));
        };

        private static void CreateMapFor<T>(IMapperConfigurationExpression configExpression, Expression<Func<string, T>> conversionExpression)
        {
            configExpression.CreateMap<string, T>().ConvertUsing(conversionExpression);
        }
    }
}
