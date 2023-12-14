using System.Text;
using System.ComponentModel.DataAnnotations;
using LiveStudio.Migrationz.Schema;

namespace LiveStudio.Migrationz;

public interface IComposer
{
    string ComposeDefinition(Chamber chamber);
    string ComposeMigration(Chamber chamber);
}

public class Composer : IComposer
{
    public string ComposeDefinition(Chamber chamber)
    {
        var sb = new StringBuilder();
        sb.AppendLine("using System;");
        sb.AppendLine("using System.ComponentModel.DataAnnotations;");
        sb.AppendLine();
        sb.AppendLine($"public class {chamber.AssemblyName} {{");
        chamber.Fields.ToList().OrderBy(x => x.Rank).ToList().ForEach(f =>
        {
            if (f.Type == FieldType.Key)
                 sb.AppendLine($"  [Key]");
            sb.AppendLine($"  public {FieldMap()[f.Type]} {f.Name}  {{ get; set; }}");
        });
        sb.AppendLine("}");
        return sb.ToString();
    }

    public string ComposeMigration(Chamber chamber)
    {
        var sb = new StringBuilder();
        sb.AppendLine("using System;");
        sb.AppendLine("using System.ComponentModel.DataAnnotations;");
        sb.AppendLine("using FluentMigrator;");
        sb.AppendLine();
        sb.AppendLine($"[Migration({chamber.VersionDate:yyyyMMdd}{chamber.Version:0000})]");
        sb.AppendLine($"public class {chamber.MigrationName} : Migration");
        sb.AppendLine("{");
        sb.AppendLine($"{spc(4)}public override void Up()");
        sb.AppendLine($"{spc(4)}{{");
        sb.AppendLine($"{spc(8)}Create.Table(\"{chamber.AssemblyName}\")");
        chamber.Fields.ToList().OrderBy(x => x.Rank).ToList().ForEach(f =>
        {
            sb.AppendLine($"{spc(12)}{MigrationMap(f.Name)[f.Type]}");
        });
        sb.Append(";");
        sb.AppendLine($"{spc(4)}}}");
        sb.AppendLine($"{spc(4)}public override void Down()");
        sb.AppendLine($"{spc(4)}{{");
        sb.AppendLine($"{spc(8)}Delete.Table(\"{chamber.AssemblyName}\");");
        sb.AppendLine($"{spc(4)}}}");
        sb.AppendLine("}");
        return sb.ToString();
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

    public Dictionary<FieldType, string> MigrationMap(string col)
    {
        return new Dictionary<FieldType, string>
        {
            { FieldType.Key, $" .WithColumn(\"Id\").AsGuid().PrimaryKey()" },
            { FieldType.Numeric, $" .WithColumn(\"{col}\").AsDecimal().Nullable()" },
            { FieldType.Date, $" .WithColumn(\"{col}\").AsDateTime2().Nullable()" },
            { FieldType.Boolean, $" .WithColumn(\"{col}\").AsBoolean().Nullable()" },
            { FieldType.String, $" .WithColumn(\"{col}\").AsString().Nullable()" }
        };
    }
    
    private static string spc(int count) => new(' ',count);
}