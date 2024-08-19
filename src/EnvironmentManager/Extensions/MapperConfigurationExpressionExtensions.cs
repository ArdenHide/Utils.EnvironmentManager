using System;
using AutoMapper;
using System.Linq.Expressions;

namespace EnvironmentManager.Extensions
{
    /// <summary>
    /// Provides extension methods for configuring mappings in <see cref="IMapperConfigurationExpression"/>.
    /// </summary>
    public static class MapperConfigurationExpressionExtensions
    {
        /// <summary>
        /// Configures a mapping from <see langword="string"/> to the specified type <typeparamref name="T"/> using the provided conversion expression.
        /// </summary>
        /// <typeparam name="T">The target type to which the environment variable should be mapped.</typeparam>
        /// <param name="configExpression">The <see cref="IMapperConfigurationExpression"/> to configure.</param>
        /// <param name="conversionExpression">An expression defining how to convert a <see langword="string"/> to <typeparamref name="T"/>.</param>
        /// <remarks>
        /// Example how to configure parsing following types: <see langword="TimeSpan"/>, <see langword="int"/> array and <see langword="string"/> array.
        /// <code>
        /// var configurator = new EnvManagerMappingConfigurator()
        ///     .CreateMapFor&lt;TimeSpan&gt;(x => TimeSpan.Parse(x))
        ///     .CreateMapFor&lt;int[]&gt;(x => x
        ///         .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
        ///         .Select(num => int.Parse(num.Trim()))
        ///         .ToArray&lt;int&gt;()
        ///     )
        ///     .CreateMapFor&lt;string[]&gt;(x => x
        ///         .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
        ///         .ToArray&lt;string&gt;()
        ///     )
        ///     .Build()
        /// </code>
        /// </remarks>
        public static void CreateMapFor<T>(this IMapperConfigurationExpression configExpression, Expression<Func<string, T>> conversionExpression)
        {
            configExpression.CreateMap<string, T>().ConvertUsing(conversionExpression);
        }
    }
}
