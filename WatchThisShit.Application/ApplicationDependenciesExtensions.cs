using Microsoft.Extensions.DependencyInjection;
using WatchThisShit.Application.Repositories;

namespace WatchThisShit.Application;

public static class ApplicationDependenciesExtensions
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        services.AddSingleton<IMovieRepository, MovieRepository>();
        return services;
    }
}