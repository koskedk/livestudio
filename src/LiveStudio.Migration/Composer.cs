using System.Text;
using LiveStudio.Migration.Schema;

namespace LiveStudio.Migration;

public interface IComposer
{
    string ComposeDefinition(Chamber chamber);
    string ComposeMigration(Chamber chamber);
}

public class Composer:IComposer
{
    public string Type { get; set; }
    public string ComposeDefinition(Chamber chamber)
    {
        var sb = new StringBuilder();
        sb.AppendLine("using System;");
     //   sb.AppendLine("using System.ComponentModel.DataAnnotations;");
        sb.AppendLine();
        sb.AppendLine($"public class {chamber.Name} {{");
        chamber.Fields.ToList().OrderBy(x=>x.Rank).ToList().ForEach(f =>
        {
            if (f.Type == FieldType.Key)
               // sb.AppendLine($"  [Key]");                
            sb.AppendLine($"  public {FieldMap()[f.Type]} {f.Name}  {{ get; set; }}");    
        });
        sb.AppendLine("}");
        return sb.ToString();
    }

    public string ComposeMigration(Chamber chamber)
    {
        throw new NotImplementedException();
    }
    
    public Dictionary<FieldType, string> FieldMap()
    {
        return new Dictionary<FieldType, string>
        {
            { FieldType.Key, "Guid" },
            { FieldType.Guid, "Guid?" },
            { FieldType.String, "string?" },
            { FieldType.Numeric, "decimal?" },
            { FieldType.Date, "DateTime?" },
            { FieldType.Boolean, "bool?" }
        };
    }
}
