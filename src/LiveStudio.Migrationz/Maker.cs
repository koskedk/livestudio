using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Runtime.Loader;
using FluentMigrator.Builders.Create;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CSharp;
using Microsoft.Extensions.Options;

namespace LiveStudio.Migrationz;

public class Made
{
    public Type Type { get; set; }
    public string File { get; set; }
}

public interface IMaker
{
    Made Make(string definitionCode, string assemblyName, string path);
}

public class Maker : IMaker
{
    private readonly IOptions<LiveStudioOptions> _options;

    public Maker(IOptions<LiveStudioOptions> options)
    {
        _options = options;
    }

    public Made Make(string definitionCode, string assemblyName, string path)
    {
        var made = new Made();

        if (_options.Value.Candidates is not null)
            path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _options.Value.Candidates);
        
        var dll = Path.Combine(path, $"{assemblyName}.dll");

        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(definitionCode);
       
        var refPaths = new List<string>()
        {
            typeof(System.Object).GetTypeInfo().Assembly.Location,
            typeof(Console).GetTypeInfo().Assembly.Location,
            Path.Combine(Path.GetDirectoryName(typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly.Location),
                "System.Runtime.dll"),
            "netstandard.dll",
            typeof(System.ComponentModel.DataAnnotations.KeyAttribute).Assembly.Location,
            typeof(System.Runtime.AssemblyTargetedPatchBandAttribute).Assembly.Location,
            typeof(FluentMigrator.Migration).Assembly.Location,
            typeof(ICreateExpressionRoot).Assembly.Location
        };
        
        MetadataReference[] references = refPaths.Select(r => MetadataReference.CreateFromFile(r)).ToArray();

        CSharpCompilation compilation = CSharpCompilation.Create(
            assemblyName,
            new[] { syntaxTree },
            references: references,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        
        // Emit the compiled assembly
        EmitResult emitResult = compilation.Emit(dll);

        if (emitResult.Success)
        {
            Console.WriteLine("Compilation successful. DLL saved to: " + dll);
        }
        else
        {
            Console.WriteLine("Compilation failed:");
            foreach (var diagnostic in emitResult.Diagnostics)
            {
                Console.WriteLine(diagnostic);
            }
        }

        Assembly asmbly = AssemblyLoadContext.Default.LoadFromAssemblyPath(dll);
        made.Type = asmbly.GetType(assemblyName);
        made.File = dll;
        return made;
    }
}
