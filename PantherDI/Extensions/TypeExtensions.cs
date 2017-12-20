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
                           .SelectMany(t => t.GetTypeInfo().GetCustomAttributes<ContractAttribute>().Select(attr => Tuple.Create(t, attr)))
                           .Select(x => x.Item2.Contract ?? x.Item1);
        }
    }
}