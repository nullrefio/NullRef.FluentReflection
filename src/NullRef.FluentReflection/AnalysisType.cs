using System.Collections.Generic;
using System.Linq;

namespace NullRef.FluentReflection
{
    public class AnalysisType
    {
        internal AnalysisType(AnalysisAssembly assemblyInfo)
        {
            Types = assemblyInfo.Assemblies.SelectMany(x => x.GetTypes());
        }

        internal IEnumerable<System.Type> Types { get; set; }
    }
}
