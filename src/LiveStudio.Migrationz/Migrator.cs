using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace LiveStudio.Migrationz;

public interface IMigrator
{
    bool Run();
}

public class Migrator : IMigrator
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    
    public Migrator(IServiceProvider serviceProvider,IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    public bool Run()
    {
        var runner = GetRunner();
        runner.MigrateUp();
        return true;
    }

    private IMigrationRunner GetRunner()
    {
        var services = new ServiceCollection();
        services.AddMigrationServices(_configuration);
        return services.BuildServiceProvider().GetRequiredService<IMigrationRunner>();
    }
}