using System.Reflection;

namespace NullRef.FluentReflection
{
    public class MatchParameter
    {
        /// <summary>
        /// The declaring object type of the parent method
        /// </summary>
        public System.Type Type { get; set; }
        /// <summary>
        /// The parent method information for the parameter
        /// </summary>
        public MethodBase Method { get; set; }
        /// <summary>
        /// The parameter information
        /// </summary>
        public ParameterInfo Parameter { get; set; }
        public override string ToString() => $"{Type.Name}.{Method.Name}({Parameter.ParameterType.Name} {Parameter.Name})";
    }
}
