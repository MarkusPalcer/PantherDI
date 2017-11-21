using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PantherDI.Attributes;

namespace PantherDI.Extensions
{
    public static class TypeExtensions
    {
        public static IEnumerable<object> GetFulfilledContracts(this System.Type type)
        {
            var result = type.ScanForContracts().ToArray();

            return result.Length == 0 ? new object[] {type} : result.Distinct();
        }

        public static IEnumerable<object> ScanForContracts(this Type type)
        {
            var typeInfo = type.GetTypeInfo();

            foreach (var attribute in typeInfo.GetCustomAttributes<ContractAttribute>())
            {
                yield return attribute.Contract ?? type;
            }

            foreach (var implementedInterface in typeInfo.ImplementedInterfaces)
            {
                foreach (var contract in ScanForContracts(implementedInterface))
                {
                    yield return contract;
                }
            }

            if (type != typeof(object) && typeInfo.BaseType != null)
            {
                foreach (var contract in ScanForContracts(typeInfo.BaseType))
                {
                    yield return contract;
                }
            }
        }
    }
}