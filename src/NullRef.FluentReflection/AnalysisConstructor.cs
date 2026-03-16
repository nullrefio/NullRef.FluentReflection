using System;
using System.Collections.Generic;
using System.Linq;

namespace NullRef.FluentReflection
{
    public interface IAnalysisConstructor
    {
        IAnalysisConstructor Filter(Func<MatchConstructor, bool> predicate);
    }

    internal class AnalysisConstructor : IAnalysisConstructor
    {
        internal AnalysisConstructor(AnalysisType item)
        {
            Constructors = item.Types
                .SelectMany(t => t.GetConstructors()
                .Select(x => new MatchConstructor { Type = t, Method = x }));

        }

        internal IEnumerable<MatchConstructor> Constructors { get; set; }

        public IAnalysisConstructor Filter(Func<MatchConstructor, bool> predicate)
        {
            Constructors = Constructors.Where(predicate);
            return this;
        }
    }
}
