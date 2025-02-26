using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using WatchThisShit.Application.Common.Interfaces;

namespace WatchThisShit.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger _logger;
    private readonly IUser _user;

    public LoggingBehaviour(ILogger<TRequest> logger, IUser user)
    {
        _logger = logger;
        _user = user;
    }

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        string? requestName = typeof(TRequest).Name;
        string? userName = string.Empty;


        _logger.LogInformation("WatchThisShit Request: {Name} {@UserName} {@Request}",
            requestName, userName, request);

        return Task.CompletedTask;
    }
}
