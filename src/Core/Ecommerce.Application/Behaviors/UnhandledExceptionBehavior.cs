using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Application.Behaviors;

public class UnhandledExceptionBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TRequest> _logger;

    public UnhandledExceptionBehavior(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name; //Nombre de la clase
            _logger.LogError(ex, "Application reques: ocurrio una exception para el reques {@Name} {@Request}", requestName, request);
            throw new Exception("Application request with errors");
        }
    }
}