using Xunit;
using FluentAssertions;
using EnvironmentManager.Core;
using EnvironmentManager.Tests.TestHelpers;
using Microsoft.Extensions.Logging.Abstractions;
using EnvManager = EnvironmentManager.Static.EnvManager;

namespace EnvironmentManager.Tests.Static;

public class EnvManagerStaticTests
{
    public class Get : TestData
    {
        [Theory]
        [MemberData(nameof(CommonTestData))]
        [MemberData(nameof(ImplementedTestData))]
        public void WhenValidValue_ShouldReturnConvertedValue<TOutput>(string stored, TOutput expected)
        {
            var variableName = NameOfVariable<TOutput>();
            Environment.SetEnvironmentVariable(variableName, stored);
            EnvManager.Initialize(MapperConfiguration);

            var result = EnvManager.Get(typeof(TOutput), variableName);

            result.Should().BeEquivalentTo(expected);
        }
    }

    public class Get_Generic : TestData
    {
        [Theory]
        [MemberData(nameof(CommonTestData))]
        [MemberData(nameof(ImplementedTestData))]
        public void WhenValidValue_ShouldReturnConvertedValue<TOutput>(string stored, TOutput expected)
        {
            var variableName = NameOfVariable<TOutput>();
            Environment.SetEnvironmentVariable(variableName, stored);
            EnvManager.Initialize(MapperConfiguration);

            var result = EnvManager.Get<TOutput>(variableName);

            result.Should().BeEquivalentTo(expected);
        }
    }

    public class GetRequired : TestData
    {
        [Theory]
        [MemberData(nameof(CommonTestData))]
        [MemberData(nameof(ImplementedTestData))]
        public void WhenValidValue_ShouldReturnConvertedValue<TOutput>(string stored, TOutput expected)
        {
            var variableName = NameOfVariable<TOutput>();
            Environment.SetEnvironmentVariable(variableName, stored);
            EnvManager.Initialize(MapperConfiguration);

            var result = EnvManager.GetRequired(typeof(TOutput), variableName);

            result.Should().BeEquivalentTo(expected);
        }
    }

    public class GetRequired_Generic : TestData
    {
        [Theory]
        [MemberData(nameof(CommonTestData))]
        [MemberData(nameof(ImplementedTestData))]
        public void WhenValidValue_ShouldReturnConvertedValue<TOutput>(string stored, TOutput expected)
        {
            var variableName = NameOfVariable<TOutput>();
            Environment.SetEnvironmentVariable(variableName, stored);
            EnvManager.Initialize(MapperConfiguration);

            var result = EnvManager.GetRequired<TOutput>(variableName);

            result.Should().BeEquivalentTo(expected);
        }
    }

    public class Initialize : TestData
    {
        [Fact]
        public void WithCustomMapperConfig_ShouldUseCustomMapperInManager()
        {
            EnvManager.Initialize(MapperConfiguration);

            EnvManager.Manager.Logger.Should().Be(NullLogger<IEnvManager>.Instance);
            EnvManager.Manager.Mapper.ConfigurationProvider.Should().BeEquivalentTo(MapperConfiguration);
        }

        [Fact]
        public void WithCustomManager_ShouldUseCustomMapperInManager()
        {
            EnvManager.Initialize(EnvironmentManager);

            EnvManager.Manager.Should().Be(EnvironmentManager);
        }
    }
}