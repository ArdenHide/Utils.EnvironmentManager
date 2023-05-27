using System.Globalization;

namespace EnvironmentManager;

public static class EnvManager
{
    public static T GetEnvironmentValue<T>(string variableName, bool raiseException = false)
    {
        var envValue = Environment.GetEnvironmentVariable(variableName);

        if (string.IsNullOrEmpty(envValue))
        {
            HandleMissingEnvironmentVariable(variableName, raiseException);
            return default!;
        }

        return ConvertEnvironmentValue<T>(variableName, envValue, raiseException);
    }

    private static void HandleMissingEnvironmentVariable(string variableName, bool raiseException)
    {
        string errorMessage = $"Environment variable '{variableName}' is null or empty.";
        if (raiseException)
        {
            throw new InvalidOperationException(errorMessage);
        }
        else
        {
            Console.WriteLine(errorMessage);
        }
    }

    private static T ConvertEnvironmentValue<T>(string variableName, string envValue, bool raiseException)
    {
        try
        {
            return ConvertValue<T>(envValue);
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

    private static T ConvertValue<T>(string value)
    {
        Type targetType = typeof(T);

        if (targetType == typeof(DateTime))
        {
            return (T)(object)ParseDateTime(value);
        }

        if (IsNumericType(targetType))
        {
            return (T)Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture);
        }

        return (T)Convert.ChangeType(value, targetType);
    }

    private static DateTime ParseDateTime(string value)
    {
        if (DateTime.TryParse(value, out DateTime result) ||
            DateTime.TryParseExact(value, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
        {
            return result;
        }

        throw new FormatException($"Failed to parse DateTime value '{value}'.");
    }

    private static bool IsNumericType(Type type)
    {
        return type == typeof(decimal) || type == typeof(double) || type == typeof(float) ||
               type == typeof(int) || type == typeof(uint) || type == typeof(long) || type == typeof(ulong) ||
               type == typeof(short) || type == typeof(ushort) || type == typeof(byte) || type == typeof(sbyte);
    }
}
