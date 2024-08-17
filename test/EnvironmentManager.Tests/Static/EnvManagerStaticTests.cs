using Xunit;
using EnvironmentManager.Static;
using EnvironmentManager.Tests.TestHelpers;

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

            RunTest(() => EnvManager.Get(typeof(TOutput), variableName), expected!);
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

            RunTest(() => EnvManager.Get<TOutput>(variableName), expected);
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

            RunTest(() => EnvManager.GetRequired(typeof(TOutput), variableName), expected!);
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

            RunTest(() => EnvManager.GetRequired<TOutput>(variableName), expected);
        }
    }
}