using EnvironmentManager.Attributes;
using Xunit;
using FluentAssertions;
using EnvironmentManager.Extensions;

namespace EnvironmentManager.Tests.Extensions;

public class EnumExtensionsTests
{
    internal enum Environments
    {
        RPC_URL,
        [EnvironmentVariable(typeof(int))]
        CHAIN_ID
    }

    internal static Dictionary<Environments, string> EnvironmentsValues => new()
    {
        { Environments.RPC_URL, "http://localhost:5050" },
        { Environments.CHAIN_ID, "97" }
    };

    public class GetEnvironmentValue
    {
        public GetEnvironmentValue()
        {
            EnvironmentsValues.ToList().ForEach(x => Environment.SetEnvironmentVariable(x.Key.ToString(), x.Value));
        }

        [Theory]
        [InlineData(Environments.RPC_URL, "http://localhost:5050")]
        [InlineData(Environments.CHAIN_ID, 97)]
        internal void WhenValueExist_WhenRaiseExceptionSetFalse(Environments environment, object expected)
        {
            var value = environment.GetEnvironmentValue();

            value.Should().Be(expected);
        }
    }
}