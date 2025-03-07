using NullRef.FluentReflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NullRef.FluentReflection
{
    public static partial class FluentReflectionExtensions
    {
        public static IAnalysisAssembly ForAssembly(this Assembly assembly)
        {
            var r = new AnalysisAssembly();
            r.Assemblies.Add(assembly);
            return r;
        }

        public static IAnalysisAssembly ForAssemblies(this Assembly[] assemblies)
        {
            var r = new AnalysisAssembly();
            r.Assemblies.AddRange(assemblies);
            return r;
        }


        #region Type

        public static IAnalysisType Is<T>(this IAnalysisAssembly item)
            => item.Is(typeof(T));

        public static IAnalysisType Is(this IAnalysisAssembly item, System.Type type)
        {
            var input = new AnalysisType(item as AnalysisAssembly);
            input.Types = input.Types.Where(x => x == type);
            return input;
        }

        public static IAnalysisType AsType(this System.Type item) => new AnalysisType(new[] { item });

        public static IAnalysisType AsType(this IEnumerable<System.Type> items) => new AnalysisType(items);

        public static IAnalysisType IsAny(this IAnalysisAssembly item, System.Type[] types)
        {
            var input = new AnalysisType(item as AnalysisAssembly);
            input.Types = input.Types.Where(x => types.Contains(x));
            return input;
        }

        public static IAnalysisType Implements<T>(this IAnalysisAssembly item)
            => item.Implements(typeof(T));

        public static IAnalysisType Implements(this IAnalysisAssembly item, System.Type type)
        {
            var input = new AnalysisType(item as AnalysisAssembly);
            input.Types = input.Types.Where(x => type.IsAssignableFrom(x));
            return input;
        }

        public static IAnalysisType IsNotAbstract(this IAnalysisType item)
        {
            var input = item as AnalysisType;
            input.Types = input.Types.Where(x => !x.IsAbstract);
            return item;
        }

        public static IAnalysisType ImplementsAny(this IAnalysisAssembly item, params System.Type[] types)
        {
            var input = new AnalysisType(item as AnalysisAssembly);
            input.Types = input.Types.Where(x => types.Any(z => z.IsAssignableFrom(x)));
            return input;
        }

        public static IAnalysisType DoesNotImplement<T>(this IAnalysisAssembly item)
            => item.DoesNotImplement(typeof(T));

        public static IAnalysisType DoesNotImplement(this IAnalysisAssembly item, System.Type type)
        {
            var input = new AnalysisType(item as AnalysisAssembly);
            input.Types = input.Types.Where(x => !type.IsAssignableFrom(x));
            return input;
        }

        public static IAnalysisType Types(this IAnalysisAssembly item) => new AnalysisType(item as AnalysisAssembly);

        public static IAnalysisType WithAttribute<T>(this IAnalysisType item, Func<T, bool> predicate)
            where T : System.Attribute
        {
            var input = item as AnalysisType;
            input.Types = input.Types.Where(x => x.GetCustomAttributes<T>().Cast<T>().Where(predicate).Any());
            return item;
        }

        public static IAnalysisType WithAttribute<T>(this IAnalysisType item)
            where T : System.Attribute => item.WithAttribute(typeof(T));

        public static IAnalysisType WithAttribute(this IAnalysisType item, System.Type attributeType)
        {
            attributeType.ThrowIfNotAttribute();
            var input = item as AnalysisType;
            input.Types = input.Types.Where(x => x.GetCustomAttribute(attributeType) != null);
            return item;
        }

        /// <summary>
        /// Filter to only types that are marked as abstract classes
        /// </summary>
        public static IAnalysisType IsAbstract(this IAnalysisType item) => item.IsAbstract(true);

        /// <summary>
        /// Filter to only types that are marked as abstract or not based on the specified parameter
        /// </summary>
        public static IAnalysisType IsAbstract(this IAnalysisType item, bool value)
        {
            var input = item as AnalysisType;
            input.Types = input.Types.Where(x => x.IsAbstract == value);
            return item;
        }

        /// <summary>
        /// Filter to only types that have a default constructor
        /// </summary>
        public static IAnalysisType HasDefaultConstructor(this IAnalysisType item)
        {
            var input = item as AnalysisType;
            input.Types = input.Types.Where(x => x.GetConstructors().Any(z => z.GetParameters().Length == 0));
            return item;
        }

        /// <summary>
        /// Filter to only types that have a default constructor and no other constructors
        /// </summary>
        public static IAnalysisType HasOnlyDefaultConstructor(this IAnalysisType item)
        {
            var input = item as AnalysisType;
            input.Types = input.Types
                .Where(x => x.GetConstructors().Length == 0 &&
                            x.GetConstructors()
                             .Any(z => z.GetParameters().Length == 0));
            return item;
        }

        /// <summary>
        /// Filter to only types that do not have a default constructor
        /// </summary>
        public static IAnalysisType MissingDefaultConstructor(this IAnalysisType item)
        {
            var input = item as AnalysisType;
            input.Types = input.Types.Where(x => !x.GetConstructors().Any(z => z.GetParameters().Length == 0));
            return item;
        }

        /// <summary>
        /// Match types with an exact name
        /// </summary>
        public static IAnalysisType NameIs(this IAnalysisType item, string str)
        {
            var input = item as AnalysisType;
            input.Types = input.Types.Where(x => x.Name == str);
            return item;
        }

        /// <summary>
        /// Match types with names that contains a sub-string.
        /// </summary>
        public static IAnalysisType NameContains(this IAnalysisType item, string str)
        {
            var input = item as AnalysisType;
            input.Types = input.Types.Where(x => x.Name.ToLowerInvariant().Contains(str.ToLowerInvariant()));
            return item;
        }

        /// <summary>
        /// Match types with names that start with a sub-string.
        /// </summary>
        public static IAnalysisType NameStartsWith(this IAnalysisType item, string str)
        {
            var input = item as AnalysisType;
            input.Types = input.Types.Where(x => x.Name.StartsWith(str, StringComparison.InvariantCultureIgnoreCase));
            return item;
        }

        /// <summary>
        /// Match types with names that end with a sub-string.
        /// </summary>
        public static IAnalysisType NameEndsWith(this IAnalysisType item, string str)
        {
            var input = item as AnalysisType;
            input.Types = input.Types.Where(x => x.Name.EndsWith(str, StringComparison.InvariantCultureIgnoreCase));
            return item;
        }

        /// <summary>
        /// Match types with names that do not contain a sub-string.
        /// </summary>
        public static IAnalysisType NameDoesNotContain(this IAnalysisType item, string str)
        {
            var input = item as AnalysisType;
            input.Types = input.Types.Where(x => !x.Name.ToLowerInvariant().Contains(str.ToLowerInvariant()));
            return item;
        }

        /// <summary>
        /// Match types with names that do not start with a sub-string.
        /// </summary>
        public static IAnalysisType NameDoesNotStartWith(this IAnalysisType item, string str)
        {
            var input = item as AnalysisType;
            input.Types = input.Types.Where(x => !x.Name.StartsWith(str, StringComparison.InvariantCultureIgnoreCase));
            return item;
        }

        /// <summary>
        /// Match types with names that do not end with a sub-string.
        /// </summary>
        public static IAnalysisType NameDoesNotEndWith(this IAnalysisType item, string str)
        {
            var input = item as AnalysisType;
            input.Types = input.Types.Where(x => !x.Name.EndsWith(str, StringComparison.InvariantCultureIgnoreCase));
            return item;
        }

        /// <summary>
        /// Match types that do not implement the specified type
        /// </summary>
        public static IAnalysisType DoesNotImplement<T>(this IAnalysisType item)
            => item.DoesNotImplement(typeof(T));

        /// <summary>
        /// Match types that do not implement the specified type
        /// </summary>
        public static IAnalysisType DoesNotImplement(this IAnalysisType item, System.Type type)
        {
            var input = item as AnalysisType;
            input.Types = input.Types.Where(x => !type.IsAssignableFrom(x));
            return item;
        }

        /// <summary>
        /// Match types that do implement a specified type
        /// </summary>
        public static IAnalysisType Implements<T>(this IAnalysisType item)
            => item.Implements(typeof(T));

        /// <summary>
        /// Match types that do implement a specified type
        /// </summary>
        public static IAnalysisType Implements(this IAnalysisType item, System.Type type)
        {
            var input = item as AnalysisType;
            input.Types = input.Types.Where(x => type.IsAssignableFrom(x));
            return item;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Return all constructors for filtered types
        /// </summary>
        public static IAnalysisConstructor Constructors(this IAnalysisType item)
        {
            return new AnalysisConstructor(item as AnalysisType);
        }

        /// <summary>
        /// Return all default constructors with no parameters for filtered types
        /// </summary>
        public static IAnalysisConstructor DefaultConstructor(this IAnalysisConstructor item)
        {
            var input = item as AnalysisConstructor;
            input.Constructors = input.Constructors.Where(x => x.GetParameters().Length == 0);
            return item;
        }

        /// <summary>
        /// Return all non-default constructors for filtered types
        /// </summary>
        public static IAnalysisConstructor NonDefaultConstructors(this IAnalysisConstructor item)
        {
            var input = item as AnalysisConstructor;
            input.Constructors = input.Constructors.Where(x => x.GetParameters().Length > 0);
            return item;
        }

        /// <summary>
        /// Return parameters for filtered constructors
        /// </summary>
        public static IAnalysisParameter Parameters(this IAnalysisConstructor item)
        {
            return new AnalysisParameter(item as AnalysisConstructor);
        }

        #endregion

        #region Methods
        public static IAnalysisMethod Methods(this IAnalysisType item) => item.Methods(BindingFlags.Public);

        public static IAnalysisMethod Methods(this IAnalysisType item, BindingFlags bindingFlags)
        {
            return new AnalysisMethod(item as AnalysisType, bindingFlags);
        }

        public static IAnalysisMethod WithAttribute<T>(this IAnalysisMethod item, Func<T, bool> predicate)
            where T : System.Attribute
        {
            var input = item as AnalysisMethod;
            input.Methods = input.Methods.Where(x => x.Method.GetCustomAttributes<T>().Cast<T>().Where(predicate).Any());
            return item;
        }

        public static IAnalysisMethod WithAttribute<T>(this IAnalysisMethod item)
            where T : System.Attribute => item.WithAttribute(typeof(T));

        public static IAnalysisMethod WithAttribute(this IAnalysisMethod item, System.Type attributeType)
        {
            attributeType.ThrowIfNotAttribute();
            var input = item as AnalysisMethod;
            input.Methods = input.Methods.Where(x => x.Method.GetCustomAttribute(attributeType) != null);
            return item;
        }

        public static IAnalysisMethod MissingAttribute<T>(this IAnalysisMethod item)
            where T : System.Attribute => item.MissingAttribute(typeof(T));

        public static IAnalysisMethod MissingAttribute(this IAnalysisMethod item, System.Type attributeType)
        {
            attributeType.ThrowIfNotAttribute();
            var input = item as AnalysisMethod;
            input.Methods = input.Methods.Where(x => x.Method.GetCustomAttribute(attributeType) is null);
            return item;
        }

        /// <summary>
        /// Match methods with an exact name
        /// </summary>
        public static IAnalysisMethod NameIs(this IAnalysisMethod item, string str)
        {
            var input = item as AnalysisMethod;
            input.Methods = input.Methods.Where(x => x.Method.Name == str);
            return item;
        }

        public static IAnalysisMethod NameContains(this IAnalysisMethod item, string str)
        {
            var input = item as AnalysisMethod;
            input.Methods = input.Methods.Where(x => x.Method.Name.ToLowerInvariant().Contains(str.ToLowerInvariant()));
            return item;
        }

        /// <summary>
        /// Match methods with names that start with a sub-string.
        /// </summary>
        public static IAnalysisMethod NameStartsWith(this IAnalysisMethod item, string str)
        {
            var input = item as AnalysisMethod;
            input.Methods = input.Methods.Where(x => x.Method.Name.StartsWith(str, StringComparison.InvariantCultureIgnoreCase));
            return item;
        }

        /// <summary>
        /// Match methods with names that end with a sub-string.
        /// </summary>
        public static IAnalysisMethod NameEndsWith(this IAnalysisMethod item, string str)
        {
            var input = item as AnalysisMethod;
            input.Methods = input.Methods.Where(x => x.Method.Name.EndsWith(str, StringComparison.InvariantCultureIgnoreCase));
            return item;
        }

        /// <summary>
        /// Match methods with names that do not contain a sub-string.
        /// </summary>
        public static IAnalysisMethod NameDoesNotContain(this IAnalysisMethod item, string str)
        {
            var input = item as AnalysisMethod;
            input.Methods = input.Methods.Where(x => !x.Method.Name.ToLowerInvariant().Contains(str.ToLowerInvariant()));
            return item;
        }

        /// <summary>
        /// Match methods with names that do not start with a sub-string.
        /// </summary>
        public static IAnalysisMethod NameDoesNotStartWith(this IAnalysisMethod item, string str)
        {
            var input = item as AnalysisMethod;
            input.Methods = input.Methods.Where(x => !x.Method.Name.StartsWith(str, StringComparison.InvariantCultureIgnoreCase));
            return item;
        }

        /// <summary>
        /// Match methods with names that do not end with a sub-string.
        /// </summary>
        public static IAnalysisMethod NameDoesNotEndWith(this IAnalysisMethod item, string str)
        {
            var input = item as AnalysisMethod;
            input.Methods = input.Methods.Where(x => !x.Method.Name.EndsWith(str, StringComparison.InvariantCultureIgnoreCase));
            return item;
        }

        /// <summary>
        /// Match methods that do not have any attributes in the specified type list
        /// </summary>
        public static IAnalysisMethod MissingAttributeAll(this IAnalysisMethod item, params System.Type[] types)
        {
            types.ToList().ForEach(x => x.ThrowIfNotAttribute());
            var input = item as AnalysisMethod;
            input.Methods = input.Methods.Where(x => x.Method.GetCustomAttributes().Select(z => z.GetType()).Intersect(types).Count() == 0);
            return item;
        }

        public static IAnalysisMethod MissingAttributeAll<T1, T2>(this IAnalysisMethod item)
            where T1 : System.Attribute
            where T2 : System.Attribute
        {
            return item.MissingAttributeAll(typeof(T1), typeof(T2));
        }

        public static IAnalysisMethod MissingAttributeAll<T1, T2, T3>(this IAnalysisMethod item)
            where T1 : System.Attribute
            where T2 : System.Attribute
            where T3 : System.Attribute
        {
            return item.MissingAttributeAll(typeof(T1), typeof(T2), typeof(T3));
        }

        /// <summary>
        /// Match methods that return the specified type or a derivation thereof
        /// </summary>
        public static IAnalysisMethod Returns<T>(this IAnalysisMethod item)
            => item.Returns(typeof(T));

        /// <summary>
        /// Match methods that return the specified type or a derivation thereof
        /// </summary>
        public static IAnalysisMethod Returns(this IAnalysisMethod item, System.Type type)
        {
            var input = item as AnalysisMethod;
            input.Methods = input.Methods.Where(x => type.IsAssignableFrom(x.Method.ReturnType));
            return item;
        }

        /// <summary>
        /// Match methods that do not return the specified type or a derivation thereof
        /// </summary>
        public static IAnalysisMethod DoesNotReturn<T>(this IAnalysisMethod item)
            => item.DoesNotReturn(typeof(T));

        /// <summary>
        /// Match methods that do not return the specified type or a derivation thereof
        /// </summary>
        public static IAnalysisMethod DoesNotReturn(this IAnalysisMethod item, System.Type type)
        {
            var input = item as AnalysisMethod;
            input.Methods = input.Methods.Where(x => !type.IsAssignableFrom(x.Method.ReturnType));
            return item;
        }

        public static IAnalysisMethod ReturnsTaskOf<T>(this IAnalysisMethod item)
            => item.ReturnsTaskOf(typeof(T));

        public static IAnalysisMethod ReturnsTaskOf(this IAnalysisMethod item, System.Type type)
        {
            var input = (item as AnalysisMethod);
            input.Methods = input.Methods.Where(x =>
                                typeof(System.Threading.Tasks.Task).IsAssignableFrom(x.Method.ReturnType) &&
                                x.Method.ReturnType.IsGenericType &&
                                x.Method.ReturnType.GetGenericTypeDefinition() == typeof(System.Threading.Tasks.Task<>) &&
                                x.Method.ReturnType.GenericTypeArguments.Length == 1 &&
                                type.IsAssignableFrom(x.Method.ReturnType.GenericTypeArguments[0]));
            return input;
        }

        public static IAnalysisMethod ReturnsTask(this IAnalysisMethod item)
        {
            var input = (item as AnalysisMethod);
            input.Methods = input.Methods.Where(x => typeof(System.Threading.Tasks.Task).IsAssignableFrom(x.Method.ReturnType)).ToList();
            return input;
        }

        public static IAnalysisMethod DoesNotReturnsTask(this IAnalysisMethod item)
        {
            var input = (item as AnalysisMethod);
            input.Methods = input.Methods.Where(x => !typeof(System.Threading.Tasks.Task).IsAssignableFrom(x.Method.ReturnType)).ToList();
            return input;
        }

        #endregion

        #region Parameter

        public static IAnalysisParameter Parameters(this IAnalysisMethod item)
        {
            return new AnalysisParameter(item as AnalysisMethod);
        }

        /// <summary>
        /// Match parameters that have an attribute of the specified type
        /// </summary>
        public static IAnalysisParameter WithAttribute<T>(this IAnalysisParameter item)
            where T : System.Attribute
        {
            var input = item as AnalysisParameter;
            input.Parameters = input.Parameters.Where(x => x.Parameter.GetCustomAttribute<T>() != null);
            return item;
        }

        /// <summary>
        /// Match parameters that have an attribute of the specified type
        /// </summary>
        public static IAnalysisParameter WithAttribute<T>(this IAnalysisParameter item, Func<T, bool> predicate)
            where T : System.Attribute
        {
            var input = item as AnalysisParameter;
            input.Parameters = input.Parameters.Where(x => x.Parameter.GetCustomAttributes<T>().Cast<T>().Where(predicate).Any());
            return item;
        }

        public static IAnalysisParameter WithAttribute(this IAnalysisParameter item, System.Type attributeType)
        {
            var input = item as AnalysisParameter;
            input.Parameters = input.Parameters.Where(x => x.Parameter.GetCustomAttribute(attributeType) != null);
            return item;
        }

        public static IAnalysisParameter WithAttributeAny<T1, T2, T3, T4, T5>(this IAnalysisParameter item)
            where T1 : System.Attribute
            where T2 : System.Attribute
            where T3 : System.Attribute
            where T4 : System.Attribute
            where T5 : System.Attribute
        {
            var input = item as AnalysisParameter;
            input.Parameters = input.Parameters.Where(x => x.Parameter.GetCustomAttribute<T1>() != null ||
                                                         x.Parameter.GetCustomAttribute<T2>() != null ||
                                                         x.Parameter.GetCustomAttribute<T3>() != null ||
                                                         x.Parameter.GetCustomAttribute<T4>() != null ||
                                                         x.Parameter.GetCustomAttribute<T5>() != null);
            return item;
        }

        public static IAnalysisParameter WithAttributeAny<T1, T2, T3, T4>(this IAnalysisParameter item)
            where T1 : System.Attribute
            where T2 : System.Attribute
            where T3 : System.Attribute
            where T4 : System.Attribute => item.WithAttributeAny<T1, T2, T3, T4, T4>();

        public static IAnalysisParameter WithAttributeAny<T1, T2, T3>(this IAnalysisParameter item)
            where T1 : System.Attribute
            where T2 : System.Attribute
            where T3 : System.Attribute => item.WithAttributeAny<T1, T2, T3, T3, T3>();

        public static IAnalysisParameter WithAttributeAny<T1, T2>(this IAnalysisParameter item)
            where T1 : System.Attribute
            where T2 : System.Attribute => item.WithAttributeAny<T1, T2, T2, T2, T2>();

        public static IAnalysisParameter MissingAttributeAll<T1, T2, T3, T4, T5>(this IAnalysisParameter item)
            where T1 : System.Attribute
            where T2 : System.Attribute
            where T3 : System.Attribute
            where T4 : System.Attribute
            where T5 : System.Attribute
        {
            var input = item as AnalysisParameter;
            input.Parameters = input.Parameters.Where(x => x.Parameter.GetCustomAttribute<T1>() is null &&
                                                         x.Parameter.GetCustomAttribute<T2>() is null &&
                                                         x.Parameter.GetCustomAttribute<T3>() is null &&
                                                         x.Parameter.GetCustomAttribute<T4>() is null &&
                                                         x.Parameter.GetCustomAttribute<T5>() is null);
            return item;
        }

        public static IAnalysisParameter MissingAttributeAll<T1, T2, T3, T4>(this IAnalysisParameter item)
            where T1 : System.Attribute
            where T2 : System.Attribute
            where T3 : System.Attribute
            where T4 : System.Attribute => item.MissingAttributeAll<T1, T2, T3, T4, T4>();

        public static IAnalysisParameter MissingAttributeAll<T1, T2, T3>(this IAnalysisParameter item)
            where T1 : System.Attribute
            where T2 : System.Attribute
            where T3 : System.Attribute => item.MissingAttributeAll<T1, T2, T3, T3, T3>();

        public static IAnalysisParameter MissingAttributeAll<T1, T2>(this IAnalysisParameter item)
            where T1 : System.Attribute
            where T2 : System.Attribute => item.MissingAttributeAll<T1, T2, T2, T2, T2>();

        /// <summary>
        /// Match parameters that do not have an attribute of the specified type
        /// </summary>
        public static IAnalysisParameter MissingAttribute<T>(this IAnalysisParameter item)
            where T : System.Attribute => item.MissingAttribute(typeof(T));

        /// <summary>
        /// Match parameters that do not have an attribute of the specified type
        /// </summary>
        public static IAnalysisParameter MissingAttribute(this IAnalysisParameter item, System.Type attributeType)
        {
            attributeType.ThrowIfNotAttribute();
            var input = item as AnalysisParameter;
            input.Parameters = input.Parameters.Where(x => x.Parameter.GetCustomAttribute(attributeType) is null);
            return item;
        }

        public static IAnalysisParameter Is<T>(this IAnalysisParameter item)
            => item.Is(typeof(T));

        public static IAnalysisParameter Is(this IAnalysisParameter item, System.Type type)
        {
            var input = item as AnalysisParameter;
            input.Parameters = input.Parameters.Where(x => x.Parameter.ParameterType == type);
            return item;
        }

        /// <summary>
        /// Match parameters of the specified type or a derivation thereof
        /// </summary>
        public static IAnalysisParameter Implements<T>(this IAnalysisParameter item)
            => item.Implements(typeof(T));

        /// <summary>
        /// Match parameters of the specified type or a derivation thereof
        /// </summary>
        public static IAnalysisParameter Implements(this IAnalysisParameter item, System.Type type)
        {
            var input = item as AnalysisParameter;
            input.Parameters = input.Parameters.Where(x => type.IsAssignableFrom(x.Parameter.ParameterType));
            return item;
        }

        /// <summary>
        /// Match parameters that are not of the specified type or a derivation thereof
        /// </summary>
        public static IAnalysisParameter DoesNotImplement<T>(this IAnalysisParameter item)
            => item.DoesNotImplement(typeof(T));

        /// <summary>
        /// Match parameters that are not of the specified type or a derivation thereof
        /// </summary>
        public static IAnalysisParameter DoesNotImplement(this IAnalysisParameter item, System.Type type)
        {
            var input = item as AnalysisParameter;
            input.Parameters = input.Parameters.Where(x => !type.IsAssignableFrom(x.Parameter.ParameterType));
            return item;
        }

        public static IAnalysisParameter IsNot<T>(this IAnalysisParameter item)
            => item.IsNot(typeof(T));

        public static IAnalysisParameter IsNot(this IAnalysisParameter item, System.Type type)
        {
            var input = item as AnalysisParameter;
            input.Parameters = input.Parameters.Where(x => x.Parameter.ParameterType != type);
            return item;
        }

        public static IAnalysisParameter IsNotAny(this IAnalysisParameter item, params System.Type[] types)
        {
            var input = item as AnalysisParameter;
            input.Parameters = input.Parameters.Where(x => !types.Contains(x.Parameter.ParameterType));
            return item;
        }

        /// <summary>
        /// Filter to only return value type objects
        /// </summary>
        public static IAnalysisParameter ValueTypes(this IAnalysisParameter item)
        {
            var input = item as AnalysisParameter;
            input.Parameters = input.Parameters.Where(x => x.Parameter.ParameterType.IsValueType);
            return item;
        }

        /// <summary>
        /// Filter to only return reference type objects
        /// </summary>
        public static IAnalysisParameter ReferenceTypes(this IAnalysisParameter item)
        {
            var input = item as AnalysisParameter;
            input.Parameters = input.Parameters.Where(x => !x.Parameter.ParameterType.IsValueType);
            return item;
        }

        /// <summary>
        /// Filter parameters of any of the defined specific types
        /// </summary>
        public static IAnalysisParameter IsAny(this IAnalysisParameter item, params System.Type[] types)
        {
            var input = item as AnalysisParameter;
            input.Parameters = input.Parameters.Where(x => types.Contains(x.Parameter.ParameterType));
            return item;
        }

        /// <summary>
        /// Match parameters with an exact name
        /// </summary>
        public static IAnalysisParameter NameIs(this IAnalysisParameter item, string str)
        {
            var input = item as AnalysisParameter;
            input.Parameters = input.Parameters.Where(x => x.Parameter.Name == str);
            return item;
        }

        /// <summary>
        /// Match parameters with names that contains a sub-string.
        /// </summary>
        public static IAnalysisParameter NameContains(this IAnalysisParameter item, string str)
        {
            var input = item as AnalysisParameter;
            input.Parameters = input.Parameters.Where(x => x.Parameter.Name.ToLowerInvariant().Contains(str.ToLowerInvariant()));
            return item;
        }

        /// <summary>
        /// Match parameters with names that start with a sub-string.
        /// </summary>
        public static IAnalysisParameter NameStartsWith(this IAnalysisParameter item, string str)
        {
            var input = item as AnalysisParameter;
            input.Parameters = input.Parameters.Where(x => x.Parameter.Name.StartsWith(str, StringComparison.InvariantCultureIgnoreCase));
            return item;
        }

        /// <summary>
        /// Match parameters with names that end with a sub-string.
        /// </summary>
        public static IAnalysisParameter NameEndsWith(this IAnalysisParameter item, string str)
        {
            var input = item as AnalysisParameter;
            input.Parameters = input.Parameters.Where(x => x.Parameter.Name.EndsWith(str, StringComparison.InvariantCultureIgnoreCase));
            return item;
        }

        /// <summary>
        /// Match parameters with names that do not contain a sub-string.
        /// </summary>
        public static IAnalysisParameter NameDoesNotContain(this IAnalysisParameter item, string str)
        {
            var input = item as AnalysisParameter;
            input.Parameters = input.Parameters.Where(x => !x.Parameter.Name.ToLowerInvariant().Contains(str.ToLowerInvariant()));
            return item;
        }

        /// <summary>
        /// Match parameters with names that do not start with a sub-string.
        /// </summary>
        public static IAnalysisParameter NameDoesNotStartWith(this IAnalysisParameter item, string str)
        {
            var input = item as AnalysisParameter;
            input.Parameters = input.Parameters.Where(x => !x.Parameter.Name.StartsWith(str, StringComparison.InvariantCultureIgnoreCase));
            return item;
        }

        /// <summary>
        /// Match parameters with names that do not end with a sub-string.
        /// </summary>
        public static IAnalysisParameter NameDoesNotEndWith(this IAnalysisParameter item, string str)
        {
            var input = item as AnalysisParameter;
            input.Parameters = input.Parameters.Where(x => !x.Parameter.Name.EndsWith(str, StringComparison.InvariantCultureIgnoreCase));
            return item;
        }

        /// <summary>
        /// Match parameters with types that are an interface
        /// </summary>
        public static IAnalysisParameter IsInterface(this IAnalysisParameter item, bool value = true)
        {
            var input = item as AnalysisParameter;
            input.Parameters = input.Parameters.Where(x => x.Type.IsInterface == value);
            return item;
        }

        /// <summary>
        /// Return the declaring parent types on which the filtered set of parameters exist
        /// </summary>
        public static IAnalysisType DeclaringTypes(this IAnalysisParameter item)
        {
            var input = item as AnalysisParameter;
            return new AnalysisType(input.Parameters.Select(x => x.Parameter.Member.DeclaringType).Distinct());
        }

        #endregion

        #region Property

        public static IAnalysisProperty Properties(this IAnalysisType item) => item.Properties(BindingFlags.Public);
        public static IAnalysisProperty Properties(this IAnalysisType item, BindingFlags bindingFlags)
        {
            return new AnalysisProperty(item as AnalysisType, bindingFlags);
        }

        /// <summary>
        /// Filter properties of a specific type
        /// </summary>
        public static IAnalysisProperty Is<T>(this IAnalysisProperty item)
            => item.Is(typeof(T));

        public static IAnalysisProperty Is(this IAnalysisProperty item, System.Type type)
        {
            var input = item as AnalysisProperty;
            input.Properties = input.Properties.Where(x => x.Property.PropertyType == type);
            return item;
        }

        /// <summary>
        /// Filter properties of any of the defined specific types
        /// </summary>
        public static IAnalysisProperty IsAny(this IAnalysisProperty item, params System.Type[] types)
        {
            var input = item as AnalysisProperty;
            input.Properties = input.Properties.Where(x => types.Contains(x.Property.PropertyType));
            return item;
        }

        public static IAnalysisProperty DoesNotImplement<T>(this IAnalysisProperty item)
            => item.DoesNotImplement(typeof(T));

        public static IAnalysisProperty DoesNotImplement(this IAnalysisProperty item, System.Type type)
        {
            var input = item as AnalysisProperty;
            input.Properties = input.Properties.Where(x => !type.IsAssignableFrom(x.Property.PropertyType));
            return item;
        }

        public static IAnalysisProperty IsNot<T>(this IAnalysisProperty item)
            => item.IsNot(typeof(T));

        public static IAnalysisProperty IsNot(this IAnalysisProperty item, System.Type type)
        {
            var input = item as AnalysisProperty;
            input.Properties = input.Properties.Where(x => x.Property.PropertyType != type);
            return item;
        }

        /// <summary>
        /// Filter to only return properties that are an Enum type
        /// </summary>
        public static IAnalysisProperty IsEnum(this IAnalysisProperty item)
        {
            var input = item as AnalysisProperty;
            input.Properties = input.Properties.Where(x => x.Property.PropertyType.IsEnum || x.Property.PropertyType.IsNullableEnum());
            return item;
        }

        /// <summary>
        /// Match only properties that have a public Get method
        /// </summary>
        public static IAnalysisProperty IsReadable(this IAnalysisProperty item) => item.IsReadable(true);

        /// <summary>
        /// Match only properties that have/have not a public Get method based on the specified value
        /// </summary>
        public static IAnalysisProperty IsReadable(this IAnalysisProperty item, bool value)
        {
            var input = item as AnalysisProperty;
            if (value)
                input.Properties = input.Properties.Where(x => x.Property.GetGetMethod() != null);
            else
                input.Properties = input.Properties.Where(x => x.Property.GetGetMethod() == null);
            return item;
        }

        /// <summary>
        /// Match only properties that have a public Set method
        /// </summary>
        public static IAnalysisProperty IsWritable(this IAnalysisProperty item) => item.IsWritable(true);

        /// <summary>
        /// Match only properties that have/have not a public Set method based on the specified value
        /// </summary>
        public static IAnalysisProperty IsWritable(this IAnalysisProperty item, bool value)
        {
            var input = item as AnalysisProperty;
            if (value)
                input.Properties = input.Properties.Where(x => x.Property.GetSetMethod() != null);
            else
                input.Properties = input.Properties.Where(x => x.Property.GetSetMethod() == null);
            return item;
        }

        /// <summary>
        /// Filter to only return value type objects
        /// </summary>
        public static IAnalysisProperty ValueTypes(this IAnalysisProperty item)
        {
            var input = item as AnalysisProperty;
            input.Properties = input.Properties.Where(x => x.Property.PropertyType.IsValueType);
            return item;
        }

        /// <summary>
        /// Filter to only return reference type objects
        /// </summary>
        public static IAnalysisProperty ReferenceTypes(this IAnalysisProperty item)
        {
            var input = item as AnalysisProperty;
            input.Properties = input.Properties.Where(x => !x.Property.PropertyType.IsValueType);
            return item;
        }

        public static IAnalysisProperty IsVirtual(this IAnalysisProperty item) => item.IsVirtual(true);

        /// <summary>
        /// Filter to only return properties that have one or both virtual Get or virtual Set methods
        /// </summary>
        public static IAnalysisProperty IsVirtual(this IAnalysisProperty item, bool value)
        {
            var input = item as AnalysisProperty;
            input.Properties = input.Properties.Where(x => (x.Property.GetGetMethod()?.IsVirtual ?? x.Property.GetSetMethod()?.IsVirtual ?? false) == value);
            return item;
        }

        public static IAnalysisProperty GenericTypes(this IAnalysisProperty item)
        {
            var input = item as AnalysisProperty;
            input.Properties = input.Properties.Where(x => x.Property.PropertyType.IsGenericType);
            return item;
        }

        public static IAnalysisProperty WithAttribute<T>(this IAnalysisProperty item)
            where T : System.Attribute
        {
            var input = item as AnalysisProperty;
            input.Properties = input.Properties.Where(x => x.Property.GetCustomAttribute<T>() != null);
            return item;
        }

        public static IAnalysisProperty WithAttribute<T>(this IAnalysisProperty item, Func<T, bool> predicate)
            where T : System.Attribute
        {
            var input = item as AnalysisProperty;
            input.Properties = input.Properties.Where(x => x.Property.GetCustomAttributes<T>().Cast<T>().Where(predicate).Any());
            return item;
        }

        public static IAnalysisProperty WithAttribute(this IAnalysisProperty item, System.Type attributeType)
        {
            var input = item as AnalysisProperty;
            input.Properties = input.Properties.Where(x => x.Property.GetCustomAttribute(attributeType) != null);
            return item;
        }

        public static IAnalysisProperty MissingAttribute<T>(this IAnalysisProperty item)
           where T : System.Attribute => item.MissingAttribute(typeof(T));

        public static IAnalysisProperty MissingAttribute(this IAnalysisProperty item, System.Type attributeType)
        {
            attributeType.ThrowIfNotAttribute();
            var input = item as AnalysisProperty;
            input.Properties = input.Properties.Where(x => x.Property.GetCustomAttribute(attributeType) is null);
            return item;
        }

        public static IAnalysisProperty MissingAttributeAll(this IAnalysisProperty item, params System.Type[] types)
        {
            var input = item as AnalysisProperty;
            input.Properties = input.Properties.Where(x => x.Property.GetCustomAttributes().Select(z => z.GetType()).Intersect(types).Count() == 0);
            return item;
        }

        public static IAnalysisProperty MissingAttributeAll<T1, T2>(this IAnalysisProperty item)
            where T1 : System.Attribute
            where T2 : System.Attribute
        {
            return item.MissingAttributeAll(typeof(T1), typeof(T2));
        }

        public static IAnalysisProperty MissingAttributeAll<T1, T2, T3>(this IAnalysisProperty item)
            where T1 : System.Attribute
            where T2 : System.Attribute
            where T3 : System.Attribute
        {
            return item.MissingAttributeAll(typeof(T1), typeof(T2), typeof(T3));
        }

        /// <summary>
        /// Match parameters with an exact name
        /// </summary>
        public static IAnalysisProperty NameIs(this IAnalysisProperty item, string name)
        {
            var input = item as AnalysisProperty;
            input.Properties = input.Properties.Where(x => x.Property.Name == name);
            return item;
        }

        public static IAnalysisProperty NameIsNot(this IAnalysisProperty item, string name)
        {
            var input = item as AnalysisProperty;
            input.Properties = input.Properties.Where(x => !x.Property.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            return item;
        }

        public static IAnalysisProperty NameContains(this IAnalysisProperty item, string str)
        {
            var input = item as AnalysisProperty;
            input.Properties = input.Properties.Where(x => x.Property.Name.ToLowerInvariant().Contains(str.ToLowerInvariant()));
            return item;
        }

        /// <summary>
        /// Match properties with names that start with a sub-string.
        /// </summary>
        public static IAnalysisProperty NameStartsWith(this IAnalysisProperty item, string str)
        {
            var input = item as AnalysisProperty;
            input.Properties = input.Properties.Where(x => x.Property.Name.StartsWith(str, StringComparison.InvariantCultureIgnoreCase));
            return item;
        }

        /// <summary>
        /// Match properties with names that end with a sub-string.
        /// </summary>
        public static IAnalysisProperty NameEndsWith(this IAnalysisProperty item, string str)
        {
            var input = item as AnalysisProperty;
            input.Properties = input.Properties.Where(x => x.Property.Name.EndsWith(str, StringComparison.InvariantCultureIgnoreCase));
            return item;
        }

        /// <summary>
        /// Match properties with names that do not contain a sub-string.
        /// </summary>
        public static IAnalysisProperty NameDoesNotContain(this IAnalysisProperty item, string str)
        {
            var input = item as AnalysisProperty;
            input.Properties = input.Properties.Where(x => !x.Property.Name.ToLowerInvariant().Contains(str.ToLowerInvariant()));
            return item;
        }

        /// <summary>
        /// Match properties with names that do not start with a sub-string.
        /// </summary>
        public static IAnalysisProperty NameDoesNotStartWith(this IAnalysisProperty item, string str)
        {
            var input = item as AnalysisProperty;
            input.Properties = input.Properties.Where(x => !x.Property.Name.StartsWith(str, StringComparison.InvariantCultureIgnoreCase));
            return item;
        }

        /// <summary>
        /// Match properties with names that do not end with a sub-string.
        /// </summary>
        public static IAnalysisProperty NameDoesNotEndWith(this IAnalysisProperty item, string str)
        {
            var input = item as AnalysisProperty;
            input.Properties = input.Properties.Where(x => !x.Property.Name.EndsWith(str, StringComparison.InvariantCultureIgnoreCase));
            return item;
        }

        /// <summary>
        /// Match properties with types that are not nullable
        /// </summary>
        public static IAnalysisProperty IsNullable(this IAnalysisProperty item) => item.IsNullable(true);

        public static IAnalysisProperty IsNullable(this IAnalysisProperty item, bool value)
        {
            var input = item as AnalysisProperty;
            input.Properties = input.Properties.Where(x => (Nullable.GetUnderlyingType(x.Property.PropertyType) != null) == value);
            return item;
        }

        #endregion

        #region Fields

        public static IAnalysisField Fields(this IAnalysisType item) => item.Fields(BindingFlags.Public);
        public static IAnalysisField Fields(this IAnalysisType item, BindingFlags bindingFlags)
        {
            return new AnalysisField(item as AnalysisType, bindingFlags);
        }

        public static IAnalysisConst Consts(this IAnalysisType item) => item.Consts(BindingFlags.Public | BindingFlags.Static);
        public static IAnalysisConst Consts(this IAnalysisType item, BindingFlags bindingFlags)
        {
            return new AnalysisConst(item as AnalysisType, bindingFlags | BindingFlags.Static);
        }

        /// <summary>
        /// Filter fields of a specific type
        /// </summary>
        public static IAnalysisField Is<T>(this IAnalysisField item)
            => item.Is(typeof(T));

        public static IAnalysisField Is(this IAnalysisField item, System.Type type)
        {
            var input = item as AnalysisField;
            input.Fields = input.Fields.Where(x => x.Field.FieldType == type);
            return item;
        }

        /// <summary>
        /// Filter Fields of any of the defined specific types
        /// </summary>
        public static IAnalysisField IsAny(this IAnalysisField item, params System.Type[] types)
        {
            var input = item as AnalysisField;
            input.Fields = input.Fields.Where(x => types.Contains(x.Field.FieldType));
            return item;
        }

        public static IAnalysisField DoesNotImplement<T>(this IAnalysisField item)
            => item.DoesNotImplement(typeof(T));

        public static IAnalysisField DoesNotImplement(this IAnalysisField item, System.Type type)
        {
            var input = item as AnalysisField;
            input.Fields = input.Fields.Where(x => !type.IsAssignableFrom(x.Field.FieldType));
            return item;
        }

        public static IAnalysisField IsNot<T>(this IAnalysisField item)
            => item.IsNot(typeof(T));

        public static IAnalysisField IsNot(this IAnalysisField item, System.Type type)
        {
            var input = item as AnalysisField;
            input.Fields = input.Fields.Where(x => x.Field.FieldType != type);
            return item;
        }

        /// <summary>
        /// Filter to only return Fields that are an Enum type
        /// </summary>
        public static IAnalysisField IsEnum(this IAnalysisField item)
        {
            var input = item as AnalysisField;
            input.Fields = input.Fields.Where(x => x.Field.FieldType.IsEnum || x.Field.FieldType.IsNullableEnum());
            return item;
        }

        #endregion

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
        public static System.Type[] ToArray(this IAnalysisType item)
        {
            return (item as AnalysisType).Types.ToArray();
        }

        public static MatchParameter[] ToArray(this IAnalysisParameter item)
        {
            return (item as AnalysisParameter).Parameters.ToArray();
        }

        public static MatchMethod[] ToArray(this IAnalysisMethod item)
        {
            return (item as AnalysisMethod).Methods.ToArray();
        }

        public static MatchProperty[] ToArray(this IAnalysisProperty item)
        {
            return (item as AnalysisProperty).Properties.ToArray();
        }

        public static MatchField[] ToArray(this IAnalysisField item)
        {
            return (item as AnalysisField).Fields.ToArray();
        }

        public static MatchConstValue<T>[] Values<T>(this IAnalysisConst item)
        {
            var retval = new List<MatchConstValue<T>>();
            var list = (item as AnalysisField).Fields.ToList();
            foreach (var el in list.Where(x => typeof(T).IsAssignableFrom(x.Field.FieldType)))
            {
                retval.Add(new MatchConstValue<T>
                {
                    Field = el.Field,
                    Type = el.Type,
                    Value = (T)el.Field.GetValue(null),
                });
            }
            return retval.ToArray();
        }

        public static AnalysisValidateResult DoesNotReturnErrors<T>(this IAnalysisMethod item)
            => item.DoesNotReturnErrors(typeof(T));

        public static AnalysisValidateResult DoesNotReturnErrors(this IAnalysisMethod item, System.Type type)
        {
            var input = item as AnalysisMethod;
            var r = new AnalysisValidateResult
            {
                Errors = input.Methods.Select(x => new AnalysisError { Text = $"The method {x.Type.Name}.{x.Method.Name} does not return {type.Name}" }).ToList(),
            };
            return r;
        }

        public static AnalysisValidateResult PropertyMissingAttributeErrors<T>(this IAnalysisProperty item)
            => item.PropertyMissingAttributeErrors(typeof(T));

        public static AnalysisValidateResult PropertyMissingAttributeErrors(this IAnalysisProperty item, System.Type type)
        {
            var input = item as AnalysisProperty;
            var r = new AnalysisValidateResult
            {
                Errors = input.Properties.Select(x => new AnalysisError { Text = $"The property {x.Type.Name}.{x.Property.Name} is missing attribute {type.Name}" }).ToList(),
            };
            return r;
        }

        public static bool IsNullableEnum(this Type t) => Nullable.GetUnderlyingType(t)?.IsEnum ?? false;

        internal static void ThrowIfNotAttribute(this Type t)
        {
            if (!typeof(System.Attribute).IsAssignableFrom(t))
                throw new InvalidCastException($"'{t.Name}' must be of type System.Attribute.");
        }
    }
}
