using System;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace EnvironmentManager.Core
{
    public interface IEnvManager
    {
        public IMapper Mapper { get; }
        public ILogger<IEnvManager> Logger { get; }

        public object Get(Type type, string variableName, bool raiseException = false);
        public T Get<T>(string variableName, bool raiseException = false);

        public object GetRequired(Type type, string variableName);
        public T GetRequired<T>(string variableName);
    }
}
