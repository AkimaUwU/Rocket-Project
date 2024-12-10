using Microsoft.Extensions.DependencyInjection;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Contracts.Operations;

public sealed class QueryDispatcher : IQueryDispatcher
{
    private readonly IServiceScopeFactory _factory;

    public QueryDispatcher(IServiceScopeFactory factory) => _factory = factory;

    public async Task<Result<TQueryResult>> Resolve<TQuery, TQueryResult>(TQuery query)
        where TQuery : IQuery<TQueryResult>
    {
        using (var scope = _factory.CreateScope())
        {
            var handler = scope.ServiceProvider.GetRequiredService<
                IQueryHandler<TQuery, TQueryResult>
            >();
            return await handler.Handle(query);
        }
    }
}
