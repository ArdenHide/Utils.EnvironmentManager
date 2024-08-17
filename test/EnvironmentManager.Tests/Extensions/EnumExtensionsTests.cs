using Xunit;
using AutoMapper;
using FluentAssertions;
using EnvironmentManager.Extensions;
using EnvironmentManager.Static;
using EnvironmentManager.Tests.TestHelpers;

namespace EnvironmentManager.Tests.Extensions;

public class EnumExtensionsTests
{
    public class Get : TestData
    {
        [Theory]
        [MemberData(nameof(EnumTestData))]
        [MemberData(nameof(ImplementedEnumTestData))]
        public void WhenValidValue_ShouldReturnConvertedValue<TOutput>(Enum enumMember, string stored, TOutput expected)
        {
            Environment.SetEnvironmentVariable(enumMember.ToString(), stored);

            RunTest(() => enumMember.Get(typeof(TOutput), EnvironmentManager), expected!);
        }
    }

    public class Get_Dynamic : TestData
    {
        [Theory]
        [MemberData(nameof(EnumTestData))]
        [MemberData(nameof(ImplementedEnumTestData))]
        public void WhenValidValue_ShouldReturnConvertedValue<TOutput>(Enum enumMember, string stored, TOutput expected)
        {
            Environment.SetEnvironmentVariable(enumMember.ToString(), stored);

            RunTest(() => (TOutput)enumMember.Get(EnvironmentManager), expected!);
        }
    }

    public class Get_Generic : TestData
    {
        [Theory]
        [MemberData(nameof(EnumTestData))]
        [MemberData(nameof(ImplementedEnumTestData))]
        public void WhenValidValue_ShouldReturnConvertedValue<TOutput>(Enum enumMember, string stored, TOutput expected)
        {
            Environment.SetEnvironmentVariable(enumMember.ToString(), stored);

            RunTest(() => enumMember.Get<TOutput>(EnvironmentManager), expected);
        }
    }

    public class GetRequired : TestData
    {
        [Theory]
        [MemberData(nameof(EnumTestData))]
        [MemberData(nameof(ImplementedEnumTestData))]
        public void WhenValidValue_ShouldReturnConvertedValue<TOutput>(Enum enumMember, string stored, TOutput expected)
        {
            Environment.SetEnvironmentVariable(enumMember.ToString(), stored);

            RunTest(() => enumMember.GetRequired(typeof(TOutput), EnvironmentManager), expected!);
        }
    }

    public class GetRequired_Dynamic : TestData
    {
        [Theory]
        [MemberData(nameof(EnumTestData))]
        [MemberData(nameof(ImplementedEnumTestData))]
        public void WhenValidValue_ShouldReturnConvertedValue<TOutput>(Enum enumMember, string stored, TOutput expected)
        {
            Environment.SetEnvironmentVariable(enumMember.ToString(), stored);

            RunTest(() => enumMember.GetRequired(EnvironmentManager), expected!);
        }
    }

    public class GetRequired_Generic : TestData
    {
        [Theory]
        [MemberData(nameof(EnumTestData))]
        [MemberData(nameof(ImplementedEnumTestData))]
        public void WhenValidValue_ShouldReturnConvertedValue<TOutput>(Enum enumMember, string stored, TOutput expected)
        {
            Environment.SetEnvironmentVariable(enumMember.ToString(), stored);

            RunTest(() => enumMember.GetRequired<TOutput>(EnvironmentManager), expected);
        }
    }

    public class GetInternal : TestData
    {
        [Theory]
        [MemberData(nameof(ImplementedEnumTestData))]
        public void WhenUsedDefaultEnvManager_ShouldThrowCauseDefaultNotImplementPassedTestData<TOutput>(Enum enumMember, string stored, TOutput _)
        {
            Environment.SetEnvironmentVariable(enumMember.ToString(), stored);

            var testCode = () =>
            {
                EnvManager.Initialize(null, null);
                return enumMember.GetInternal<TOutput>(typeof(TOutput), raiseException: true, envManager: null);
            };

            testCode.Should().Throw<InvalidCastException>()
                .WithMessage($"Failed to convert environment variable '{enumMember}' to type '{typeof(TOutput)}'.")
                .WithInnerException<AutoMapperMappingException>()
                .WithMessage($@"Missing type map configuration or unsupported mapping.

Mapping types:
{nameof(String)} -> {typeof(TOutput).Name}
{typeof(string).FullName} -> {typeof(TOutput).FullName}");
        }

        [Theory]
        [MemberData(nameof(EnumTestData))]
        [MemberData(nameof(ImplementedEnumTestData))]
        public void WhenUsedCustomEnvManager_ShouldSuccessfullyParseWithCustomManager<TOutput>(Enum enumMember, string stored, TOutput expected)
        {
            Environment.SetEnvironmentVariable(enumMember.ToString(), stored);

            RunTest(() => enumMember.GetInternal<TOutput>(typeof(TOutput), raiseException: true, envManager: EnvironmentManager), expected!);
        }
    }
}