using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NullRef.FluentReflection;

public class AnalysisProperty
{
    internal AnalysisProperty(AnalysisType item)
    {
        Properties = item.Types
            .SelectMany(x => x.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            .Select(x => new MatchProperty { Type = x.DeclaringType, Property = x });
    }

    internal IEnumerable<MatchProperty> Properties { get; set; }
}
