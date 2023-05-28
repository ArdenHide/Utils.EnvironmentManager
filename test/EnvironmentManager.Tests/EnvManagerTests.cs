using Newtonsoft.Json.Linq;
using System.Globalization;
using Xunit;
using Xunit.Abstractions;

namespace EnvironmentManager.Tests;

public class EnvManagerTests
{
    private readonly ITestOutputHelper output;
    private const string EnvName = "VARIABLE";
    private static string EnvNotSetErrorMessage =>
        $"Environment variable '{EnvName}' is null or empty.";
    private static string ConvertErrorMessage(Type type) =>
        $"Failed to convert environment variable '{EnvName}' to type '{type}'.";

    public EnvManagerTests(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void GetEnvironmentValue_WithRaiseErrorEnvNotSet_ThrowException()
    {
        Environment.SetEnvironmentVariable(EnvName, "");

        Action testCode = () => EnvManager.GetEnvironmentValue<string>(EnvName, true);

        var exception = Assert.Throws<InvalidOperationException>(testCode);
        Assert.Equal(EnvNotSetErrorMessage, exception.Message);
    }

    [Theory]
    [InlineData('ñ')]
    [InlineData("some string")]
    [InlineData(123456789)]
    [InlineData(true)]
    public void GetEnvironmentValue_WithoutRaiseErrorEnvNotSet_DefaultValue<T>(T type)
    {
        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);
        Environment.SetEnvironmentVariable(EnvName, "");

        var result = EnvManager.GetEnvironmentValue<T>(EnvName);

        Assert.NotEqual(type, result);
        Assert.Equal(default, result);
        Assert.Contains(EnvNotSetErrorMessage, consoleOutput.ToString());
    }

    [Fact]
    public void GetEnvironmentValue_WithRaiseErrorImpossibleConvert_ThrowException()
    {
        Environment.SetEnvironmentVariable(EnvName, "123");

        Action testCode = () => EnvManager.GetEnvironmentValue<bool>(EnvName, true);

        var exception = Assert.Throws<InvalidCastException>(testCode);
        Assert.Equal(ConvertErrorMessage(typeof(bool)), exception.Message);
    }

    [Fact]
    public void GetEnvironmentValue_WithoutRaiseErrorImpossibleConvert_DefaultValue()
    {
        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);
        Environment.SetEnvironmentVariable(EnvName, "123");

        var result = EnvManager.GetEnvironmentValue<bool>(EnvName);

        Assert.Equal(default, result);
        Assert.Contains(ConvertErrorMessage(typeof(bool)), consoleOutput.ToString());
    }

    [Fact]
    public void GetEnvironmentValue_UnsupportedDateTimeFormat_ThrowException()
    {
        Environment.SetEnvironmentVariable(EnvName, "2023|05|28T19:53:00");

        Action testCode = () => EnvManager.GetEnvironmentValue<DateTime>(EnvName, true);

        var exception = Assert.Throws<InvalidCastException>(testCode);
        Assert.Equal(ConvertErrorMessage(typeof(DateTime)), exception.Message);
        var innerException = Assert.IsType<FormatException>(exception.InnerException);
        Assert.Equal($"Failed to parse DateTime value '2023|05|28T19:53:00'.", innerException.Message);
    }

    [Theory]
    [MemberData(nameof(Data))]
    public void GetEnvironmentValue_Types(string envValue, object expected, Type type)
    {
        Environment.SetEnvironmentVariable(EnvName, envValue);

        dynamic result = EnvManager.GetEnvironmentValue(type, EnvName);

        Assert.Equal(expected, (object)result);
        output.WriteLine(result.GetType().Name);
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
            new object[] { "true", true, typeof(bool) },
            new object[] { "2023-05-15T19:00:33", DateTime.Parse("2023-05-15T19:00:33"), typeof(DateTime) },
            new object[] { "2023/05/15 07:00:33 PM", DateTime.ParseExact("2023/05/15 07:00:33 PM", "yyyy/MM/dd hh:mm:ss tt", CultureInfo.InvariantCulture), typeof(DateTime) },
            new object[] { "15.05.2023 19:00:33", DateTime.ParseExact("15.05.2023 19:00:33", "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture), typeof(DateTime) }
        };
}
