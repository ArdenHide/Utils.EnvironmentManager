using Moq;
using AutoMapper;
using FluentAssertions;
using EnvironmentManager.Core;
using Microsoft.Extensions.Logging;
using EnvironmentManager.Attributes;
using EnvironmentManager.Configuration;

namespace EnvironmentManager.Tests.TestHelpers;

public class TestData
{
    protected static readonly Mock<ILogger<IEnvManager>> MockLogger = new();

    protected static readonly MapperConfiguration MapperConfiguration = new EnvManagerMappingConfigurator()
        .CreateMapFor<int[]>(x => x
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(num => int.Parse(num.Trim()))
            .ToArray<int>()
        )
        .CreateMapFor<string[]>(x => x
            .Split(new[] { ',' }, StringSplitOptions.TrimEntries)
            .ToArray<string>()
        )
        .Build();

    protected static readonly EnvManager EnvironmentManager = new(MapperConfiguration, MockLogger.Object);

    public enum TestEnum
    {
        [EnvironmentVariable(typeof(char), true)]
        CHAR,
        [EnvironmentVariable(typeof(string), true)]
        STRING,
        [EnvironmentVariable(typeof(decimal), true)]
        DECIMAL,
        [EnvironmentVariable(typeof(double), true)]
        DOUBLE,
        [EnvironmentVariable(typeof(float), true)]
        FLOAT,
        [EnvironmentVariable(typeof(int), true)]
        INT,
        [EnvironmentVariable(typeof(uint), true)]
        UINT,
        [EnvironmentVariable(typeof(long), true)]
        LONG,
        [EnvironmentVariable(typeof(ulong), true)]
        ULONG,
        [EnvironmentVariable(typeof(short), true)]
        SHORT,
        [EnvironmentVariable(typeof(ushort), true)]
        USHORT,
        [EnvironmentVariable(typeof(byte), true)]
        BYTE,
        [EnvironmentVariable(typeof(sbyte), true)]
        SBYTE,
        [EnvironmentVariable(typeof(bool), true)]
        BOOLEAN_TRUE,
        [EnvironmentVariable(typeof(bool), true)]
        BOOLEAN_true,
        [EnvironmentVariable(typeof(bool), true)]
        BOOLEAN_FALSE,
        [EnvironmentVariable(typeof(bool), true)]
        BOOLEAN_false,
        [EnvironmentVariable(typeof(TimeSpan), true)]
        TIME_SPAN,
        [EnvironmentVariable(typeof(TestImplementedEnum), true)]
        ENUM
    }

    public enum TestImplementedEnum
    {
        [EnvironmentVariable(typeof(int[]), true)]
        INTEGER_ARRAY,
        [EnvironmentVariable(typeof(string[]), true)]
        STRING_ARRAY
    }

    public static IList<object[]> ImplementedEnumTestData => CreateEnumTestData<TestImplementedEnum>(ImplementedTestData);
    public static IList<object[]> EnumTestData => CreateEnumTestData<TestEnum>(CommonTestData);

    private static IList<object[]> CreateEnumTestData<TEnum>(IList<object[]> commonData)
    {
        var enumData = new List<object[]>();
        var enumValues = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();

        for (var i = 0; i < commonData.Count; i++)
        {
            var stored = commonData[i][0] as string;
            var expected = commonData[i][1];
            var enumValue = enumValues[i];

            enumData.Add(new[] { enumValue!, stored!, expected });
        }

        return enumData;
    }

    public static IList<object[]> CommonTestData => new List<object[]>
    {
        BuildData("с", 'с'),
        BuildData("string", "string"),
        BuildData($"{decimal.MaxValue}", decimal.MaxValue),
        BuildData($"{double.MaxValue}", double.MaxValue),
        BuildData($"{float.MaxValue}", float.MaxValue),
        BuildData($"{int.MaxValue}", int.MaxValue),
        BuildData($"{uint.MaxValue}", uint.MaxValue),
        BuildData($"{long.MaxValue}", long.MaxValue),
        BuildData($"{ulong.MaxValue}", ulong.MaxValue),
        BuildData($"{short.MaxValue}", short.MaxValue),
        BuildData($"{ushort.MaxValue}", ushort.MaxValue),
        BuildData($"{byte.MaxValue}", byte.MaxValue),
        BuildData($"{sbyte.MaxValue}", sbyte.MaxValue),
        BuildData("TRUE", true),
        BuildData("true", true),
        BuildData("FALSE", false),
        BuildData("false", false),
        BuildData("60:00:00:00", new TimeSpan(days: 60, hours: 0, minutes: 0, seconds: 0)),
        BuildData("STRING_ARRAY", TestImplementedEnum.STRING_ARRAY)
    };

    public static IList<object[]> ImplementedTestData => new List<object[]>
    {
        BuildData("21, 4, 12, 61", new[] { 21, 4, 12, 61 }),
        BuildData("cat, tree, rome", new[] { "cat", "tree", "rome" })
    };

    protected static object[] BuildData<TOutput>(string stored, TOutput expected)
    {
        return new object[] { stored, expected! };
    }

    protected static void RunTest<TOutput>(Func<TOutput> methodToTest, TOutput expected)
    {
        var result = methodToTest.Invoke();
        result.Should().BeEquivalentTo(expected);
    }

    protected static string NameOfVariable<T>() => NameOfVariable(typeof(T));
    protected static string NameOfVariable(Type type) => $"{type.Name.ToUpper()}_VARIABLE_{Guid.NewGuid()}";
}