using System;

namespace EnvironmentManager.Extensions
{
    public static class EnumExtensions
    {
        private static readonly EnvManager envManager = new EnvManager();

        public static TResponse GetEnvironmentValue<TEnum, TResponse>(this TEnum key, bool raiseException = false)
            where TEnum : Enum
        {
            return envManager.GetEnvironmentValue<TResponse>(key.ToString(), raiseException);
        }
    }
}