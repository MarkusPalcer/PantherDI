﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PantherDI.ContainerCreation.CatalogBuilderHelpers;
using PantherDI.Registry.Catalog;
using PantherDI.Registry.Registration.Factory;
using PantherDI.Registry.Registration.Registration;

namespace PantherDI.ContainerCreation
{
    public class CatalogBuilder : ICatalogBuilder
    {
        public List<ICatalog> Catalogs { get; } = new List<ICatalog>();

        public List<IRegistration> Registrations { get; } = new List<IRegistration>();

        private List<ICatalogBuilderHelper> RegistrationHelpers { get; } = new List<ICatalogBuilderHelper>();

        public List<Type> Types { get; } = new List<Type>();

        public ICatalog Build()
        {
            foreach (var typeRegistrationHelper in RegistrationHelpers)
            {
                typeRegistrationHelper.RegisterTo(this);
            }

            var catalogs = Catalogs
                           .Concat(new ICatalog[] { new TypeCatalog(Types), new ManualCatalog(Registrations.ToArray()) })
                           .ToArray();

            return new MergedCatalog(catalogs);
        }

        public TypeCatalogBuilderHelper Register(Type t)
        {
            var result = new TypeCatalogBuilderHelper(t);
            RegistrationHelpers.Add(result);
            return result;
        }

        public TypeCatalogBuilderHelper Register<T>()
        {
            return Register(typeof(T));
        }

        public InstanceCatalogBuilderHelper<T> Register<T>(T instance)
        {
            var instanceRegistrationHelper = new InstanceCatalogBuilderHelper<T>(instance);
            RegistrationHelpers.Add(instanceRegistrationHelper);
            return instanceRegistrationHelper;
        }

        public ICatalogBuilder WithCatalog(ICatalog catalog)
        {
            Catalogs.Add(catalog);
            return this;
        }

        public ICatalogBuilder WithAssembly(Assembly assembly)
        {
            return WithCatalog(new AssemblyCatalog(assembly));
        }

        public ICatalogBuilder WithAssemblyOf(Type type)
        {
            return WithAssembly(type.GetTypeInfo().Assembly);
        }

        public ICatalogBuilder WithAssemblyOf<T>()
        {
            return WithAssemblyOf(typeof(T));
        }

        public ICatalogBuilder WithType(Type type)
        {
            Types.Add(type);
            return this;
        }

        public ICatalogBuilder WithType<T>()
        {
            return WithType(typeof(T));
        }

        public ICatalogBuilder WithRegistration(IRegistration registration)
        {
            Registrations.Add(registration);
            return this;
        }

        public ICatalogBuilder WithInstance<T>(T instance)
        {
            Register(instance);
            return this;
        }

        public ICatalogBuilder WithFactory<T>(IFactory factory)
        {
            Register<T>()
                           .WithFactory(factory);
            return this;
        }
    }
}