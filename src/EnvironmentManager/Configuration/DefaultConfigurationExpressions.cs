using System;
using AutoMapper;
using EnvironmentManager.Extensions;

namespace EnvironmentManager.Configuration
{
    /// <summary>
    /// Provides default configuration expressions for AutoMapper that map string environment variable values to common data types.
    /// </summary>
    public static class DefaultConfigurationExpressions
    {
        /// <summary>
        /// Gets the default AutoMapper configuration expression that creates mappings from strings to various primitive types.
        /// </summary>
        /// <remarks>
        /// This configuration maps environment variable strings to the following types:
        /// <list type="bullet">
        /// <item><see cref="string"/></item>
        /// <item><see cref="decimal"/></item>
        /// <item><see cref="double"/></item>
        /// <item><see cref="float"/></item>
        /// <item><see cref="int"/></item>
        /// <item><see cref="uint"/></item>
        /// <item><see cref="long"/></item>
        /// <item><see cref="ulong"/></item>
        /// <item><see cref="short"/></item>
        /// <item><see cref="ushort"/></item>
        /// <item><see cref="byte"/></item>
        /// <item><see cref="sbyte"/></item>
        /// <item><see cref="bool"/></item>
        /// </list>
        /// </remarks>
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
