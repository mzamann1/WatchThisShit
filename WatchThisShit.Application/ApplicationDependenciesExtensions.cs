using Microsoft.Extensions.DependencyInjection;
using WatchThisShit.Application.Database;
using WatchThisShit.Application.Repositories;

namespace WatchThisShit.Application;

public static class ApplicationDependenciesExtensions
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        services.AddSingleton<IMovieRepository, MovieRepository>();
        return services;
    }

    public static IServiceCollection AddDatabaseDependencies(this IServiceCollection services, string connectionString)
    {
        
        services.AddSingleton<IDbConnectionFactory>(_ => new NpgSQLConnectionFactory(connectionString));
        services.AddSingleton<DbInitializer>();
        return services;
    }
}