using LiveStudio.Migration.Schema;

namespace LiveStudio.Migration.Tests;

[TestFixture]
public class ComposerTests
{
    private  IComposer? _composer;
    private Chamber? _chamber;

    [SetUp]
    public void SetUp()
    {
        _composer = new Composer();
        
        _chamber = new Chamber("Kit");
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
        Assert.That(_composer.ComposeMigration(_chamber),Is.Not.Empty);
    }
}