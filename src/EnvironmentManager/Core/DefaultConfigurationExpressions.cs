using System;
using AutoMapper;
using EnvironmentManager.Extensions;

namespace EnvironmentManager.Core
{
    public static class DefaultConfigurationExpressions
    {
        public static readonly Action<IMapperConfigurationExpression> DefaultConfiguration = configExpression =>
        {
            configExpression.CreateMapFor(x => x);
            configExpression.CreateMapFor(x => decimal.Parse(x));
            configExpression.CreateMapFor(x => double.Parse(x));
            configExpression.CreateMapFor(x => float.Parse(x));
            configExpression.CreateMapFor(x => int.Parse(x));
            configExpression.CreateMapFor(x => uint.Parse(x));
            configExpression.CreateMapFor(x => long.Parse(x));
            configExpression.CreateMapFor(x => ulong.Parse(x));
            configExpression.CreateMapFor(x => short.Parse(x));
            configExpression.CreateMapFor(x => ushort.Parse(x));
            configExpression.CreateMapFor(x => byte.Parse(x));
            configExpression.CreateMapFor(x => sbyte.Parse(x));
            configExpression.CreateMapFor(x => bool.Parse(x));
        };
    }
}
