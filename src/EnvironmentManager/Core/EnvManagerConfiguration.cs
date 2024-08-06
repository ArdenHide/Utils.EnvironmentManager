using AutoMapper;
using Microsoft.Extensions.Logging;

namespace EnvironmentManager.Core
{
    public class EnvManagerConfiguration
    {
        private readonly IMapper _mapper;
        private readonly ILogger<EnvManager> _logger;
    }
}
