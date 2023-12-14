using Microsoft.Extensions.DependencyInjection;

namespace LiveStudio.Migrationz.Tests;

[TestFixture]
public class MigratorTests
{
    private IMigrator _migrator;
    
    [SetUp]
    public void SetUp()
    {
        _migrator =TestInitializer.ServiceProvider.GetRequiredService<IMigrator>();
    }
    
    [Test]
    public void should_Run()
    {
        var res = _migrator.Run();
        Assert.That(res,Is.True);
    }
}