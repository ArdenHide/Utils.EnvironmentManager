using System;
using System.Linq;

namespace EnvironmentManager.Extensions
{
    internal class TypeExtensions
    {
        public static object GetDefaultValueOrThrow(Type type)
        {
            return GetDefaultValueOrThrowInternal<object>(type);
        }

        public static T GetDefaultValueOrThrow<T>()
        {
            return GetDefaultValueOrThrowInternal<T>(typeof(T));
        }

        private static T GetDefaultValueOrThrowInternal<T>(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            if (CanHaveDefaultValue(type)) return (T)Activator.CreateInstance(type);

            throw new InvalidOperationException($"Cannot return default value for type {type.FullName}");
        }

        public static bool CanHaveDefaultValue(Type type)
        {
            return type.IsValueType || (type is { IsClass: true, IsAbstract: false } && type.GetConstructors().Any(c => c.GetParameters().Length == 0));
        }
    }
}
