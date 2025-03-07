using Microsoft.Extensions.DependencyInjection;
using ReportTaskPlanner.UseCases.Abstractions.Handlers;
using ReportTaskPlanner.Utilities.ResultPattern;

namespace ReportTaskPlanner.DependencyInjection;

public sealed class RequestDispatcher
{
    private readonly IServiceScopeFactory _factory;

    public RequestDispatcher(IServiceScopeFactory factory) => _factory = factory;

    public async Task<Result> Dispatch<TRequest>(TRequest request, CancellationToken ct = default)
        where TRequest : class, IRequest
    {
        using IServiceScope scope = _factory.CreateScope();
        IServiceProvider provider = scope.ServiceProvider;
        IHandler<TRequest> handler = provider.GetRequiredService<IHandler<TRequest>>();
        return await handler.Handle(request, ct);
    }

    public async Task<Result<TResult>> Dispatch<TRequest, TResult>(
        TRequest request,
        CancellationToken ct = default
    )
        where TRequest : class, IRequest<TResult>
    {
        using IServiceScope scope = _factory.CreateScope();
        IServiceProvider provider = scope.ServiceProvider;
        IHandler<TRequest, TResult> handler = provider.GetRequiredService<
            IHandler<TRequest, TResult>
        >();
        return await handler.Handle(request, ct);
    }
}
