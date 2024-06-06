using System;

namespace EnvironmentManager.Extensions
{
    public static class EnumExtensions
    {
        private static readonly EnvManager _envManager = new EnvManager();

        public static TResponse GetEnvironmentValue<TResponse>(this Enum key, bool raiseException = false, EnvManager? envManager = null)
        {
            envManager ??= _envManager;
            return envManager.GetEnvironmentValue<TResponse>(key.ToString(), raiseException);
        }

        public static string GetEnvironmentValue(this Enum key, bool raiseException = false, EnvManager? envManager = null)
        {
            return key.GetEnvironmentValue<string>(raiseException, envManager);
        }
    }
}