using AutoMapper;

namespace EnvironmentManager;

public class EnvManager
{
    private readonly IMapper mapper;

    public EnvManager(IConfigurationProvider config)
    {
        mapper = new Mapper(config);
    }

    public static EnvManager CreateWithDefaultConfiguration() =>
        new(new EnvManagerMappingConfigurator().Build());

    public object GetEnvironmentValue(Type type, string variableName, bool raiseException = false)
    {
        var method = typeof(EnvManager).GetMethod(nameof(GetEnvironmentValue), new[] { typeof(string), typeof(bool) });
        var genericMethod = method!.MakeGenericMethod(type);
        return genericMethod.Invoke(this, new object[] { variableName, raiseException })!;
    }

    public T GetEnvironmentValue<T>(string variableName, bool raiseException = false)
    {
        var envValue = Environment.GetEnvironmentVariable(variableName);

        if (!string.IsNullOrEmpty(envValue))
        {
            return ConvertEnvironmentValue<T>(variableName, envValue, raiseException);
        }

        HandleMissingEnvironmentVariable(variableName, raiseException);
        return default!;

    }

    private static void HandleMissingEnvironmentVariable(string variableName, bool raiseException)
    {
        var errorMessage = $"Environment variable '{variableName}' is null or empty.";
        if (raiseException)
        {
            throw new InvalidOperationException(errorMessage);
        }

        Console.WriteLine(errorMessage);
    }

    private T ConvertEnvironmentValue<T>(string variableName, string envValue, bool raiseException)
    {
        try
        {
            return mapper.Map<string, T>(envValue);
        }
        catch (Exception ex)
        {
            if (raiseException)
            {
                throw new InvalidCastException($"Failed to convert environment variable '{variableName}' to type '{typeof(T)}'.", ex);
            }

            Console.WriteLine($"Failed to convert environment variable '{variableName}' to type '{typeof(T)}'. Returning default value.");
            return default!;
        }
    }
}
