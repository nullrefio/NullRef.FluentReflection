using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NullRef.FluentReflection;

public class AnalysisMethod
{
    internal AnalysisMethod(AnalysisType item)
    {
        var ignore = new string[] { nameof(item.GetType), nameof(item.Equals), nameof(item.ToString), nameof(item.GetHashCode) };

        Methods = item.Types
            .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(x => !ignore.Contains(x.Name))
            .Select(x => new MatchMethod { Type = t, Method = x }));
    }

    internal IEnumerable<MatchMethod> Methods { get; set; }
}
