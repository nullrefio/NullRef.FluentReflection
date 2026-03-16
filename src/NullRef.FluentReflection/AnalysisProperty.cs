using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NullRef.FluentReflection
{
    public interface IAnalysisProperty
    {
        IAnalysisProperty Filter(Func<MatchProperty, bool> predicate);
    }

    internal class AnalysisProperty : IAnalysisProperty
    {
        internal AnalysisProperty(AnalysisType item) : this(item, BindingFlags.Public) { }

        internal AnalysisProperty(AnalysisType item, BindingFlags bindingFlags)
        {
            Properties = item.Types
                .SelectMany(x => x.GetProperties(bindingFlags | BindingFlags.Instance))
                .Where(x => x.PropertyType.FullName != null)
                .Select(x => new MatchProperty { Type = x.ReflectedType, Property = x });
        }

        internal IEnumerable<MatchProperty> Properties { get; set; }

        public IAnalysisProperty Filter(Func<MatchProperty, bool> predicate)
        {
            Properties = Properties.Where(predicate);
            return this;
        }
    }
}
