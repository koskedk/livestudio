using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CSharp;

namespace LiveStudio.Migration;

public class Made
{
    public Type Type { get; set; }
    public string File { get; set; }
}
public interface IMaker
{
    Made Make(string definition,string name,string path);
}

public class Maker : IMaker
{
    public Made Make(string definition, string name, string path)
    {
        var fp = Path.Combine(path, "Kit.dll");

        var made = new Made();

        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(definition);

        string assemblyName = Path.GetRandomFileName();
        var refPaths = new[]
        {
            typeof(System.Object).GetTypeInfo().Assembly.Location,
            typeof(Console).GetTypeInfo().Assembly.Location,
            Path.Combine(Path.GetDirectoryName(typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly.Location),
                "System.Runtime.dll")
        };
        MetadataReference[] references = refPaths.Select(r => MetadataReference.CreateFromFile(r)).ToArray();

        CSharpCompilation compilation = CSharpCompilation.Create(
            assemblyName,
            syntaxTrees: new[] { syntaxTree },
            references: references,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        using (var ms = new MemoryStream())
        {
            EmitResult result = compilation.Emit(ms);

            if (!result.Success)
            {
                IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                    diagnostic.IsWarningAsError ||
                    diagnostic.Severity == DiagnosticSeverity.Error);

                foreach (Diagnostic diagnostic in failures)
                {
                    Console.Error.WriteLine("\t{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                }
            }
            else
            {
                ms.Seek(0, SeekOrigin.Begin);

                Assembly assembly = AssemblyLoadContext.Default.LoadFromStream(ms);
                var type = assembly.GetType("Kit");

                // Save the DLL to a file if needed
                File.WriteAllBytes(fp, ms.ToArray());
                made.Type = type;
                made.File = fp;
            }
        }

        return made;
    }
}
