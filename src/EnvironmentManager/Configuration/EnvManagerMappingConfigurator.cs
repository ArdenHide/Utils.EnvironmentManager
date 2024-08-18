using System;
using AutoMapper;
using System.Linq.Expressions;
using EnvironmentManager.Extensions;

namespace EnvironmentManager.Configuration
{
    /// <summary>
    /// Provides a fluent API for configuring environment variable mappings using AutoMapper.
    /// </summary>
    public class EnvManagerMappingConfigurator
    {
        private readonly MapperConfigurationExpression _configExpression = new MapperConfigurationExpression();

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvManagerMappingConfigurator"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor initializes the configuration with the default mappings defined in <see cref="DefaultConfigurationExpressions.DefaultConfiguration"/>.
        /// </remarks>
        public EnvManagerMappingConfigurator()
        {
            DefaultConfigurationExpressions.DefaultConfiguration(_configExpression);
        }

        /// <summary>
        /// Creates a mapping from <see langword="string"/> to the specified type <typeparamref name="T"/> using the provided conversion expression.
        /// </summary>
        /// <typeparam name="T">The target type to which the environment variable should be mapped.</typeparam>
        /// <param name="conversionExpression">An expression defining how to convert a <see langword="string"/> to <typeparamref name="T"/>.</param>
        /// <returns>The current <see cref="EnvManagerMappingConfigurator"/> instance for fluent chaining.</returns>
        /// <remarks>
        /// Example how to configure parsing environment variable with following types: <see langword="TimeSpan"/>, <see langword="int"/> array and <see langword="string"/> array.
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
        public EnvManagerMappingConfigurator CreateMapFor<T>(Expression<Func<string, T>> conversionExpression)
        {
            _configExpression.CreateMapFor(conversionExpression);
            return this;
        }

        /// <summary>
        /// Builds and returns the <see cref="MapperConfiguration"/> based on the configured mappings.
        /// </summary>
        /// <returns>A <see cref="MapperConfiguration"/> instance containing the configured mappings.</returns>
        public MapperConfiguration Build() => new MapperConfiguration(_configExpression);
    }
}
