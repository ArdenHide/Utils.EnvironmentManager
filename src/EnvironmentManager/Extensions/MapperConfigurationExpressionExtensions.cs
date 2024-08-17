using System;
using AutoMapper;
using System.Linq.Expressions;

namespace EnvironmentManager.Extensions
{
    public static class MapperConfigurationExpressionExtensions
    {
        public static void CreateMapFor<T>(this IMapperConfigurationExpression configExpression, Expression<Func<string, T>> conversionExpression)
        {
            configExpression.CreateMap<string, T>().ConvertUsing(conversionExpression);
        }
    }
}
