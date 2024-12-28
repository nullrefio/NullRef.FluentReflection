using System.Collections.Generic;
using System.Linq;

namespace NullRef.FluentReflection
{
    public interface IAnalysisType { }

    internal class AnalysisType : IAnalysisType
    {
        internal AnalysisType(AnalysisAssembly assemblyInfo)
        {
            Types = assemblyInfo.Assemblies.SelectMany(x => x.GetTypes());
        }

        internal AnalysisType(IEnumerable<System.Type> types)
        {
            Types = types;
        }

        public static AnalysisType Create(IEnumerable<System.Type> types) => new AnalysisType(types);

        internal IEnumerable<System.Type> Types { get; set; }
    }
}
