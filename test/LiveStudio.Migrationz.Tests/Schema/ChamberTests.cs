using LiveStudio.Migrationz.Schema;

namespace LiveStudio.Migrationz.Tests.Schema;

[TestFixture]
public class ChamberTests
{
    [Test]
    public void should_Have_Id()
    {
        var test = new Chamber("DEMO","Test");
        Assert.That(test.Id,Is.Not.EqualTo(default(Guid)));
        Assert.That(test.State,Is.EqualTo(State.Added));
        Assert.That(test.Version,Is.EqualTo(0));
        Assert.That(test.Fields.Count,Is.EqualTo(1));
    } 
    [Test]
    public void should_Add()
    {
        var testKit = new Chamber("DEMO","Kit");
        
        testKit.Add("Name",FieldType.String);
        testKit.Add("Expiry",FieldType.Date);
        
        Assert.That(testKit.State,Is.EqualTo(State.Added));
        Assert.That(testKit.Fields.Count,Is.EqualTo(3));
        Assert.That(testKit.Fields.All(x=>x.State==State.Added));
        Assert.That(testKit.Version,Is.EqualTo(0));
    } 
    [Test]
    public void should_UpdateTo()
    { 
        var testKit = new Chamber("DEMO","Kit");
        testKit.Add("Name",FieldType.String);
        testKit.Add("Expiry",FieldType.Date);

        var field = testKit.Fields.First(x => x.Name == "Expiry");
        testKit.UpdateTo(field.Id,"ExpiryDate",FieldType.Date);
        
        Assert.That(testKit.State,Is.EqualTo(State.Changed));
        Assert.That(testKit.Fields.Count(x=>x.State==State.Added),Is.EqualTo(2));
        Assert.That(testKit.Fields.Count(x=>x.State==State.Changed),Is.EqualTo(1));
        Assert.That(testKit.Version,Is.GreaterThan(0));
    }
}