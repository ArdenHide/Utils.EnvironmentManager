using System;

namespace EnvironmentManager.Core
{
    public interface IEnvManager
    {
        public object Get(Type type, string variableName, bool raiseException = false);
        public T Get<T>(string variableName, bool raiseException = false);

        public object GetRequired(Type type, string variableName);
        public T GetRequired<T>(string variableName);
    }
}
