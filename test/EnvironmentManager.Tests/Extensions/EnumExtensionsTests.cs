using Xunit;
using FluentAssertions;
using EnvironmentManager.Extensions;
using EnvironmentManager.Tests.TestHelpers;

namespace EnvironmentManager.Tests.Extensions;

public class EnumExtensionsTests
{
    public class Get_String : TestData
    {
        [Fact]
        public void WhenValidValue_ShouldReturnConvertedValue()
        {
            var stored = "Hello World!";
            Environment.SetEnvironmentVariable(TestEnum.STRING.ToString(), stored);

            var result = TestEnum.STRING.Get(EnvironmentManager);

            result.Should().BeEquivalentTo(stored);
        }
    }

    public class Get_Typed : TestData
    {
        [Theory]
        [MemberData(nameof(EnumTestData))]
        [MemberData(nameof(ImplementedEnumTestData))]
        public void WhenValidValue_ShouldReturnConvertedValue<TOutput>(Enum enumMember, string stored, TOutput expected)
        {
            Environment.SetEnvironmentVariable(enumMember.ToString(), stored);

            var result = enumMember.Get(typeof(TOutput), EnvironmentManager);

            result.Should().BeEquivalentTo(expected);
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

            var result = enumMember.Get<TOutput>(EnvironmentManager);

            result.Should().BeEquivalentTo(expected);
        }
    }

    public class GetRequired_String : TestData
    {
        [Fact]
        public void WhenValidValue_ShouldReturnConvertedValue()
        {
            var stored = "Hello World!";
            Environment.SetEnvironmentVariable(TestEnum.STRING.ToString(), stored);

            var result = TestEnum.STRING.GetRequired(EnvironmentManager);

            result.Should().BeEquivalentTo(stored);
        }
    }

    public class GetRequired_Typed : TestData
    {
        [Theory]
        [MemberData(nameof(EnumTestData))]
        [MemberData(nameof(ImplementedEnumTestData))]
        public void WhenValidValue_ShouldReturnConvertedValue<TOutput>(Enum enumMember, string stored, TOutput expected)
        {
            Environment.SetEnvironmentVariable(enumMember.ToString(), stored);

            var result = enumMember.GetRequired(typeof(TOutput), EnvironmentManager);

            result.Should().BeEquivalentTo(expected);
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

            var result = enumMember.GetRequired<TOutput>(EnvironmentManager);

            result.Should().BeEquivalentTo(expected);
        }
    }
}