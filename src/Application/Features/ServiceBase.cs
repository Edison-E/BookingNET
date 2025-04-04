namespace BookPro.Application.Features;

public abstract class ServiceBase
{
    protected readonly IMapper _mapper;
    protected readonly ILogger _logger;
    protected readonly IManagerResourceLenguaje _managerLenguaje;

    public ServiceBase(IMapper mapper, ILogger logger, IManagerResourceLenguaje managerLenguaje)
    {
        _mapper = mapper;
        _logger = logger;
        _managerLenguaje = managerLenguaje;
    }

    protected void LogWarning(string message)
    {
        _logger.LogWarning("Warning: " + message);
    }

    protected void LogError(string message, Exception exception)
    {
        _logger.LogError(exception, "Error: " + message);
    }
}
