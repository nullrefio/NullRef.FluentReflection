using System.Reflection;

namespace NullRef.FluentReflection
{
    public class MatchMethod
    {
        /// <summary>
        /// The declaring object type
        /// </summary>
        public System.Type Type { get; set; }
        public MethodInfo Method { get; set; }
        public override string ToString() => $"{Type.Name}.{Method.Name}";
    }
}
