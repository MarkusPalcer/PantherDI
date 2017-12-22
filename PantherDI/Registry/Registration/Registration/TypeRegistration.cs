using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PantherDI.Attributes;
using PantherDI.Extensions;
using PantherDI.Registry.Registration.Factory;

namespace PantherDI.Registry.Registration.Registration
{
    public class TypeRegistration : IRegistration

    {
        private readonly Dictionary<string, object> _metadata = new Dictionary<string, object>();

        public TypeRegistration(TypeInfo type) : this(type, type.GetFulfilledContracts()) { }

        public TypeRegistration(TypeInfo type, IEnumerable<object> fulfilledContracts)
        {
            RegisteredType = type.AsType();
            FulfilledContracts = new HashSet<object>(fulfilledContracts);
            var constructorFactories = type.DeclaredConstructors
                                           .Where(x => !x.GetCustomAttributes<IgnoreAttribute>().Any())
                                           .Select(x => new ConstructorFactory(x));
            Factories = new HashSet<IFactory>(constructorFactories);
            Singleton = type.GetCustomAttributes<SingletonAttribute>().Any();
            CollectMetadata(RegisteredType, _metadata);
        }

        internal static void CollectMetadata(Type registeredType, Dictionary<string, object> metadata)
        {
            var typeHierarchy = registeredType
                .GetTypeHierarchy()
                .Select(x => x.GetTypeInfo())
                .Reverse();

            foreach (var type in typeHierarchy)
            {
                foreach (var attribute in type.GetCustomAttributes<MetadataAttribute>().Where(x => x.HasValue))
                {
                    metadata[attribute.Key] = attribute.Value;
                }

                var fieldMetadata = type.DeclaredFields.Where(x => x.IsStatic && !x.IsSpecialName)
                    .SelectManyForPairs(x => x.GetCustomAttributes<MetadataAttribute>(),
                                        (f, a) => new MetadataAttribute(a.Key ?? f.Name, f.GetValue(null)));

                foreach (var attribute in fieldMetadata)
                {
                    metadata[attribute.Key] = attribute.Value;
                }

                var propertyMetadata = type.DeclaredProperties.Where(x => x.GetMethod.IsStatic)
                                           .SelectManyForPairs(x => x.GetCustomAttributes<MetadataAttribute>(),
                                                               (p, a) => new MetadataAttribute(a.Key ?? p.Name, p.GetValue(null)));

                foreach (var attribute in propertyMetadata)
                {
                    metadata[attribute.Key] = attribute.Value;
                }
            }
        }

        public Type RegisteredType { get; }
        public ISet<object> FulfilledContracts { get; }
        public ISet<IFactory> Factories { get; }
        public bool Singleton { get; }

        public IReadOnlyDictionary<string, object> Metadata => _metadata;

        public static TypeRegistration Create<T>() 
        {
            return new TypeRegistration(typeof(T).GetTypeInfo());
        }
    }
}