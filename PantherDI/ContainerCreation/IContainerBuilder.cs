using System;
using System.Collections.Generic;
using System.Reflection;
using PantherDI.ContainerCreation.CatalogBuilderHelpers;
using PantherDI.Registry.Catalog;
using PantherDI.Registry.Registration.Factory;
using PantherDI.Registry.Registration.Registration;
using PantherDI.Resolvers;

namespace PantherDI.ContainerCreation
{
    public interface IContainerBuilder
    {
        List<ICatalog> Catalogs { get; }
        List<IResolver> Resolvers { get; }
        List<IRegistration> Registrations { get; }
        bool IsStrict { get; set; }
        bool UseLateProcessing { get; set; }
        List<Type> Types { get; }

        /// <summary>
        /// Creates the container configured so far
        /// </summary>
        IContainer Build();

        /// <summary>
        /// Adds a catalog to the container configuration
        /// </summary>
        /// <returns>The ContainerBuilder for fluent access</returns>
        ContainerBuilder WithCatalog(ICatalog catalog);

        /// <summary>
        /// Adds a resolver to the container configuration
        /// </summary>
        /// <returns>The ContainerBuilder for fluent access</returns>
        ContainerBuilder WithResolver(IResolver resolver);

        /// <summary>
        /// Removes a resolver type from the configuration
        /// </summary>
        /// <typeparam name="TResolverToRemove">The type of resolver to remove</typeparam>
        /// <returns>The ContainerBuilder for fluent access</returns>
        ContainerBuilder WithoutResolver<TResolverToRemove>();

        /// <summary>
        /// Adds all resolvers for generic types to the configuration
        /// </summary>
        /// <returns>The ContainerBuilder for fluent access</returns>
        ContainerBuilder WithGenericResolvers();

        /// <summary>
        /// Switches to non-strict handling of requests for not registered types.
        /// 
        /// In non-strict mode the container will register an unregistered type as soon as its being resolved.
        /// </summary>
        ContainerBuilder WithSupportForUnregisteredTypes();

        /// <summary>
        /// Switches to strict handling of requests for not registered types.
        /// 
        /// In strict mode the container will throw an exception when an unregistered type is being resolved.
        /// </summary>
        ContainerBuilder WithStrictRegistrationHandling();

        /// <summary>
        /// Uses reflection to add the content of an assembly to the container.
        /// </summary>
        /// <param name="assembly">The assembly to add</param>
        ContainerBuilder WithAssembly(Assembly assembly);

        /// <summary>
        /// Uses reflection to add the content of the assembly that contains the given type
        /// </summary>
        ContainerBuilder WithAssemblyOf(Type type);

        /// <summary>
        /// Uses reflection to add the content of the assembly that contains the given type
        /// </summary>
        ContainerBuilder WithAssemblyOf<T>();

        /// <summary>
        /// Uses reflection to add a type to the container
        /// </summary>
        ContainerBuilder WithType(Type type);

        /// <summary>
        /// Uses reflection to add a type to the container
        /// </summary>
        ContainerBuilder WithType<T>();

        /// <summary>
        /// Adds a registration to the container
        /// </summary>
        ContainerBuilder WithRegistration(IRegistration registration);

        /// <summary>
        /// Adds an instance to the container
        /// </summary>
        ContainerBuilder WithInstance<T>(T instance);

        /// <summary>
        /// Adds resolvers for factory functions with 0-16 parameters
        /// </summary>
        ContainerBuilder WithFuncResolvers();

        /// <summary>
        /// Enables late processing on the container
        /// 
        /// This means registrations are processed on the first request
        /// </summary>
        ContainerBuilder WithLateProcessing();

        /// <summary>
        /// Adds a factory to the container
        /// </summary>
        ContainerBuilder WithFactory<T>(IFactory factory);

        /// <summary>
        /// Registers a type to the container
        /// </summary>
        TypeCatalogBuilderHelper Register<T>();

        /// <summary>
        /// Registers a type to the container
        /// </summary>
        TypeCatalogBuilderHelper Register(Type t);

        /// <summary>
        /// Registers an instance to the container
        /// </summary>
        InstanceCatalogBuilderHelper<T> Register<T>(T instance);
        }
}