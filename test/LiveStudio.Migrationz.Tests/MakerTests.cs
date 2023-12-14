using LiveStudio.Migrationz.Schema;
using Microsoft.Extensions.DependencyInjection;

namespace LiveStudio.Migrationz.Tests;

[TestFixture]
public class MakerTests
{
    private string _dir;
    private  IMaker? _maker;
    private Chamber? _chamber;
    private IComposer _composer;

    [SetUp]
    public void SetUp()
    {
        _maker =TestInitializer.ServiceProvider.GetRequiredService<IMaker>();
        _composer = TestInitializer.ServiceProvider.GetRequiredService<IComposer>();
        _chamber = new Chamber("HTS","Kit");
        _chamber.Add("Name",FieldType.String);
        _chamber.Add("Expiry",FieldType.Date);
        // " = \"TestArtifacts/Candidates\";";
    }
    
    [Test]
    public void should_Make()
    {
        var def =_composer.ComposeDefinition(_chamber);
        var made = _maker.Make(def,_chamber.AssemblyName,TestContext.CurrentContext.TestDirectory);
        Assert.That(made.Type,Is.Not.Null);
        Console.WriteLine(made.Type);
        Console.WriteLine(made.File);
    }
    
    [Test]
    public void should_Make_Mgs()
    {
        var def =_composer.ComposeMigration(_chamber);
        var made = _maker.Make(def,_chamber.MigrationName,TestContext.CurrentContext.TestDirectory);
        Assert.That(made.Type,Is.Not.Null);
        Console.WriteLine(made.Type);
        Console.WriteLine(made.File);
    }
}