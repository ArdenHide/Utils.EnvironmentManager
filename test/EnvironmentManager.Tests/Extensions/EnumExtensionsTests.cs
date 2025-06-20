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

    public class GetOrDefault_String : TestData
    {
        private readonly string _defaultValue = "Bye World!";

        [Fact]
        public void WhenValidValue_ShouldReturnConvertedValue()
        {
            var stored = "Hello World!";
            Environment.SetEnvironmentVariable(TestEnum.STRING.ToString(), stored);

            var result = TestEnum.STRING.GetOrDefault(_defaultValue, EnvironmentManager);

            result.Should().BeEquivalentTo(stored);
        }

        [Fact]
        public void WhenNotFoundValue_ShouldReturnDefaultValue()
        {
            Environment.SetEnvironmentVariable(TestEnum.STRING.ToString(), null);

            var result = TestEnum.STRING.GetOrDefault(_defaultValue, EnvironmentManager);

            result.Should().BeEquivalentTo(_defaultValue);
        }
    }

    public class GetOrDefault_Typed : TestData
    {
        [Theory]
        [MemberData(nameof(EnumTestData))]
        [MemberData(nameof(ImplementedEnumTestData))]
        public void WhenValidValue_ShouldReturnConvertedValue<TOutput>(Enum enumMember, string stored, TOutput expected)
        {
            Environment.SetEnvironmentVariable(enumMember.ToString(), stored);

            var result = enumMember.GetOrDefault(typeof(TOutput), null!, EnvironmentManager);

            result.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [MemberData(nameof(EnumTestData))]
        [MemberData(nameof(ImplementedEnumTestData))]
        public void WhenNotFoundValue_ShouldReturnDefaultValue<TOutput>(Enum enumMember, string _, TOutput defaultValue)
        {
            Environment.SetEnvironmentVariable(enumMember.ToString(), null);

            var result = enumMember.GetOrDefault(typeof(TOutput), defaultValue!, EnvironmentManager);

            result.Should().BeEquivalentTo(defaultValue);
        }
    }

    public class GetOrDefault_Generic : TestData
    {
        [Theory]
        [MemberData(nameof(EnumTestData))]
        [MemberData(nameof(ImplementedEnumTestData))]
        public void WhenValidValue_ShouldReturnConvertedValue<TOutput>(Enum enumMember, string stored, TOutput expected)
        {
            Environment.SetEnvironmentVariable(enumMember.ToString(), stored);

            var result = enumMember.GetOrDefault<TOutput>(default!, EnvironmentManager);

            result.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [MemberData(nameof(EnumTestData))]
        [MemberData(nameof(ImplementedEnumTestData))]
        public void WhenNotFoundValue_ShouldReturnDefaultValue<TOutput>(Enum enumMember, string _, TOutput defaultValue)
        {
            Environment.SetEnvironmentVariable(enumMember.ToString(), null);

            var result = enumMember.GetOrDefault(typeof(TOutput), defaultValue!, EnvironmentManager);

            result.Should().BeEquivalentTo(defaultValue);
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