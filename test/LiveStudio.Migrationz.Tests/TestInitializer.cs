using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LiveStudio.Migrationz.Tests;

[SetUpFixture]
public class TestInitializer
{
    public static IServiceProvider ServiceProvider;
    public static IConfiguration Configuration;
    public static string TempConnection;
    public static string TempOriginConnection;

    [OneTimeSetUp]
    public void Init()
    {
        SetupDependencyInjection();
    }

    private void SetupDependencyInjection()
    {
        var config = Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Test.json", optional: false, reloadOnChange: true)
            .Build();
        
        var cn = TempConnection = GetTempDbConnection(config);
        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Test.json")
            .Build());
        services.AddServices(config);
        ServiceProvider = services.BuildServiceProvider();
    }
    
    private string GetTempDbConnection(IConfiguration configuration)
    {
        RemoveTestsFilesDbs();
        var dir = $"{TestContext.CurrentContext.TestDirectory}";
        var cn = @$"DataSource={dir}\TestArtifacts\Database\shared.db".Replace(".db", $"{DateTime.Now.Ticks}.db");
        return cn;
    }

    private void RemoveTestsFilesDbs()
    {
        string[] keyFiles = { "shared.db"};
        string[] keyDirs = { @"TestArtifacts\Database"};

        foreach (var keyDir in keyDirs)
        {
            DirectoryInfo di = new DirectoryInfo(keyDir);
            foreach (FileInfo file in di.GetFiles())
            {
                if (!keyFiles.Contains(file.Name))
                    file.Delete();
            }
        }
    }
}