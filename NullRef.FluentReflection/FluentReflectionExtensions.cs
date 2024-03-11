using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NullRef.FluentReflection
{
    public static partial class FluentReflectionExtensions
    {
        public static AnalysisAssembly ForAssembly(this Assembly assembly)
        {
            var r = new AnalysisAssembly();
            r.Assemblies.Add(assembly);
            return r;
        }

        public static AnalysisAssembly ForAssemblies(this Assembly[] assemblies)
        {
            var r = new AnalysisAssembly();
            r.Assemblies.AddRange(assemblies);
            return r;
        }






        public static AnalysisType ForType<T>(this AnalysisAssembly item)
        {
            var input = new AnalysisType(item);
            input.Types = input.Types.Where(x => x == typeof(T));
            return input;
        }

        public static AnalysisType ForTypes(this AnalysisAssembly item, System.Type[] types)
        {
            var input = new AnalysisType(item);
            input.Types = input.Types.Where(x => types.Contains(x));
            return input;
        }

        public static AnalysisType ImplementsType<T>(this AnalysisAssembly item)
        {
            var input = new AnalysisType(item);
            input.Types = input.Types.Where(x => typeof(T).IsAssignableFrom(x));
            return input;
        }

        public static AnalysisType ImplementsTypes(this AnalysisAssembly item, params System.Type[] types)
        {
            var input = new AnalysisType(item);
            input.Types = input.Types.Where(x => types.Any(z => z.IsAssignableFrom(x)));
            return input;
        }

        public static AnalysisType DoesNotImplementType<T>(this AnalysisAssembly item)
        {
            var input = new AnalysisType(item);
            input.Types = input.Types.Where(x => !typeof(T).IsAssignableFrom(x));
            return input;
        }

        public static AnalysisType Types(this AnalysisAssembly item)
        {
            return new AnalysisType(item);
        }

        public static AnalysisType WithAttribute<T>(this AnalysisType item)
            where T : System.Attribute
        {
            item.Types = item.Types.Where(x => x.GetCustomAttribute<T>() != null);
            return item;
        }

        public static AnalysisType Abstract(this AnalysisType item, bool value)
        {
            item.Types = item.Types.Where(x => x.IsAbstract == value);
            return item;
        }

        public static AnalysisType WithDefaultConstructor(this AnalysisType item)
        {
            item.Types = item.Types.Where(x => x.GetConstructors().Any(z => z.GetParameters().Length == 0));
            return item;
        }

        public static AnalysisType WithOnlyDefaultConstructor(this AnalysisType item)
        {
            item.Types = item.Types
                .Where(x => x.GetConstructors().Length == 0 &&
                            x.GetConstructors()
                             .Any(z => z.GetParameters().Length == 0));
            return item;
        }

        public static AnalysisType MissingDefaultConstructor(this AnalysisType item)
        {
            item.Types = item.Types.Where(x => !x.GetConstructors().Any(z => z.GetParameters().Length == 0));
            return item;
        }

        public static AnalysisType NameEndsWith(this AnalysisType item, string str)
        {
            item.Types = item.Types.Where(x => x.Name.EndsWith(str, StringComparison.InvariantCultureIgnoreCase));
            return item;
        }

        public static AnalysisType NameStartsWith(this AnalysisType item, string str)
        {
            item.Types = item.Types.Where(x => x.Name.StartsWith(str, StringComparison.InvariantCultureIgnoreCase));
            return item;
        }

        public static AnalysisType NameContains(this AnalysisType item, string str)
        {
            item.Types = item.Types.Where(x => x.Name.ToLower().Contains(str.ToLower()));
            return item;
        }

        public static AnalysisType DoesNotImplementType<T>(this AnalysisType item)
        {
            item.Types = item.Types.Where(x => !typeof(T).IsAssignableFrom(x));
            return item;
        }




        public static AnalysisMethod Methods(this AnalysisType item)
        {
            return new AnalysisMethod(item);
        }

        public static AnalysisMethod WithAttribute<T>(this AnalysisMethod item)
        where T : System.Attribute
        {
            item.Methods = item.Methods.Where(x => x.Method.GetCustomAttribute<T>() != null);
            return item;
        }

        public static AnalysisMethod MissingAttribute<T>(this AnalysisMethod item)
            where T : System.Attribute
        {
            item.Methods = item.Methods.Where(x => x.Method.GetCustomAttribute<T>() is null);
            return item;
        }

        public static AnalysisMethod NameContains(this AnalysisMethod item, string str)
        {
            item.Methods = item.Methods.Where(x => x.Method.Name.ToLower().Contains(str.ToLower()));
            return item;
        }

        public static AnalysisMethod MissingAttributes(this AnalysisMethod item, params System.Type[] types)
        {
            types.ThrowIfNotType<System.Attribute>();
            item.Methods = item.Methods.Where(x => x.Method.GetCustomAttributes().Select(z => z.GetType()).Intersect(types).Count() == 0);
            return item;
        }

        public static AnalysisMethod MissingAttributes<T1, T2>(this AnalysisMethod item)
            where T1 : System.Attribute
            where T2 : System.Attribute
        {
            return item.MissingAttributes(typeof(T1), typeof(T2));
        }

        public static AnalysisMethod MissingAttributes<T1, T2, T3>(this AnalysisMethod item)
            where T1 : System.Attribute
            where T2 : System.Attribute
            where T3 : System.Attribute
        {
            return item.MissingAttributes(typeof(T1), typeof(T2), typeof(T3));
        }





        public static AnalysisParameter Parameters(this AnalysisMethod item)
        {
            return new AnalysisParameter(item);
        }

        public static AnalysisParameter WithAttribute<T>(this AnalysisParameter item)
        where T : System.Attribute
        {
            item.Parameters = item.Parameters.Where(x => x.Parameter.GetCustomAttribute<T>() != null);
            return item;
        }

        public static AnalysisParameter WithAnyAttribute<T1, T2, T3, T4, T5>(this AnalysisParameter item)
            where T1 : System.Attribute
            where T2 : System.Attribute
            where T3 : System.Attribute
            where T4 : System.Attribute
            where T5 : System.Attribute
        {
            item.Parameters = item.Parameters.Where(x => x.Parameter.GetCustomAttribute<T1>() != null ||
                                                         x.Parameter.GetCustomAttribute<T2>() != null ||
                                                         x.Parameter.GetCustomAttribute<T3>() != null ||
                                                         x.Parameter.GetCustomAttribute<T4>() != null ||
                                                         x.Parameter.GetCustomAttribute<T5>() != null);
            return item;
        }

        public static AnalysisParameter WithAnyAttribute<T1, T2, T3, T4>(this AnalysisParameter item)
            where T1 : System.Attribute
            where T2 : System.Attribute
            where T3 : System.Attribute
            where T4 : System.Attribute => item.WithAnyAttribute<T1, T2, T3, T4, T4>();

        public static AnalysisParameter WithAnyAttribute<T1, T2, T3>(this AnalysisParameter item)
            where T1 : System.Attribute
            where T2 : System.Attribute
            where T3 : System.Attribute => item.WithAnyAttribute<T1, T2, T3, T3, T3>();

        public static AnalysisParameter WithAnyAttribute<T1, T2>(this AnalysisParameter item)
            where T1 : System.Attribute
            where T2 : System.Attribute => item.WithAnyAttribute<T1, T2, T2, T2, T2>();

        public static AnalysisParameter MissingAllAttributes<T1, T2, T3, T4, T5>(this AnalysisParameter item)
            where T1 : System.Attribute
            where T2 : System.Attribute
            where T3 : System.Attribute
            where T4 : System.Attribute
            where T5 : System.Attribute
        {
            item.Parameters = item.Parameters.Where(x => x.Parameter.GetCustomAttribute<T1>() is null &&
                                                         x.Parameter.GetCustomAttribute<T2>() is null &&
                                                         x.Parameter.GetCustomAttribute<T3>() is null &&
                                                         x.Parameter.GetCustomAttribute<T4>() is null &&
                                                         x.Parameter.GetCustomAttribute<T5>() is null);
            return item;
        }

        public static AnalysisParameter MissingAllAttributes<T1, T2, T3, T4>(this AnalysisParameter item)
            where T1 : System.Attribute
            where T2 : System.Attribute
            where T3 : System.Attribute
            where T4 : System.Attribute => item.MissingAllAttributes<T1, T2, T3, T4, T4>();

        public static AnalysisParameter MissingAllAttributes<T1, T2, T3>(this AnalysisParameter item)
            where T1 : System.Attribute
            where T2 : System.Attribute
            where T3 : System.Attribute => item.MissingAllAttributes<T1, T2, T3, T3, T3>();

        public static AnalysisParameter MissingAllAttributes<T1, T2>(this AnalysisParameter item)
            where T1 : System.Attribute
            where T2 : System.Attribute => item.MissingAllAttributes<T1, T2, T2, T2, T2>();

        public static AnalysisParameter MissingAttribute<T>(this AnalysisParameter item)
            where T : System.Attribute
        {
            item.Parameters = item.Parameters.Where(x => x.Parameter.GetCustomAttribute<T>() is null);
            return item;
        }

        public static AnalysisParameter OfType<T>(this AnalysisParameter item)
        {
            item.Parameters = item.Parameters.Where(x => x.Parameter.ParameterType == typeof(T));
            return item;
        }

        public static AnalysisParameter ImplementsType<T>(this AnalysisParameter item)
        {
            item.Parameters = item.Parameters.Where(x => typeof(T).IsAssignableFrom(x.Parameter.ParameterType));
            return item;
        }

        public static AnalysisParameter NotType<T>(this AnalysisParameter item)
        {
            item.Parameters = item.Parameters.Where(x => x.Parameter.ParameterType != typeof(T));
            return item;
        }

        public static AnalysisParameter NotTypes(this AnalysisParameter item, params System.Type[] types)
        {
            item.Parameters = item.Parameters.Where(x => !types.Contains(x.Parameter.ParameterType));
            return item;
        }

        public static AnalysisParameter ValueTypes(this AnalysisParameter item)
        {
            item.Parameters = item.Parameters.Where(x => x.Parameter.ParameterType.IsValueType);
            return item;
        }

        public static AnalysisParameter ReferenceTypes(this AnalysisParameter item)
        {
            item.Parameters = item.Parameters.Where(x => !x.Parameter.ParameterType.IsValueType);
            return item;
        }

        public static AnalysisParameter OfTypes(this AnalysisParameter item, params System.Type[] types)
        {
            item.Parameters = item.Parameters.Where(x => types.Contains(x.Parameter.ParameterType));
            return item;
        }

        public static AnalysisParameter TypeWithAttribute<T>(this AnalysisParameter item)
            where T : System.Attribute
        {
            item.Parameters = item.Parameters.Where(x => x.Parameter.GetCustomAttribute<T>() != null);
            return item;
        }





        public static AnalysisMethod Returns<T>(this AnalysisMethod item)
        {
            item.Methods = item.Methods.Where(x => typeof(T).IsAssignableFrom(x.Method.ReturnType));
            return item;
        }

        public static AnalysisMethod DoesNotReturn<T>(this AnalysisMethod item)
        {
            item.Methods = item.Methods.Where(x => !typeof(T).IsAssignableFrom(x.Method.ReturnType));
            return item;
        }

        public static AnalysisMethod ReturnsTaskOf<T>(this AnalysisMethod item)
        {
            var input = (item as AnalysisMethod);
            input.Methods = input.Methods.Where(x =>
                                typeof(System.Threading.Tasks.Task).IsAssignableFrom(x.Method.ReturnType) &&
                                x.Method.ReturnType.IsGenericType &&
                                x.Method.ReturnType.GetGenericTypeDefinition() == typeof(System.Threading.Tasks.Task<>) &&
                                typeof(T).IsAssignableFrom(x.Method.ReturnType.GenericTypeArguments[0]));
            return input;
        }

        public static AnalysisMethod ReturnsTask(this AnalysisMethod item)
        {
            var input = (item as AnalysisMethod);
            input.Methods = input.Methods.Where(x => typeof(System.Threading.Tasks.Task).IsAssignableFrom(x.Method.ReturnType)).ToList();
            return input;
        }






        public static AnalysisProperty Properties(this AnalysisType item)
        {
            return new AnalysisProperty(item);
        }

        public static AnalysisProperty OfType<T>(this AnalysisProperty item)
        {
            item.Properties = item.Properties.Where(x => x.Property.PropertyType == typeof(T));
            return item;
        }

        public static AnalysisProperty OfTypes(this AnalysisProperty item, params System.Type[] types)
        {
            item.Properties = item.Properties.Where(x => types.Contains(x.Property.PropertyType));
            return item;
        }

        public static AnalysisProperty NotType<T>(this AnalysisProperty item)
        {
            item.Properties = item.Properties.Where(x => x.Property.PropertyType != typeof(T));
            return item;
        }

        public static AnalysisProperty DoesNotImplement<T>(this AnalysisProperty item)
        {
            item.Properties = item.Properties.Where(x => !typeof(T).IsAssignableFrom(x.Property.PropertyType));
            return item;
        }

        public static AnalysisProperty IsEnum(this AnalysisProperty item)
        {
            item.Properties = item.Properties.Where(x => x.Property.PropertyType.IsEnum);
            return item;
        }

        public static AnalysisProperty IsWritable(this AnalysisProperty item, bool value)
        {
            if (value)
                item.Properties = item.Properties.Where(x => x.Property.GetSetMethod() != null);
            else
                item.Properties = item.Properties.Where(x => x.Property.GetSetMethod() == null);
            return item;
        }

        public static AnalysisProperty ValueTypes(this AnalysisProperty item)
        {
            item.Properties = item.Properties.Where(x => x.Property.PropertyType.IsValueType);
            return item;
        }

        public static AnalysisProperty ReferenceTypes(this AnalysisProperty item)
        {
            item.Properties = item.Properties.Where(x => !x.Property.PropertyType.IsValueType);
            return item;
        }

        public static AnalysisProperty GenericTypes(this AnalysisProperty item)
        {
            item.Properties = item.Properties.Where(x => x.Property.PropertyType.IsGenericType);
            return item;
        }

        public static AnalysisProperty TypeWithAttribute<T>(this AnalysisProperty item)
            where T : System.Attribute
        {
            item.Properties = item.Properties.Where(x => x.Property.GetCustomAttribute<T>() != null);
            return item;
        }




        public static AnalysisProperty WithAttribute<T>(this AnalysisProperty item)
            where T : System.Attribute
        {
            item.Properties = item.Properties.Where(x => x.Property.GetCustomAttribute<T>() != null);
            return item;
        }

        public static AnalysisProperty WithAttribute<T>(this AnalysisProperty item, Func<T, bool> predicate)
            where T : System.Attribute
        {
            item.Properties = item.Properties
                .Where(x => x.Property.GetCustomAttributes<T>().Cast<T>().Where(predicate).Any());
            return item;
        }

        public static AnalysisProperty MissingAttribute<T>(this AnalysisProperty item)
           where T : System.Attribute
        {
            item.Properties = item.Properties.Where(x => x.Property.GetCustomAttribute<T>() is null);
            return item;
        }

        public static AnalysisProperty MissingAttributes(this AnalysisProperty item, params System.Type[] types)
        {
            item.Properties = item.Properties.Where(x => x.Property.GetCustomAttributes().Select(z => z.GetType()).Intersect(types).Count() == 0);
            return item;
        }

        public static AnalysisProperty MissingAttributes<T1, T2>(this AnalysisProperty item)
            where T1 : System.Attribute
            where T2 : System.Attribute
        {
            return item.MissingAttributes(typeof(T1), typeof(T2));
        }

        public static AnalysisProperty MissingAttributes<T1, T2, T3>(this AnalysisProperty item)
            where T1 : System.Attribute
            where T2 : System.Attribute
            where T3 : System.Attribute
        {
            return item.MissingAttributes(typeof(T1), typeof(T2), typeof(T3));
        }

        public static AnalysisProperty NameIsNot(this AnalysisProperty item, string name)
        {
            item.Properties = item.Properties.Where(x => !x.Property.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            return item;
        }

        /// <summary>
        /// Get all assemblies in AppDomain
        /// </summary>
        /// <returns>Assemblies starting with 'Microsoft' and 'System' are fitered out</returns>
        public static Assembly[] GetAppDomainAssemblies()
        {
            var all = AppDomain.CurrentDomain
                .GetAssemblies()
                .ToList();

            var result = new List<Assembly>();
            foreach (var asm in all)
            {
                result.Add(asm);
                var sub = asm.GetReferencedAssemblies();
                var nameQueue = new Queue<AssemblyName>(sub.ToArray());
                var alreadyProcessed = new HashSet<string>() { asm.FullName };
                while (nameQueue.Any())
                {
                    var name = nameQueue.Dequeue();
                    var fullName = name.FullName;
                    if (alreadyProcessed.Contains(fullName) || fullName.StartsWith("Microsoft.") || fullName.StartsWith("System."))
                        continue;
                    alreadyProcessed.Add(fullName);
                    try
                    {
                        var newAssembly = Assembly.Load(name);
                        result.Add(newAssembly);
                        foreach (var innerAsmName in newAssembly.GetReferencedAssemblies().ToArray())
                            nameQueue.Enqueue(innerAsmName);
                    }
                    catch { }
                }
            }
            return result.Distinct().OrderBy(x => x.FullName).ToArray();
        }
    }

    partial class FluentReflectionExtensions
    {
        public static System.Type[] ToList(this AnalysisType item)
        {
            return (item as AnalysisType).Types.ToArray();
        }

        public static MatchParameter[] ToList(this AnalysisParameter item)
        {
            return (item as AnalysisParameter).Parameters.ToArray();
        }

        public static MatchMethod[] ToList(this AnalysisMethod item)
        {
            return (item as AnalysisMethod).Methods.ToArray();
        }

        public static MatchProperty[] ToList(this AnalysisProperty item)
        {
            return (item as AnalysisProperty).Properties.ToArray();
        }

        public static AnalysisValidateResult DoesNotReturnErrors<T>(this AnalysisMethod item)
        {
            var r = new AnalysisValidateResult
            {
                Errors = item.Methods.Select(x => new AnalysisError { Text = $"The method {x.Type.Name}.{x.Method.Name} does not return {typeof(T).Name}" }).ToList(),
            };
            return r;
        }

        public static AnalysisValidateResult PropertyMissingAttributeErrors<T>(this AnalysisProperty item)
        {
            var r = new AnalysisValidateResult
            {
                Errors = item.Properties.Select(x => new AnalysisError { Text = $"The property {x.Type.Name}.{x.Property.Name} is missing attribute {typeof(T).Name}" }).ToList(),
            };
            return r;
        }

        private static void ThrowIfNotType<T>(this System.Type[] types)
        {
            var tname = typeof(T).FullName;
            var typeErrors = types.Where(x => !typeof(T).IsAssignableFrom(x)).Select(x => x.FullName).ToList();
            if (typeErrors.Any())
                throw new Exception($"The types are not of type {tname}: " + string.Join(", ", typeErrors));
        }
    }
}
