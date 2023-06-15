![Project logo](https://raw.githubusercontent.com/ArdenHide/Utils.EnvironmentManager/main/logo/1000x1000.png)

# Utils.EnvironmentManager

[![SonarCloud](https://sonarcloud.io/images/project_badges/sonarcloud-white.svg)](https://sonarcloud.io/summary/new_code?id=Utils.EnvironmentManager)

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Utils.EnvironmentManager&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Utils.EnvironmentManager)
[![SonarScanner for .NET 6](https://github.com/ArdenHide/Utils.EnvironmentManager/actions/workflows/dotnet.yml/badge.svg)](https://github.com/ArdenHide/Utils.EnvironmentManager/actions/workflows/dotnet.yml)
[![CodeFactor](https://www.codefactor.io/repository/github/ardenhide/utils.environmentmanager/badge)](https://www.codefactor.io/repository/github/ardenhide/utils.environmentmanager)

[![NuGet version](https://badge.fury.io/nu/Utils.EnvironmentManager.svg)](https://badge.fury.io/nu/Utils.EnvironmentManager)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/ArdenHide/Utils.EnvironmentManager/blob/main/LICENSE)

---

The `EnvironmentManager` namespace provides a static class `EnvManager` that allows you to easily retrieve environment variable values and handle conversions to the desired data type.

## EnvManager

The `EnvManager` class provides methods for retrieving environment variable values and handling conversions.

**Supported Types:**

- Primitive Types: `int`, `uint`, `long`, `ulong`, `short`, `ushort`, `byte`, `sbyte`, `decimal`, `double`, `float`, `bool`, `char`, `string`.
- Enum Types: Any enumeration type.
- DateTime Types: `DateTime`
- TimeSpan Types: `TimeSpan`

If the specified type is not supported, a `NotSupportedException` will be thrown.

**Method:** `GetEnvironmentValue`
```csharp
public static object GetEnvironmentValue(Type type, string variableName, bool raiseException = false)
```

This method retrieves the value of the specified environment variable and converts it to the specified `Type`.
The variable's value is fetched and then the appropriate type-specific `GetEnvironmentValue<T>` method is invoked dynamically using reflection.

This method is particularly useful when the type of the environment variable is determined at runtime.

**Parameters:**
- `type` (Type): The type to which the environment variable's value should be converted.
- `variableName` (string): The name of the environment variable.
- `raiseException` (bool, optional): Specifies whether to raise an exception when the environment variable is null or empty or when the conversion fails. Defaults to `false`.

**Returns:**
- `object`: The converted value of the environment variable.


**Method:** `GetEnvironmentValue<T>`
```csharp
public static T GetEnvironmentValue<T>(string variableName, bool raiseException = false)
```

This method retrieves the value of the specified environment variable and converts it to the specified type `T`. When working with decimal values, ensure that the decimal separator is a dot (.), not a comma (,). For example, use `1.23456789` instead of `1,23456789`.

**Parameters:**
- `variableName` (string): The name of the environment variable.
- `raiseException` (bool, optional): Specifies whether to raise an exception when the environment variable is null or empty or when the conversion fails. Defaults to `false`.

**Returns:**
- `T`: The converted value of the environment variable.

**Example:**

```csharp
// Retrieve the value of the "API_KEY" environment variable as a string
// Environment value: [Your API Key]
string apiKey = EnvManager.GetEnvironmentValue<string>("API_KEY");
Console.WriteLine($"API Key: {apiKey}");

// Retrieve the value of the "MAX_CONNECTIONS" environment variable as an integer
// Environment value: 123
int maxConnections = EnvManager.GetEnvironmentValue<int>("MAX_CONNECTIONS");
Console.WriteLine($"Max Connections: {maxConnections}");

// Retrieve the value of the "ENABLE_LOGGING" environment variable as a boolean
// Environment value: true
bool enableLogging = EnvManager.GetEnvironmentValue<bool>("ENABLE_LOGGING");
Console.WriteLine($"Enable Logging: {enableLogging}");

// Retrieve the value of the "PI_VALUE" environment variable as a decimal
// Environment value: 3.1415926535
decimal piValue = EnvManager.GetEnvironmentValue<decimal>("PI_VALUE");
Console.WriteLine($"Pi Value: {piValue}");

// Retrieve the value of the "DATE_FORMAT" environment variable as a DateTime
// Environment value: 2023-05-14T14:30:00
DateTime dateFormat = EnvManager.GetEnvironmentValue<DateTime>("DATE_FORMAT");
Console.WriteLine($"Date Format: {dateFormat.ToString("yyyy-MM-ddTHH:mm:ss")}");
```

In the example above, the `GetEnvironmentValue` method is used to retrieve the values of different environment variables. The method automatically handles conversions to the specified types (`string`, `int`, `bool`, `decimal`, and `DateTime`). If the environment variable is null or empty, the method either raises an exception (if `raiseException` is `true`) or returns the default value for the specified type.

For the `DateTime` conversion, the method supports various date and time formats. If the value of the environment variable is not in a valid format, a `FormatException` will be thrown. Make sure to set the appropriate date and time format for the `DATE_FORMAT` environment variable.

Note: If the conversion fails, the method either raises an exception (if `raiseException` is `true`) or returns the default value for the specified type.

**Method: `AddCustomDateTimeFormat`**

```csharp
public static void AddCustomDateTimeFormat(string format)
```

This method allows you to add a custom date and time format that the `GetEnvironmentValue<DateTime>` method will recognize when attempting to parse a `DateTime` from an environment variable's value.

The `DateTime` parsing process attempts to match the environment variable's value with the formats specified in the `formats` array, which includes a predefined set of common date and time formats.
By using the `AddCustomDateTimeFormat` method, you can add your own formats to this list.

**Parameters:**
- `format` (string): The custom date and time format to be added.

**Example:**

```csharp
// Add a custom date and time format
EnvManager.AddCustomDateTimeFormat("dd-MMM-yyyy HH:mm");

// Retrieve the value of the "CUSTOM_DATE_FORMAT" environment variable as a DateTime using the newly added format
// Environment value: 14-Jun-2023 14:30
object customDateFormat = EnvManager.GetEnvironmentValue<DateTime>("CUSTOM_DATE_FORMAT");
Console.WriteLine($"Custom Date Format: {customDateFormat.ToString()}");
```

In the example above, the `AddCustomDateTimeFormat` method is used to add a new date and time format.
Then the `GetEnvironmentValue` method is used to retrieve the value of the "CUSTOM_DATE_FORMAT" environment variable.
The method recognizes the newly added format and successfully converts the environment variable's value to a `DateTime` object.
