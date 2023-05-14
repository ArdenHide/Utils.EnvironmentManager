namespace EnvironmentManager;

public static class EnvManager
{
    public static T GetEnvironmentValue<T>(string variableName, bool raiseException = false)
    {
        var envValue = Environment.GetEnvironmentVariable(variableName);

        if (string.IsNullOrEmpty(envValue))
        {
            if (raiseException)
            {
                throw new InvalidOperationException($"Environment variable '{variableName}' is null or empty.");
            }
            else
            {
                Console.WriteLine($"Environment variable '{variableName}' is null or empty.");
                return default!;
            }
        }

        try
        {
            T convertedValue = (T)Convert.ChangeType(envValue, typeof(T));
            return convertedValue;
        }
        catch (Exception ex)
        {
            if (raiseException)
            {
                throw new InvalidCastException($"Failed to convert environment variable '{variableName}' to type '{typeof(T)}'.", ex);
            }
            else
            {
                Console.WriteLine($"Failed to convert environment variable '{variableName}' to type '{typeof(T)}'. Returning default value.");
                return default!;
            }
        }
    }
}
