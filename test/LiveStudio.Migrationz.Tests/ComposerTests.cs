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
        _chamber = new Chamber("HtsKitSchema", "HtsKit");
        _chamber.Add("SiteCode", FieldType.Numeric);
        _chamber.Add("FacilityName", FieldType.String);
        _chamber.Add("PatientId", FieldType.Numeric);
        _chamber.Add("Uuid", FieldType.String);
        _chamber.Add("DateCreated", FieldType.Date);
        _chamber.Add("DateLastModified", FieldType.Date);
        _chamber.Add("KitName", FieldType.String);
        _chamber.Add("LotNumber", FieldType.String);
        _chamber.Add("Expiry", FieldType.Date);
        _chamber.Add("Voided", FieldType.Boolean);
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