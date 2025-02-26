using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using WatchThisShit.Application.Database;
using WatchThisShit.Application.Repositories;
using WatchThisShit.Application.Services;

namespace WatchThisShit.Application;

public static class ApplicationDependenciesExtensions
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<IMovieService, MovieService>();
        services.AddValidatorsFromAssemblyContaining<IApplicationMarker>();
        return services;
    }

    public static IServiceCollection AddDatabaseDependencies(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(_ => new NpgSQLConnectionFactory(connectionString));
        services.AddSingleton<DbInitializer>();
        return services;
    }
}