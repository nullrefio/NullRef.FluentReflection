using System.Collections.Generic;
using System.Linq;

namespace NullRef.FluentReflection;

public class AnalysisType
{
    internal AnalysisType(AnalysisAssembly assemblyInfo)
    {
        Types = assemblyInfo.Assemblies.SelectMany(x => x.GetTypes());
    }

    internal IEnumerable<System.Type> Types { get; set; }
}

internal class TypeMap
{
    public System.Type Type { get; set; }
    public bool IsConcrete { get; set; }
}
