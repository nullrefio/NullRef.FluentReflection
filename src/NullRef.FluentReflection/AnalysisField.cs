using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NullRef.FluentReflection
{
    public interface IAnalysisField { }

    internal class AnalysisField : IAnalysisField
    {
        internal AnalysisField(AnalysisType item) : this(item, BindingFlags.Public) { }

        internal AnalysisField(AnalysisType item, BindingFlags bindingFlags)
        {
            Fields = item.Types
                .SelectMany(x => x.GetFields(bindingFlags | BindingFlags.Instance))
                .Where(x => x.FieldType.FullName != null)
                .Select(x => new MatchField { Type = x.DeclaringType, Field = x });
        }

        internal IEnumerable<MatchField> Fields { get; set; }
    }

    public interface IAnalysisConst : IAnalysisField { }

    internal class AnalysisConst : AnalysisField, IAnalysisConst
    {
        internal AnalysisConst(AnalysisType item) : this(item, BindingFlags.Public) { }

        internal AnalysisConst(AnalysisType item, BindingFlags bindingFlags) : base(item, bindingFlags) { }
    }
}
