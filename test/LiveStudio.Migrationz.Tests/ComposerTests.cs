using LiveStudio.Migrationz.Schema;
using Microsoft.Extensions.DependencyInjection;

namespace LiveStudio.Migrationz.Tests;

[TestFixture]
public class ComposerTests
{
    private  IComposer? _composer;
    private Chamber? _chamber;

    [SetUp]
    public void SetUp()
    {
        _composer = TestInitializer.ServiceProvider.GetRequiredService<IComposer>();
        
        _chamber = new Chamber("DEMO","Kit");
        _chamber.Add("Name",FieldType.String);
        _chamber.Add("Expiry",FieldType.Date);
    }
    [Test]
    public void should_ComposeDefinition()
    {
        var def = _composer.ComposeDefinition(_chamber);
        Assert.That(def,Is.Not.Empty);
        Console.WriteLine(def);
    }

    [Test]
    public void should_ComposeMigration()
    {
        var def = _composer.ComposeMigration(_chamber);
        Assert.That(def,Is.Not.Empty);
        Console.WriteLine(def);
    }
}