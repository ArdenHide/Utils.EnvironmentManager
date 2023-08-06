using AutoMapper;
using System.Linq.Expressions;

namespace EnvironmentManager;

public class EnvManager
{
    private readonly IMapper mapper;

    public EnvManager()
        : this(CreateDefaultMapperConfiguration())
    { }

    public EnvManager(IConfigurationProvider mapperConfiguration)
    {
        mapper = new Mapper(mapperConfiguration);
    }

    public static MapperConfiguration CreateDefaultMapperConfiguration(params ConfigurationAction[] additionalConfigs)
    {
        return new MapperConfiguration(cfg =>
        {
            CreateMapFor(cfg, x => x);
            CreateMapFor(cfg, x => decimal.Parse(x));
            CreateMapFor(cfg, x => double.Parse(x));
            CreateMapFor(cfg, x => float.Parse(x));
            CreateMapFor(cfg, x => int.Parse(x));
            CreateMapFor(cfg, x => uint.Parse(x));
            CreateMapFor(cfg, x => long.Parse(x));
            CreateMapFor(cfg, x => ulong.Parse(x));
            CreateMapFor(cfg, x => short.Parse(x));
            CreateMapFor(cfg, x => ushort.Parse(x));
            CreateMapFor(cfg, x => byte.Parse(x));
            CreateMapFor(cfg, x => sbyte.Parse(x));

            foreach (var additionalConfig in additionalConfigs)
            {
                additionalConfig(cfg);
            }
        });
    }

    public static void CreateMapFor<T>(IProfileExpression cfg, Expression<Func<string, T>> conversionExpression) =>
        cfg.CreateMap<string, T>().ConvertUsing(conversionExpression);

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
