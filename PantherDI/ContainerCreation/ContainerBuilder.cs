using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PantherDI.Registry.Catalog;
using PantherDI.Registry.Registration.Factory;
using PantherDI.Registry.Registration.Registration;
using PantherDI.Resolvers;
using PantherDI.Resolvers.Aggregation;

namespace PantherDI.ContainerCreation
{
    /// <summary>
    /// Helper class to create a container from a configuration
    /// </summary>
    public class ContainerBuilder : IEnumerable, IContainerBuilder
    {
        internal interface IRegistrationHelper
        {
            void RegisterTo(ContainerBuilder cb);
        }

        public List<ICatalog> Catalogs { get; } = new List<ICatalog>();

        public List<IResolver> Resolvers { get; } = new List<IResolver>();

        public List<IRegistration> Registrations { get; } = new List<IRegistration>();

        private List<IRegistrationHelper> RegistrationHelpers { get; } = new List<IRegistrationHelper>();

        public bool IsStrict { get; set; } = true;

        public bool UseLateProcessing { get; set; }

        public List<Type> Types { get; } = new List<Type>();

        /// <summary>
        /// Creates the container configured so far
        /// </summary>
        public IContainer Build()
        {
            Container result = null;

            /* ReSharper disable once AccessToModifiedClosure
             * Justification: Access to modified closure is desired. Container should resolve itself.
             */
            Register<Container>()
                .As<IContainer>()
                .WithFactory(() => result);

            foreach (var typeRegistrationHelper in RegistrationHelpers)
            {
                typeRegistrationHelper.RegisterTo(this);
            }

            var manuallyRegisteredResolvers = new AllMatchesResolver(Resolvers);

            var containerResolvers = new FirstMatchResolver
            {
                manuallyRegisteredResolvers
            };

            if (!IsStrict)
            {
                manuallyRegisteredResolvers.Add(new ReflectionResolver());
            }

            var catalogs = Catalogs
                .Concat(new ICatalog[] { new TypeCatalog(Types), new ManualCatalog(Registrations.ToArray()) })
                .ToArray();

            var converter = new RegistrationConverter(new MergedCatalog(catalogs), containerResolvers);
            

            IResolver registrationResolver;

            if (!UseLateProcessing)
            {
                registrationResolver = converter.KnowledgeBase;
            }
            else
            {
                registrationResolver = new RegistrationProcessingResolver(converter);
            }

            containerResolvers.Insert(0, registrationResolver);

            if (!UseLateProcessing)
            {
                converter.ProcessAll();
            }

            
            result = new Container(containerResolvers);
            return result;
        }

        private void Add(IRegistrationHelper registrationHelper)
        {
            RegistrationHelpers.Add(registrationHelper);
        }

        #region Obsoleted

        [Obsolete("Use WithResolver(IContainer.AsResolver) instead")]
        public ContainerBuilder WithResolver(IContainer container)
        {
            return WithResolver(container.AsResolver());
        }

        [Obsolete("Enumerating all contents of the builder will be removed in the future")]
        public IEnumerator GetEnumerator()
        {
            return Catalogs.Cast<object>().Concat(Resolvers).Concat(Registrations).GetEnumerator();
        }

        /// <summary>
        /// Adds a catalog to the container configuration
        /// </summary>
        [Obsolete("Please use WithCatalog instead.")]
        public void Add(ICatalog catalog)
        {
            Catalogs.Add(catalog);
        }

        /// <summary>
        /// Adds a resolver to the container configuration
        /// </summary>
        [Obsolete("Please use WithResolver instead.")]
        public void Add(IResolver resolver)
        {
            Resolvers.Add(resolver);
        }

        /// <summary>
        /// Adds a registraion to the container configuration
        /// </summary>
        [Obsolete("Please use WithRegistration instead.")]
        public void Add(IRegistration registration)
        {
            Registrations.Add(registration);
        }
        #endregion

        /// <summary>
        /// Adds a catalog to the container configuration
        /// </summary>
        /// <returns>The ContainerBuilder for fluent access</returns>
        public ContainerBuilder WithCatalog(ICatalog catalog)
        {
            Catalogs.Add(catalog);
            return this;
        }

        /// <summary>
        /// Adds a resolver to the container configuration
        /// </summary>
        /// <returns>The ContainerBuilder for fluent access</returns>
        public ContainerBuilder WithResolver(IResolver resolver)
        {
            Resolvers.Add(resolver);
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
            return WithResolver(new EnumerableResolver())
                .WithResolver(new LazyResolver())
                .WithResolver(new MetadataResolver())
                .WithFuncResolvers();
        }

        /// <summary>
        /// Switches to non-strict handling of requests for not registered types.
        /// 
        /// In non-strict mode the container will register an unregistered type as soon as its being resolved.
        /// </summary>
        public ContainerBuilder WithSupportForUnregisteredTypes()
        {
            IsStrict = false;
            return this;
        }

        /// <summary>
        /// Switches to strict handling of requests for not registered types.
        /// 
        /// In strict mode the container will throw an exception when an unregistered type is being resolved.
        /// </summary>
        public ContainerBuilder WithStrictRegistrationHandling()
        {
            IsStrict = true;
            return this;
        }

        /// <summary>
        /// Uses reflection to add the content of an assembly to the container.
        /// </summary>
        /// <param name="assembly">The assembly to add</param>
        public ContainerBuilder WithAssembly(Assembly assembly)
        {
            return WithCatalog(new AssemblyCatalog(assembly));
        }

        /// <summary>
        /// Uses reflection to add the content of the assembly that contains the given type
        /// </summary>
        public ContainerBuilder WithAssemblyOf(Type type)
        {
            return WithAssembly(type.GetTypeInfo().Assembly);
        }

        /// <summary>
        /// Uses reflection to add the content of the assembly that contains the given type
        /// </summary>
        public ContainerBuilder WithAssemblyOf<T>()
        {
            return WithAssemblyOf(typeof(T));
        }

        /// <summary>
        /// Uses reflection to add a type to the container
        /// </summary>
        public ContainerBuilder WithType(Type type)
        {
            Types.Add(type);
            return this;
        }

        /// <summary>
        /// Uses reflection to add a type to the container
        /// </summary>
        public ContainerBuilder WithType<T>()
        {
            return WithType(typeof(T));
        }

        /// <summary>
        /// Adds a registration to the container
        /// </summary>
        public ContainerBuilder WithRegistration(IRegistration registration)
        {
            Registrations.Add(registration);
            return this;
        }

        /// <summary>
        /// Adds an instance to the container
        /// </summary>
        public ContainerBuilder WithInstance<T>(T instance)
        {
            Register(instance);
            return this;
        }

        /// <summary>
        /// Adds resolvers for factory functions with 0-16 parameters
        /// </summary>
        public ContainerBuilder WithFuncResolvers()
        {
            foreach (var funcResolver in FuncResolvers.All.Select(x => x()))
            {
                Resolvers.Add(funcResolver);
            }

            return this;
        }

        /// <summary>
        /// Enables late processing on the container
        /// 
        /// This means registrations are processed on the first request
        /// </summary>
        public ContainerBuilder WithLateProcessing()
        {
            UseLateProcessing = true;
            return this;
        }

        /// <summary>
        /// Adds a factory to the container
        /// </summary>
        public ContainerBuilder WithFactory<T>(IFactory factory)
        {
            Register<T>()
                .WithFactory(factory);
            return this;
        }

        /// <summary>
        /// Registers a type to the container
        /// </summary>
        public TypeRegistrationHelper Register<T>()
        {
            return Register(typeof(T));
        }

        /// <summary>
        /// Registers a type to the container
        /// </summary>
        public TypeRegistrationHelper Register(Type t)
        {
            var result = new TypeRegistrationHelper(t);
            RegistrationHelpers.Add(result);
            return result;
        }

        /// <summary>
        /// Registers an instance to the container
        /// </summary>
        public InstanceRegistrationHelper<T> Register<T>(T instance)
        {
            var instanceRegistrationHelper = new InstanceRegistrationHelper<T>(instance);
            RegistrationHelpers.Add(instanceRegistrationHelper);
            return instanceRegistrationHelper;
        }

        /// <summary>
        /// Registers a factory to the container
        /// </summary>
        [Obsolete("Use Register<ReturnType>().WithFactory instead.")]
        public FactoryRegistrationHelper Register<T>(IFactory factory)
        {
            var helper = new FactoryRegistrationHelper(typeof(T), factory);
            Add(helper);
            return helper;
        }
    }
}