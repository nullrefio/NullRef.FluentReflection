using System;
using System.Collections.Generic;
using System.Linq;

namespace NullRef.FluentReflection
{
    public interface IAnalysisParameter
    {
        IAnalysisParameter Filter(Func<MatchParameter, bool> predicate);
    }

    internal class AnalysisParameter : IAnalysisParameter
    {
        internal AnalysisParameter(AnalysisMethod item)
        {
            var ignore = new string[] { nameof(item.GetType), nameof(item.Equals), nameof(item.ToString), nameof(item.GetHashCode) };

            Parameters = item.Methods
                .SelectMany(x => x.Method.GetParameters()
                    .Select(z => new MatchParameter { Type = x.Type, Method = x.Method, Parameter = z }))
                .Where(x => !ignore.Contains(x.Method.Name));
        }

        internal AnalysisParameter(AnalysisConstructor item)
        {
            var ignore = new string[] { nameof(item.GetType), nameof(item.Equals), nameof(item.ToString), nameof(item.GetHashCode) };

            Parameters = item.Constructors
                    .SelectMany(x => x.Method.GetParameters()
                    .Select(z => new MatchParameter { Type = x.Type, Method = x.Method, Parameter = z }))
                    .Where(x => !ignore.Contains(x.Method.Name));
        }

        internal IEnumerable<MatchParameter> Parameters { get; set; }

        public IAnalysisParameter Filter(Func<MatchParameter, bool> predicate)
        {
            Parameters = Parameters.Where(predicate);
            return this;
        }
    }
}
