namespace LiveStudio.Migration.Schema;

public class Chamber
{
    private readonly List<Field> _fields = new();
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public State State { get; private set; }
    public int Version { get; private set; }
    public IReadOnlyCollection<Field> Fields => _fields;

    private Chamber()
    {
        Id = Guid.NewGuid();
        State = State.Added;
        Version = 0;
        InitDefaultFields();
    }

    public Chamber(string name)
        :this()
    {
        Name = name;
    }

    private void InitDefaultFields()
    {
        if (_fields.Any())
            return;
        
        _fields.Add(new Field("Id", FieldType.Key, Id,1));
    }

    public void Add(string name, FieldType type)
    {
        if (FieldExists(name))
            throw new Exception($"Field {name} already exists");
        var rank = _fields.Max(x => x.Rank) + 1;
        _fields.Add(new Field(name, type, Id, rank));
    }

    public void UpdateTo(Guid id,string name, FieldType type)
    {
        int nextVersion = Version + 1;
        
        var field = _fields.FirstOrDefault(x => x.Id == id);
        
        if(field is null)
            return;
        
        if (!field.Name.ToLower().Equals(name.ToLower()))
        {
            if (FieldExists(name))
                throw new Exception($"Field {name} already exists");
            field.ChangeName(name);
            this.State = State.Changed;
            Version = nextVersion;
        }    
         
        if (!field.Type.Equals(type))
        {
            field.ChangeType(type);
            this.State = State.Changed;
            Version = nextVersion;
        }
    } 
    private bool FieldExists(string name)
    {
        return _fields.Any(x => x.Name.ToLower().Equals(name.ToLower().Trim()));
    }
}