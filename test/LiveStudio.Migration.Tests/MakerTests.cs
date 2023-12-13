using LiveStudio.Migration.Schema;

namespace LiveStudio.Migration.Tests;

[TestFixture]
public class MakerTests
{
    private  IMaker? _composer;
    private Chamber? _chamber;

    [SetUp]
    public void SetUp()
    {
        _composer = new Maker();
        
        _chamber = new Chamber("Kit");
        _chamber.Add("Name",FieldType.String);
        _chamber.Add("Expiry",FieldType.Date);
    }
    [Test]
    public void should_Make()
    {
        var def =new Composer().ComposeDefinition(_chamber);

        var made = _composer.Make(def,_chamber.Name,TestContext.CurrentContext.TestDirectory);
        Assert.That(made.Type,Is.Not.Null);
        Console.WriteLine(made.Type);
        Console.WriteLine(made.File);
    }
}