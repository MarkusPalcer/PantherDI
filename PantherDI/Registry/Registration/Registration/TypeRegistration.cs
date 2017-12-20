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
        }

        public Type RegisteredType { get; }
        public ISet<object> FulfilledContracts { get; }
        public ISet<IFactory> Factories { get; }
        public bool Singleton { get; }

        public IReadOnlyDictionary<string, object> Metadata { get; } = new Dictionary<string, object>();

        public static TypeRegistration Create<T>() 
        {
            return new TypeRegistration(typeof(T).GetTypeInfo());
        }
    }
}