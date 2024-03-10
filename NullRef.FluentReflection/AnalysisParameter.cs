using System;
using System.Collections.Generic;
using System.Linq;

namespace NullRef.FluentReflection;

public class AnalysisParameter
{
    internal AnalysisParameter(AnalysisMethod item)
    {
        var ignore = new string[] { nameof(item.GetType), nameof(item.Equals), nameof(item.ToString), nameof(item.GetHashCode) };

        Parameters = item.Methods
            .SelectMany(x => x.Method.GetParameters()
                .Select(z => new MatchParameter { Type = x.Type, Method = x.Method, Parameter = z }))
            .Where(x => !ignore.Contains(x.Method.Name));
    }

    internal IEnumerable<MatchParameter> Parameters { get; set; }
}
