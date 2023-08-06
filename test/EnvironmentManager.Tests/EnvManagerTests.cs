using Xunit;
using AutoMapper;
using Xunit.Abstractions;

namespace EnvironmentManager.Tests;

public class EnvManagerTests
{
    private readonly ITestOutputHelper output;
    private const string EnvName = "VARIABLE";
    private static string EnvNotSetErrorMessage => $"Environment variable '{EnvName}' is null or empty.";
    private static string ConvertErrorMessage(Type type) => $"Failed to convert environment variable '{EnvName}' to type '{type}'.";
    private static string ParseErrorMessage(Type targetType) =>
        $"Error mapping types.{Environment.NewLine}{Environment.NewLine}Mapping types:{Environment.NewLine}String -> {targetType.Name}{Environment.NewLine}System.String -> {targetType.FullName}";

    public EnvManagerTests(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void GetEnvironmentValue_WithRaiseErrorEnvNotSet_ThrowException()
    {
        Environment.SetEnvironmentVariable(EnvName, "");

        static void testCode() => EnvManager.CreateWithDefaultConfiguration().GetEnvironmentValue<string>(EnvName, true);

        var exception = Assert.Throws<InvalidOperationException>(testCode);
        Assert.Equal(EnvNotSetErrorMessage, exception.Message);
    }

    [Fact]
    public void GetEnvironmentValue_WithoutRaiseErrorEnvNotSet_DefaultValue()
    {
        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);
        Environment.SetEnvironmentVariable(EnvName, "");

        var result = EnvManager.CreateWithDefaultConfiguration().GetEnvironmentValue<char>(EnvName);

        Assert.Equal(default, result);
        Assert.Contains(EnvNotSetErrorMessage, consoleOutput.ToString());
    }

    [Fact]
    public void GetEnvironmentValue_WithRaiseErrorImpossibleConvert_ThrowException()
    {
        Environment.SetEnvironmentVariable(EnvName, "123");

        static void testCode() => EnvManager.CreateWithDefaultConfiguration().GetEnvironmentValue<bool>(EnvName, true);

        var exception = Assert.Throws<InvalidCastException>(testCode);
        Assert.Equal(ConvertErrorMessage(typeof(bool)), exception.Message);
    }

    [Fact]
    public void GetEnvironmentValue_WithoutRaiseErrorImpossibleConvert_DefaultValue()
    {
        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);
        Environment.SetEnvironmentVariable(EnvName, "123");

        var result = EnvManager.CreateWithDefaultConfiguration().GetEnvironmentValue<bool>(EnvName);

        Assert.Equal(default, result);
        Assert.Contains(ConvertErrorMessage(typeof(bool)), consoleOutput.ToString());
    }

    [Fact]
    public void GetEnvironmentValue_UnsupportedFormat_ThrowException()
    {
        Environment.SetEnvironmentVariable(EnvName, "19|53|00");

        static void testCode() => EnvManager.CreateWithDefaultConfiguration().GetEnvironmentValue<TimeSpan>(EnvName, true);

        var exception = Assert.Throws<InvalidCastException>(testCode);
        Assert.Equal(ConvertErrorMessage(typeof(TimeSpan)), exception.Message);

        var innerException = Assert.IsType<AutoMapperMappingException>(exception.InnerException);
        Assert.Equal(ParseErrorMessage(typeof(TimeSpan)), innerException.Message);
    }

    [Theory]
    [MemberData(nameof(Data))]
    public void GetEnvironmentValue_Types(string envValue, object expected, Type type)
    {
        Environment.SetEnvironmentVariable(EnvName, envValue);

        dynamic result = EnvManager.CreateWithDefaultConfiguration().GetEnvironmentValue(type, EnvName);

        Assert.Equal(expected, (object)result);
        output.WriteLine(result.GetType().Name);
    }

    [Fact]
    public void GetEnvironmentValue_AddNewFormat()
    {
        Environment.SetEnvironmentVariable(EnvName, "Value2");

        var mappingConfig = new EnvManagerMappingConfigurator()
            .CreateMapFor(x => Enum.Parse<MyEnumeration>(x, true))
            .Build();
        var envManager = new EnvManager(mappingConfig);

        var result = envManager.GetEnvironmentValue<MyEnumeration>(EnvName);

        Assert.Equal(MyEnumeration.Value2, result);
    }

    public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] { "ñ", 'ñ', typeof(char) },
            new object[] { "some string", "some string", typeof(string) },
            new object[] { "1.23456789", 1.23456789m, typeof(decimal) },
            new object[] { "1.23456789", 1.23456789d, typeof(double) },
            new object[] { "1.23456789", 1.23456789f, typeof(float) },
            new object[] { "2147483647", 2147483647, typeof(int) },
            new object[] { "4294967295", 4294967295u, typeof(uint) },
            new object[] { "9223372036854775807", 9223372036854775807L, typeof(long) },
            new object[] { "18446744073709551615", 18446744073709551615UL, typeof(ulong) },
            new object[] { "32767", (short)32767, typeof(short) },
            new object[] { "65535", (ushort)65535, typeof(ushort) },
            new object[] { "255", (byte)255, typeof(byte) },
            new object[] { "127", (sbyte)127, typeof(sbyte) },
            new object[] { "true", true, typeof(bool) }
        };

    internal enum MyEnumeration
    {
        Value1,
        Value2
    }
}
