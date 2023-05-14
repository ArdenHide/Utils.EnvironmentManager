using Xunit;
using Xunit.Abstractions;

namespace EnvironmentManager.Tests;

public class EnvManagerTests
{
    private readonly ITestOutputHelper output;
    private const string EnvName = "VARIABLE";
    private string EnvNotSetErrorMessage =>
        $"Environment variable '{EnvName}' is null or empty.";
    private string ConvertErrorMessage(Type type) =>
        $"Failed to convert environment variable '{EnvName}' to type '{type}'.";

    public EnvManagerTests(ITestOutputHelper output)
    {
        this.output = output;
    }


    [Theory]
    [InlineData("ñ", 'ñ')]
    [InlineData("some string", "some string")]
    [InlineData("123456789", 123456789)]
    [InlineData("1,23456789", 1.23456789)]
    [InlineData("1,23456789", 1.23456789f)]
    [InlineData("true", true)]
    public void GetEnvironmentValue_Types_ExcpectedValue<T>(string envValue, T expected)
    {
        Environment.SetEnvironmentVariable(EnvName, envValue);

        var result = EnvManager.GetEnvironmentValue<T>(EnvName);

        Assert.NotNull(result);
        Assert.Equal(expected, result);
        output.WriteLine(result.GetType().Name);
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
    [InlineData(1.23456789)]
    [InlineData(1.23456789f)]
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
}
