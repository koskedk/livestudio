namespace LiveStudio.Migrationz.Schema;

public class Field
{
    public Guid Id { get;private set; }
    public string Name { get; private set; }
    public FieldType Type { get; private set; }
    public State State { get; private set; }
    public Guid ChamberId { get; private set; }
    public int Rank { get; private set; }
    
    private Field()
    {
        Id=Guid.NewGuid();
        State = State.Added;
    }
    public Field(string name, FieldType type, Guid chamberId,int rank)
        :this()
    {
        Name = name;
        Type = type;
        ChamberId = chamberId;
        Rank = rank;
    }
    public void ChangeName(string newName)
    {
        Name = newName;
        State = State.Changed;
    }
    public void ChangeType(FieldType newType)
    {
        Type = newType;
        State = State.Changed;
    }
}