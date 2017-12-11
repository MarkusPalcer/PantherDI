using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PantherDI.Registry.Catalog;
using PantherDI.Registry.Registration;
using PantherDI.Registry.Registration.Factory;
using PantherDI.Registry.Registration.Registration;
using PantherDI.Resolvers;

namespace PantherDI.ContainerCreation
{
    /// <summary>
    /// Helper class to create a container from a configuration
    /// </summary>
    public class ContainerBuilder : IEnumerable
    {
        public List<ICatalog> Catalogs { get; } = new List<ICatalog>();

        public List<IResolver> Resolvers { get; } = new List<IResolver>();

        public List<IRegistration> Registrations { get; } = new List<IRegistration>();

        private List<TypeRegistrationHelper> TypeRegistrationHelpers { get; } = new List<TypeRegistrationHelper>();

        public bool IsStrict { get; set; } = true;

        public List<Type> Types { get; } = new List<Type>();

        /// <summary>
        /// Creates the container configured so far
        /// </summary>
        public IContainer Build()
        {
            foreach (var typeRegistrationHelper in TypeRegistrationHelpers)
            {
                typeRegistrationHelper.RegisterTo(this);
            }

            var resolvers = Resolvers.ToArray();

            if (!IsStrict)
            {
                resolvers = resolvers.Concat(new IResolver[] {new ReflectionResolver()}).ToArray();
            }

            var catalogs = Catalogs
                .Concat(new ICatalog[] { new TypeCatalog(Types), new ManualCatalog(Registrations.ToArray()) })
                .ToArray();

            var converter = new RegistrationConverter(new MergedCatalog(catalogs), resolvers);
            converter.ProcessAll();

            return new Container(converter.KnowledgeBase, resolvers);
        }

        /// <summary>
        /// Adds a catalog to the container configuration
        /// </summary>
        public void Add(ICatalog catalog)
        {
            Catalogs.Add(catalog);
        }

        /// <summary>
        /// Adds a resolver to the container configuration
        /// </summary>
        public void Add(IResolver resolver)
        {
            Resolvers.Add(resolver);
        }

        public void Add(IRegistration registration)
        {
            Registrations.Add(registration);
        }

        #region Implementation of IEnumerable

        public IEnumerator GetEnumerator()
        {
            return Catalogs.Cast<object>().Concat(Resolvers).Concat(Registrations).GetEnumerator();
        }

        #endregion

        /// <summary>
        /// Adds a catalog to the container configuration
        /// </summary>
        /// <returns>The ContainerBuilder for fluent access</returns>
        public ContainerBuilder WithCatalog(ICatalog catalog)
        {
            Add(catalog);
            return this;
        }

        /// <summary>
        /// Adds a resolver to the container configuration
        /// </summary>
        /// <returns>The ContainerBuilder for fluent access</returns>
        public ContainerBuilder WithResolver(IResolver resolver)
        {
            Add(resolver);
            return this;
        }

        /// <summary>
        /// Removes a resolver type from the configuration
        /// </summary>
        /// <typeparam name="TResolverToRemove">The type of resolver to remove</typeparam>
        /// <returns>The ContainerBuilder for fluent access</returns>
        public ContainerBuilder WithoutResolver<TResolverToRemove>()
        {
            Resolvers.RemoveAll(x => x.GetType() == typeof(TResolverToRemove));
            return this;
        }

        /// <summary>
        /// Adds all resolvers for generic types to the configuration
        /// </summary>
        /// <returns>The ContainerBuilder for fluent access</returns>
        public ContainerBuilder WithGenericResolvers()
        {
            return this.WithResolver(new EnumerableResolver())
                .WithResolver(new Func0Resolver())
                .WithResolver(new Func1Resolver())
                .WithResolver(new LazyResolver());
        }

        public ContainerBuilder WithSupportForUnregisteredTypes()
        {
            IsStrict = false;
            return this;
        }

        public ContainerBuilder WithStrictRegistrationHandling()
        {
            IsStrict = true;
            return this;
        }

        public ContainerBuilder WithAssembly(Assembly assembly)
        {
            return WithCatalog(new AssemblyCatalog(assembly));
        }

        public ContainerBuilder WithAssemblyOf(Type type)
        {
            return WithAssembly(type.GetTypeInfo().Assembly);
        }

        public ContainerBuilder WithAssemblyOf<T>()
        {
            return WithAssemblyOf(typeof(T));
        }

        public ContainerBuilder WithType(Type type)
        {
            Types.Add(type);
            return this;
        }

        public ContainerBuilder WithType<T>()
        {
            return WithType(typeof(T));
        }

        public ContainerBuilder WithRegistration(IRegistration registration)
        {
            Add(registration);
            return this;
        }

        public TypeRegistrationHelper Register<T>()
        {
            var result = new TypeRegistrationHelper(typeof(T));
            TypeRegistrationHelpers.Add(result);
            return result;
        }

        public class TypeRegistrationHelper
        {
            private readonly Type _type;

            public TypeRegistrationHelper(Type type)
            {
                _type = type;
                _registration = new ManualRegistration()
                {
                    RegisteredType = _type
                };
            }

            private readonly ManualRegistration _registration;

            public ISet<object> Contracts => _registration.FulfilledContracts;

            public bool RegisterWithReflection { get; set; } = false;

            public ISet<IFactory> Factories => _registration.Factories;

            public bool RegisterWithConstructors { get; set; } = false;

            public bool IsSingleton { get; set; } = false;

            internal void RegisterTo(ContainerBuilder cb)
            {
                if (RegisterWithReflection)
                {
                    cb.WithType(_type);
                }

                if (RegisterWithConstructors)
                {
                    foreach (var factory in _type.GetTypeInfo().DeclaredConstructors.Select(x => new ConstructorFactory(x)))
                    {
                        Factories.Add(factory);
                    }
                }

                _registration.Singleton = IsSingleton;

                cb.Registrations.Add(_registration);
            }

            public TypeRegistrationHelper As<TContract>()
            {
                if (_type.GetTypeInfo().IsSubclassOf(typeof(TContract)))
                {
                    throw new ArgumentException($"${_type.Name} is not assignable to  ${typeof(TContract).Name}, so it can't be registered as such.");
                }

                Contracts.Add(typeof(TContract));

                return this;
            }

            public TypeRegistrationHelper WithContract(object contract)
            {
                Contracts.Add(contract);

                return this;
            }

            public TypeRegistrationHelper UsingReflection()
            {
                RegisterWithReflection = true;
                return this;
            }

            public TypeRegistrationHelper WithConstructors()
            {
                RegisterWithConstructors = true;
                return this;
            }

            public TypeRegistrationHelper AsSingleton()
            {
                IsSingleton = true;
                return this;
            }

            public TypeRegistrationHelper WithFactory(IFactory factory)
            {
                Factories.Add(factory);
                return this;
            }
        }
    }
}