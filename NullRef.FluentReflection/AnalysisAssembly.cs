using System.Collections.Generic;
using System.Reflection;

namespace NullRef.FluentReflection;

public class AnalysisAssembly
{
    internal AnalysisAssembly() { }
    internal List<Assembly> Assemblies { get; } = new();
}
