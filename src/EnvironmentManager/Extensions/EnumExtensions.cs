using System;
using System.Reflection;
using EnvironmentManager.Attributes;

namespace EnvironmentManager.Extensions
{
    public static class EnumExtensions
    {
        public static dynamic Get(this Enum key, Core.EnvManager? envManager = null)
        {
            var attribute = GetAttribute(key);
            return GetInternal<object>(key, attribute?.Type ?? typeof(string), attribute?.IsRequired ?? false, envManager);
        }

        public static object Get(this Enum key, Type type, Core.EnvManager? envManager = null)
        {
            var attribute = GetAttribute(key);
            return GetInternal<object>(key, type, attribute?.IsRequired ?? false, envManager);
        }

        public static T Get<T>(this Enum key, Core.EnvManager? envManager = null)
        {
            var attribute = GetAttribute(key);
            return GetInternal<T>(key, typeof(T), attribute?.IsRequired ?? false, envManager);
        }

        public static dynamic GetRequired(this Enum key, Core.EnvManager? envManager = null)
        {
            var attribute = GetAttribute(key);
            return GetInternal<object>(key, attribute?.Type ?? typeof(string), true, envManager);
        }

        public static object GetRequired(this Enum key, Type type, Core.EnvManager? envManager = null)
        {
            return GetInternal<object>(key, type, true, envManager);
        }

        public static T GetRequired<T>(this Enum key, Core.EnvManager? envManager = null)
        {
            return GetInternal<T>(key, typeof(T), true, envManager);
        }

        public static T GetInternal<T>(this Enum key, Type type, bool raiseException = false, Core.EnvManager? envManager = null)
        {
            return envManager == null
                ? (T)Static.EnvManager.Get(type, key.ToString(), raiseException)
                : (T)envManager.Get(type, key.ToString(), raiseException);
        }

        private static EnvironmentVariableAttribute? GetAttribute(Enum key)
        {
            return key.GetType().GetField(key.ToString())?.GetCustomAttribute<EnvironmentVariableAttribute>();
        }
    }
}