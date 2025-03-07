using System.Collections.Generic;
using System.Linq;

namespace NullRef.FluentReflection
{
    public interface IAnalysisConstructor { }

    internal class AnalysisConstructor : IAnalysisConstructor
    {
        internal AnalysisConstructor(AnalysisType item)
        {
            Constructors = item.Types
                .SelectMany(x => x.GetConstructors());
        }

        internal IEnumerable<System.Reflection.ConstructorInfo> Constructors { get; set; }
    }
}
