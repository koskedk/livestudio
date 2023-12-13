using LiveStudio.Migration.Schema;

namespace LiveStudio.Migration.Tests.Schema;

[TestFixture]
public class FieldTests
{
   [Test]
   public void should_Have_Id()
   {
       var test = new Field("Name",FieldType.String,Guid.NewGuid(),1);
       Assert.That(test.Id,Is.Not.EqualTo(default(Guid)));
       Assert.That(test.State,Is.EqualTo(State.Added));
   } 
  
   [Test]
   public void should_ChangeName()
   { 
       var test = new Field("Expiry",FieldType.Date,Guid.NewGuid(),1);
       
       test.ChangeName("ExpiryDate");
       
       Assert.That(test.State,Is.EqualTo(State.Changed));
       Assert.That(test.Name,Is.EqualTo("ExpiryDate"));
   }
   [Test]
   public void should_ChangeType()
   { 
       var test = new Field("Batch",FieldType.String,Guid.NewGuid(),1);
       
       test.ChangeType(FieldType.Numeric);
       
       Assert.That(test.State,Is.EqualTo(State.Changed));
       Assert.That(test.Type,Is.EqualTo(FieldType.Numeric));
   }
}