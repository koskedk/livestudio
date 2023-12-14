using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LiveStudio.Migrationz;

public static class ConfigureServices
{
    public static IServiceCollection AddServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<LiveStudioOptions>(configuration.GetSection(LiveStudioOptions.LiveStudio));
        services.AddTransient<IComposer, Composer>();
        services.AddTransient<IMaker, Maker>();
        services.AddTransient<IMigrator, Migrator>();
        return services;
    }

    
    public static IServiceCollection AddMigrationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        
        var options = configuration
            .GetSection(LiveStudioOptions.LiveStudio)
            .Get<LiveStudioOptions>();
        
        services.AddFluentMigratorCore()
            .ConfigureRunner(rb =>
            {
                rb
                    .AddSQLite()
                    .WithGlobalConnectionString(options.LiveConnection)
                    .ScanIn(GetMgs(options.Candidates)).For.Migrations();
            })
            // Enable logging to console in the FluentMigrator way
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            // Build the service provider
            .BuildServiceProvider(false);
        return services;
    }
    private static Assembly[] GetMgs(string optionsCandidates)
    {
        List<Assembly> assemblies = new List<Assembly>();
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, optionsCandidates);
        string[] dllFiles = Directory.GetFiles(path, "*.dll");

        // Load each DLL as an assembly
        
        foreach (var dllFile in dllFiles)
        {
            assemblies.Add(Assembly.LoadFrom(dllFile));
        }

        return assemblies.ToArray();
    }
}