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
    public void GetEnvironmentValue_Types_Char()
    {
        string envValue = "ñ";
        char expected = 'ñ';
        Environment.SetEnvironmentVariable(EnvName, envValue);

        var result = EnvManager.GetEnvironmentValue<char>(EnvName);

        Assert.Equal(expected, result);
        output.WriteLine(result.GetType().Name);
    }

    [Fact]
    public void GetEnvironmentValue_Types_String()
    {
        string envValue = "some string";
        string expected = "some string";
        Environment.SetEnvironmentVariable(EnvName, envValue);

        var result = EnvManager.GetEnvironmentValue<string>(EnvName);

        Assert.Equal(expected, result);
        output.WriteLine(result.GetType().Name);
    }

    [Fact]
    public void GetEnvironmentValue_Types_Int()
    {
        string envValue = "123456789";
        int expected = 123456789;
        Environment.SetEnvironmentVariable(EnvName, envValue);

        var result = EnvManager.GetEnvironmentValue<int>(EnvName);

        Assert.Equal(expected, result);
        output.WriteLine(result.GetType().Name);
    }

    [Fact]
    public void GetEnvironmentValue_Types_Double()
    {
        string envValue = "1,23456789";
        double expected = 1.23456789d;
        Environment.SetEnvironmentVariable(EnvName, envValue);

        var result = EnvManager.GetEnvironmentValue<double>(EnvName);

        Assert.Equal(expected, result);
        output.WriteLine(result.GetType().Name);
    }

    [Fact]
    public void GetEnvironmentValue_Types_Float()
    {
        string envValue = "1.23456789";
        float expected = 1.23456789f;
        Environment.SetEnvironmentVariable(EnvName, envValue);

        var result = EnvManager.GetEnvironmentValue<float>(EnvName);

        Assert.Equal(expected, result);
        output.WriteLine(result.GetType().Name);
    }

    [Fact]
    public void GetEnvironmentValue_Types_Decimal()
    {
        string envValue = "1,23456789";
        decimal expected = 1.23456789m;
        Environment.SetEnvironmentVariable(EnvName, envValue);

        var result = EnvManager.GetEnvironmentValue<decimal>(EnvName);

        Assert.Equal(expected, result);
        output.WriteLine(result.GetType().Name);
    }

    [Fact]
    public void GetEnvironmentValue_Types_Boolean()
    {
        string envValue = "true";
        bool expected = true;
        Environment.SetEnvironmentVariable(EnvName, envValue);

        var result = EnvManager.GetEnvironmentValue<bool>(EnvName);

        Assert.Equal(expected, result);
        output.WriteLine(result.GetType().Name);
    }

    [Fact]
    public void GetEnvironmentValue_Types_DateTime()
    {
        string envValue = DateTime.Parse("2023-05-15T19:00:33").ToString();
        DateTime expected = DateTime.Parse("2023-05-15T19:00:33");
        Environment.SetEnvironmentVariable(EnvName, envValue);

        var result = EnvManager.GetEnvironmentValue<DateTime>(EnvName);

        Assert.Equal(expected, result);
        output.WriteLine(result.GetType().Name);
    }
}
