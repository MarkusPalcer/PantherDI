﻿using System;
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
            Factories = new HashSet<IFactory>(type.DeclaredConstructors.Select(x => new ConstructorFactory(x)));
            Singleton = type.GetCustomAttributes<SingletonAttribute>().Any();
        }

        public Type RegisteredType { get; }
        public ISet<object> FulfilledContracts { get; }
        public ISet<IFactory> Factories { get; }
        public bool Singleton { get; }

        public static TypeRegistration Create<T>() 
        {
            return new TypeRegistration(typeof(T).GetTypeInfo());
        }
    }
}