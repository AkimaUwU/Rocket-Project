using Microsoft.Extensions.DependencyInjection;
using RocketPlanner.TimeClassifier.Data;

namespace RocketPlanner.TimeClassifier.DependencyInjection;

public static class TimeClassificationServices
{
    public static IServiceCollection AddTimeClassification(this IServiceCollection services)
    {
        services = services
            .AddSingleton<InMemoryTrainingData>()
            .AddSingleton<Classificator.TimeClassifier>();
        return services;
    }
}
