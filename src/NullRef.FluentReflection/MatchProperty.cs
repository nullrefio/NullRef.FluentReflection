using System.Reflection;

namespace NullRef.FluentReflection
{
    public class MatchProperty
    {
        /// <summary>
        /// The declaring object type
        /// </summary>
        public System.Type Type { get; set; }
        public PropertyInfo Property { get; set; }
        public override string ToString() => $"{Type.Name}.{Property.Name}";
    }
}
