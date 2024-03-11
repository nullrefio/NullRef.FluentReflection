using System.Reflection;

namespace NullRef.FluentReflection
{
    public class MatchParameter
    {
        public System.Type Type { get; set; }
        public MethodInfo Method { get; set; }
        public ParameterInfo Parameter { get; set; }
        public override string ToString() => $"{Type.Name}.{Method.Name}({Parameter.ParameterType.Name} {Parameter.Name})";
    }
}
