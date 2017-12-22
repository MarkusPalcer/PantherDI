using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PantherDI.Attributes;

namespace PantherDI.Extensions
{
    public static class TypeExtensions
    {
        public static IEnumerable<Type> GetTypeHierarchy(this Type type, Type rootType = null)
        {
            rootType = rootType ?? typeof(object);

            while (type != rootType && type != null)
            {
                yield return type;

                type = type.GetTypeInfo().BaseType;
            }

            if (type != null)
                yield return type;
        }

        public static IEnumerable<TOut> SelectAttributes<TAttribute, TOut>(this IEnumerable<Type> types, Func<Type, TAttribute, TOut> selector) where TAttribute : Attribute
        {
            return types.SelectMany(t => t.GetTypeInfo().GetCustomAttributes<TAttribute>().Select(a => selector(t, a)));
        }

        public static IEnumerable<object> GetFulfilledContracts(this TypeInfo type)
        {
            var result = type.ScanForContracts().ToArray();

            return result.Length == 0 ? new object[] {type} : result.Distinct();
        }

        public static IEnumerable<object> ScanForContracts(this TypeInfo typeInfo)
        {
            return typeInfo.AsType()
                           .GetTypeHierarchy()
                           .Concat(typeInfo.ImplementedInterfaces)
                           .SelectAttributes<ContractAttribute, object>((type, attribute) => attribute.Contract ?? type);
        }
    }
}