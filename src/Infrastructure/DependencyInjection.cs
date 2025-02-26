using Microsoft.Extensions.Hosting;
using WatchThisShit.Application.Common.Interfaces;
using WatchThisShit.Application.Common.Interfaces.Repositories;
using WatchThisShit.Application.Common.Interfaces.Services;
using WatchThisShit.Infrastructure.Data;
using WatchThisShit.Infrastructure.Repositories;
using WatchThisShit.Infrastructure.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        string? connectionString = builder.Configuration["Database:ConnectionString"];
        Guard.Against.Null(connectionString, message: "Connection string 'WatchThisShitDb' not found.");

        builder.Services.AddScoped<IMovieRepository, MovieRepository>();
        builder.Services.AddScoped<IMovieService, MovieService>();
        builder.Services.AddSingleton<IDbConnectionFactory>(_ => new NpgSQLConnectionFactory(connectionString));

        builder.Services.AddSingleton<ApplicationDbContextInitialiser>();
        builder.Services.AddSingleton(TimeProvider.System);
    }
}
