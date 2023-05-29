using System.Globalization;

namespace EnvironmentManager;

public static class EnvManager
{
    private static List<string> customDateTimeFormats = new();

    public static void AddCustomDateTimeFormat(string format)
    {
        customDateTimeFormats.Add(format);
    }

    public static object GetEnvironmentValue(Type type, string variableName, bool raiseException = false)
    {
        var method = typeof(EnvManager).GetMethod(nameof(EnvManager.GetEnvironmentValue), new[] { typeof(string), typeof(bool) });
        var genericMethod = method!.MakeGenericMethod(type);
        return genericMethod.Invoke(null, new object[] { variableName, raiseException })!;
    }

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

        if (targetType.IsEnum)
        {
            return (T)Enum.Parse(targetType, value, ignoreCase: true);
        }

        if (targetType == typeof(TimeSpan))
        {
            return (T)(object)ParseTimeSpan(value);
        }

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
        var formats = new List<string>
        {
            "yyyy-MM-ddTHH:mm:ss",
            "dd.MM.yyyy HH:mm:ss",
            "MM/dd/yyyy HH:mm:ss",
            "dd/MM/yyyy HH:mm:ss",
            "yyyy-MM-dd",
            "dd.MM.yyyy",
            "MM/dd/yyyy",
            "dd/MM/yyyy",
            "HH:mm:ss",
            "yyyy-MM-dd HH:mm:ss.fffffff",
            "yyyy-MM-dd HH:mm:ss.FFF",
            "yyyy/MM/dd HH:mm:ss",
            "yyyyMMddHHmmss",
            "yyyy-MM-ddTHH:mm:ssZ",
            "yyyyMMdd"
        };
        formats.AddRange(customDateTimeFormats);

        if (DateTime.TryParse(value, out DateTime result) ||
            DateTime.TryParseExact(value, formats.ToArray(), CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
        {
            return result;
        }

        throw new FormatException($"Failed to parse DateTime value '{value}'.");
    }

    private static TimeSpan ParseTimeSpan(string value)
    {
        if (TimeSpan.TryParse(value, out TimeSpan result))
        {
            return result;
        }

        throw new FormatException($"Failed to parse TimeSpan value '{value}'.");
    }

    private static bool IsNumericType(Type type)
    {
        return type == typeof(decimal) || type == typeof(double) || type == typeof(float) ||
               type == typeof(int) || type == typeof(uint) || type == typeof(long) || type == typeof(ulong) ||
               type == typeof(short) || type == typeof(ushort) || type == typeof(byte) || type == typeof(sbyte);
    }
}
