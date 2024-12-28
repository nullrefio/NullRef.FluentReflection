using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NullRef.FluentReflection
{
    public interface IAnalysisProperty { }

    internal class AnalysisProperty : IAnalysisProperty
    {
        internal AnalysisProperty(AnalysisType item) : this(item, BindingFlags.Public) { }

        internal AnalysisProperty(AnalysisType item, BindingFlags bindingFlags)
        {
            Properties = item.Types
                .SelectMany(x => x.GetProperties(bindingFlags | BindingFlags.Instance))
                .Where(x => x.PropertyType.FullName != null)
                .Select(x => new MatchProperty { Type = x.DeclaringType, Property = x });
        }

        internal IEnumerable<MatchProperty> Properties { get; set; }
    }
}
