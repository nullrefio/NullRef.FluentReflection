using System.Collections.Generic;
using System.Reflection;

namespace NullRef.FluentReflection
{
    public interface IAnalysisAssembly { }

    internal class AnalysisAssembly : IAnalysisAssembly
    {
        internal AnalysisAssembly() { }
        internal List<Assembly> Assemblies { get; } = new List<Assembly>();
    }
}
