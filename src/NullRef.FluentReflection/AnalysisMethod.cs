using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NullRef.FluentReflection
{
    public interface IAnalysisMethod { }

    internal class AnalysisMethod : IAnalysisMethod
    {
        internal AnalysisMethod(AnalysisType item) : this(item, BindingFlags.Public) { }

        internal AnalysisMethod(AnalysisType item, BindingFlags bindingFlags)
        {
            var ignore = new string[] { nameof(item.GetType), nameof(item.Equals), nameof(item.ToString), nameof(item.GetHashCode) };

            Methods = item.Types
                .SelectMany(t => t.GetMethods(bindingFlags | BindingFlags.Instance)
                .Where(x => !ignore.Contains(x.Name))
                .Select(x => new MatchMethod { Type = t, Method = x }));
        }

        internal IEnumerable<MatchMethod> Methods { get; set; }
    }
}
