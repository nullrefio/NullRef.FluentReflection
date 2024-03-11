using System.Reflection;

namespace NullRef.FluentReflection
{
    public class MatchProperty
    {
        public System.Type Type { get; set; }
        public PropertyInfo Property { get; set; }
        public override string ToString() => $"{Type.Name}.{Property.Name}";
    }
}
