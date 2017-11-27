using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PantherDI.Attributes;

namespace PantherDI.Extensions
{
    public static class TypeExtensions
    {
        public static IEnumerable<object> GetFulfilledContracts(this TypeInfo type)
        {
            var result = type.ScanForContracts().ToArray();

            return result.Length == 0 ? new object[] {type} : result.Distinct();
        }

        public static IEnumerable<object> ScanForContracts(this TypeInfo typeInfo)
        {
            foreach (var attribute in typeInfo.GetCustomAttributes<ContractAttribute>())
            {
                yield return attribute.Contract ?? typeInfo.AsType();
            }

            foreach (var implementedInterface in typeInfo.ImplementedInterfaces)
            {
                foreach (var contract in ScanForContracts(implementedInterface.GetTypeInfo()))
                {
                    yield return contract;
                }
            }

            if (typeInfo.AsType() != typeof(object) && typeInfo.BaseType != null)
            {
                foreach (var contract in ScanForContracts(typeInfo.BaseType.GetTypeInfo()))
                {
                    yield return contract;
                }
            }
        }
    }
}