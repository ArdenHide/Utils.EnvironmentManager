![Project logo](https://raw.githubusercontent.com/ArdenHide/Utils.EnvironmentManager/main/logo/1000x1000.png)

# Utils.EnvironmentManager

[![SonarCloud](https://sonarcloud.io/images/project_badges/sonarcloud-white.svg)](https://sonarcloud.io/summary/new_code?id=Utils.EnvironmentManager)

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Utils.EnvironmentManager&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Utils.EnvironmentManager)
[![SonarScanner for .NET 6](https://github.com/ArdenHide/Utils.EnvironmentManager/actions/workflows/dotnet.yml/badge.svg)](https://github.com/ArdenHide/Utils.EnvironmentManager/actions/workflows/dotnet.yml)
[![CodeFactor](https://www.codefactor.io/repository/github/ardenhide/utils.environmentmanager/badge)](https://www.codefactor.io/repository/github/ardenhide/utils.environmentmanager)

[![NuGet version](https://badge.fury.io/nu/Utils.EnvironmentManager.svg)](https://badge.fury.io/nu/Utils.EnvironmentManager)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/ArdenHide/Utils.EnvironmentManager/blob/main/LICENSE)

---

The `EnvironmentManager` namespace now provides a class `EnvManager` that uses `AutoMapper` package, for retrieving environment variable values and performing type conversions.

> **Note**
> This documentation assumes a basic understanding of `AutoMapper` library.
> [AutoMapper docs](https://github.com/AutoMapper/AutoMapper/tree/v12.0.1#readme)

## Initialization

`EnvManager` initialization can be achieved with or without a custom `AutoMapper` configuration:

1. Without a custom configuration:
```csharp
var manager = new EnvManager();
```

2. With a custom configuration:
```csharp
var manager = new EnvManager(config: config);
```

## Methods

### GetEnvironmentValue
The method retrieves the value of the specified environment variable and converts it to the desired type using `AutoMapper`.

**Signature:**
```csharp
public object GetEnvironmentValue(Type type, string variableName, bool raiseException = false)
```

**Parameters:**
- `type` (Type): The type to which the environment variable's value should be converted.
- `variableName` (string): The name of the environment variable.
- `raiseException` (bool, optional): Specifies whether to raise an exception if the environment variable is null or empty, or when the conversion fails. Defaults to false.

**Returns:**
- `object`: The converted value of the environment variable.

### GetEnvironmentValue&lt;T&gt;
This method retrieves the value of the specified environment variable and converts it to the specified type `T`.

**Signature:**
```csharp
public T GetEnvironmentValue<T>(string variableName, bool raiseException = false)
```

**Parameters:**
- `variableName` (string): The name of the environment variable.
- `raiseException` (bool, optional): Specifies whether to raise an exception if the environment variable is null or empty, or when the conversion fails. Defaults to false.

**Returns:**
- `T`: The converted value of the environment variable.

## Adding Custom Mappings
The library now uses `AutoMapper` for type conversions.
Therefore, to add custom type conversions, you can utilize the `EnvManagerMappingConfigurator` class.

**Example:**

```csharp
var config = new EnvManagerMappingConfigurator()
    .CreateMapFor(x => DateTime.ParseExact(x, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture))
    .CreateMapFor(x => Enum.Parse<MyEnumeration>(x, true))
    .Build();

var manager = new EnvManager(config: config);

DateTime customDateFormat = manager.GetEnvironmentValue<DateTime>("CUSTOM_DATE_FORMAT");
```

In this example, a custom date format is added using the `CreateMapFor` method.
Also in this example adding mapping for a `MyEnumeration` enum.
Once the custom mappings are added, the configuration is built and passed to the `EnvManager`.

## Logging

`EnvManager` incorporates logging through the Microsoft's ILogger interface, providing insights into the operations and potential issues while working with environment variables.

### Logger Initialization

You can pass an instance of `ILogger<EnvManager>` when creating the `EnvManager`.
If no logger is provided, a default instance of `NullLogger<EnvManager>` is used, which means no logging output will be produced.

Example:
```csharp
var logger = new LoggerFactory().CreateLogger<EnvManager>();
var manager = new EnvManager(logger: logger);
```

If you wish to use the default logger (which won't produce any log output):
```csharp
var manager = new EnvManager();
```

### Logging Scenarios

Here are some situations where the `EnvManager` logs information:

1. Warning: If an environment variable is null or empty and the `raiseException` parameter is set to `false`, a warning log will be generated.
- Log Message: `"Environment variable '{VariableName}' is null or empty."`

2. Error: If there's a failed conversion of an environment variable and the `raiseException` parameter is set to `false`, an error log will be created.
- Log Message: `"Failed to convert environment variable '{VariableName}' to type '{Type}'. Returning default value."`

In both scenarios, the actual variable name and type (if applicable) will replace the placeholders {VariableName} and {Type}.
