# Utils.EnvironmentManager

![Project logo](logo/1500x500.png)

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
