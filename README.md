![Project logo](https://raw.githubusercontent.com/ArdenHide/Utils.EnvironmentManager/main/logo/1000x1000.png)

# Utils.EnvironmentManager

[![CodeFactor](https://www.codefactor.io/repository/github/ardenhide/utils.environmentmanager/badge)](https://www.codefactor.io/repository/github/ardenhide/utils.environmentmanager)
[![NuGet version](https://badge.fury.io/nu/Utils.EnvironmentManager.svg)](https://badge.fury.io/nu/Utils.EnvironmentManager)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/ArdenHide/Utils.EnvironmentManager/blob/main/LICENSE)

---

Utils.EnvironmentManager is a C# library that provides robust management of environment variables with type conversion, logging, and mapping capabilities.
It leverages AutoMapper to seamlessly convert environment variable values to strongly-typed objects, making it easier to handle configuration settings across various environments.

> **Note**
> This documentation assumes a basic understanding of `AutoMapper` library.
> [AutoMapper docs](https://github.com/AutoMapper/AutoMapper/tree/v12.0.1#readme)

## Getting Started

## EnvManager Class (Instance-Based)
The `EnvManager` class implements `IEnvManager` and provides a concrete implementation for managing environment variables.

### Methods
```csharp
public object Get(Type type, string variableName, bool raiseException = false);
public T Get<T>(string variableName, bool raiseException = false);

public object GetRequired(Type type, string variableName);
public T GetRequired<T>(string variableName);
```

**Example: Basic Usage**

```csharp
using EnvironmentManager.Core;

public class Example
{
    public static void Main()
    {
        var envManager = new EnvManager();

        Environment.SetEnvironmentVariable("NUMBER", "131");
        int number = envManager.Get<int>("NUMBER");
        Console.WriteLine($"Number: {number}.");
    }
}
// The example displays the following output:
// Number: 131.
```

## EnvManager Class (Static)
For convenience, `Utils.EnvironmentManager` provides a static `EnvManager` class that can be used without instantiating an object.

### Methods
```csharp
public static object Get(Type type, string variableName, bool raiseException = false);
public static T Get<T>(string variableName, bool raiseException = false);

public static object GetRequired(Type type, string variableName);
public static T GetRequired<T>(string variableName);
```

**Example: Static Usage**
```csharp
using EnvironmentManager.Static;

public class Example
{
    public static void Main()
    {
        Environment.SetEnvironmentVariable("NUMBER", "131");
        int number = EnvManager.Get<int>("NUMBER");
        Console.WriteLine($"Number: {number}.");
    }
}
// The example displays the following output:
// Number: 131.
```

## Enum Support
The library also supports retrieving environment variables that are associated with enum values using custom attributes.

### Methods
```csharp
public static dynamic Get(this Enum key, IEnvManager? envManager = null)
public static object Get(this Enum key, Type type, IEnvManager? envManager = null)
public static T Get<T>(this Enum key, IEnvManager? envManager = null)

public static dynamic GetRequired(this Enum key, IEnvManager? envManager = null)
public static object GetRequired(this Enum key, Type type, IEnvManager? envManager = null)
public static T GetRequired<T>(this Enum key, IEnvManager? envManager = null)
```

**Example: Enum Support**
```csharp
using EnvironmentManager.Attributes;
using EnvironmentManager.Extensions;

public class Example
{
    public enum Env
    {
        [EnvironmentVariable(typeof(int), isRequired: true)]
        NUMBER
    }

    public static void Main()
    {
        Environment.SetEnvironmentVariable("NUMBER", "131");

        int number = Env.NUMBER.Get<int>();

        Console.WriteLine($"Number: {number}.");
    }
}
// The example displays the following output:
// Number: 131.
```

## Custom Mapping Configuration
You can customize how environment variables are mapped to specific types using the `EnvManagerMappingConfigurator`.

**Example: Custom Mapping Configuration**
```csharp
using EnvironmentManager.Configuration;

public class Example
{
    public static void Main()
    {
        Environment.SetEnvironmentVariable("INTEGER_ARRAY", "32, 6, 5, 23");

        var configuration = new EnvManagerMappingConfigurator()
            .CreateMapFor<int[]>(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray<int>()
            )
            .Build();

        var envManager = new Core.EnvManager(configuration);

        Static.EnvManager.Initialize(envManager);
        // Static.EnvManager.Initialize(configuration);    // Or can pass configuration directly

        // Now you can retrieve environment variables with custom mapping
        int[] integerArrayViaStatic = Static.EnvManager.Get<int[]>("INTEGER_ARRAY");
        int[] integerArrayViaInstance = envManager.Get<int[]>("INTEGER_ARRAY");

        Console.WriteLine($"Integer array via static manager: {string.Join(", ", integerArrayViaStatic)}.");
        Console.WriteLine($"Integer array via instance manager: {string.Join(", ", integerArrayViaInstance)}.");
    }
}
// The example displays the following output:
// Integer array via static manager: 32, 6, 5, 23.
// Integer array via instance manager: 32, 6, 5, 23.
```

## Logging
The library integrates with [Microsoft.Extensions.Logging](https://www.nuget.org/packages/Microsoft.Extensions.Logging/8.0.0#readme-body-tab) to provide logging for operations involving environment variables.
You can supply your own `ILogger<IEnvManager>` instance when initializing `EnvManager`.
If no logger is provided, a default instance of `NullLogger<EnvManager>` is used, which means no logging output will be produced.

**Example: Logging**
> **Note**: In this example using [Microsoft.Extensions.Logging.Console](https://www.nuget.org/packages/Microsoft.Extensions.Logging.Console/8.0.0#readme-body-tab) package.

```csharp
using EnvironmentManager.Core;
using Microsoft.Extensions.Logging;

public class Example
{
    public static void Main()
    {
        Environment.SetEnvironmentVariable("ENVIRONMENT", "environment");

        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<IEnvManager>();

        var envManager = new EnvManager(logger: logger);

        string environment = envManager.Get<string>("INVALID_ENVIRONMENT");
        if (!string.IsNullOrEmpty(environment))
        {
            Console.WriteLine($"Environment: {environment}");
        }
    }
}
// The example displays the following output:
// warn: EnvironmentManager.Core.IEnvManager[0]
//       Environment variable 'INVALID_ENVIRONMENT' is null or empty. Trying return default value.
```

**Example: Without logging**
If you wish to use the default logger (which won't produce any log output):

```csharp
var manager = new EnvManager();
```

### Logging Scenarios
Here are some situations where the `EnvManager` logs information:

1. Warning: If an environment variable is null or empty and the `raiseException` parameter is set to `false`, a warning log will be generated.
- Log Message: `"Environment variable '{VariableName}' is null or empty. Trying return default value"`

2. Error: If there's a failed conversion of an environment variable and the `raiseException` parameter is set to `false`, an error log will be created.
- Log Message: `"Failed to convert environment variable '{VariableName}' to type '{Type}'. Trying return default value."`

In both scenarios, the actual variable name and type (if applicable) will replace the placeholders {VariableName} and {Type}.
