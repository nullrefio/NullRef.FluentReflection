using System.Linq;
using System.Reflection;

namespace NullRef.FluentReflection
{
    public class MatchConstructor
    {
        /// <summary>
        /// The declaring object type
        /// </summary>
        public System.Type Type { get; set; }
        public ConstructorInfo Method { get; set; }
        public override string ToString() => $"{Type.Name}{Method.Name}(" + string.Join(",", Method.GetParameters().Select(x => x.Name)) + ")";
    }
}
