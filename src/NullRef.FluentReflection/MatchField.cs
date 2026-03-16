using System.Reflection;

namespace NullRef.FluentReflection
{
    public class MatchField
    {
        /// <summary>
        /// The declaring object type
        /// </summary>
        public System.Type Type { get; internal set; }
        public FieldInfo Field { get; internal set; }
        public override string ToString() => $"{Type.Name}.{Field.Name}";
    }

    public class MatchConstValue<T> : MatchField
    {
        public T Value { get; internal set; }
    }
}
