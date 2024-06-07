using System;

namespace EnvironmentManager.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EnvironmentVariableAttribute : Attribute
    {
        public Type Type { get; set; }
        public bool IsRequired { get; set; }

        public EnvironmentVariableAttribute(Type? type = null, bool isRequired = false)
        {
            Type = type ?? typeof(string);
            IsRequired = isRequired;
        }
    }
}
