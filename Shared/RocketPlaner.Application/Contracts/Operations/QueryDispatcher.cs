using Microsoft.Extensions.DependencyInjection;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Contracts.Operations;

public sealed class QueryDispatcher : IQueryDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public QueryDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<Result<TQueryResult>> Resolve<TQuery, TQueryResult>(TQuery query)
        where TQuery : IQuery<TQueryResult>
    {
        var handler = _serviceProvider.GetRequiredService<IQueryHandler<TQuery, TQueryResult>>();
        return await handler.Handle(query);
    }
}
