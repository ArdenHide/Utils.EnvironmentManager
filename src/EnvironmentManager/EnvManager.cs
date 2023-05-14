using System.Globalization;

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
            T convertedValue = ConvertValue<T>(envValue);
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

    private static T ConvertValue<T>(string value)
    {
        var targetType = typeof(T);

        if (targetType == typeof(DateTime))
        {
            return (T)(object)ParseDateTime(value);
        }
        else if (IsNumericType(targetType))
        {
            return (T)Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture);
        }
        else
        {
            return (T)Convert.ChangeType(value, targetType);
        }
    }

    private static DateTime ParseDateTime(string value)
    {
        if (DateTime.TryParse(value, out var result))
        {
            return result;
        }
        else if (DateTime.TryParseExact(value, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
        {
            return result;
        }
        else
        {
            throw new FormatException($"Failed to parse DateTime value '{value}'.");
        }
    }

    private static bool IsNumericType(Type type)
    {
        return type == typeof(decimal) || type == typeof(double) || type == typeof(float) ||
               type == typeof(int) || type == typeof(uint) || type == typeof(long) || type == typeof(ulong) ||
               type == typeof(short) || type == typeof(ushort) || type == typeof(byte) || type == typeof(sbyte);
    }
}
