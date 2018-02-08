using System;
using System.Collections.Generic;
using System.Reflection;
using PantherDI.Registry.Catalog;
using PantherDI.Registry.Registration.Factory;
using PantherDI.Registry.Registration.Registration;

namespace PantherDI.ContainerCreation
{
    public interface ICatalogBuilder
    {
        /// <summary>
        /// Builds the catalog containing everything that has been added before
        /// </summary>
        /// <returns></returns>
        ICatalog Build();

        /// <summary>
        /// Adds a catalog
        /// </summary>
        ICatalogBuilder WithCatalog(ICatalog catalog);

        /// <summary>
        /// Uses reflection to add the content of an assembly
        /// </summary>
        /// <param name="assembly">The assembly to add</param>
        ICatalogBuilder WithAssembly(Assembly assembly);

        /// <summary>
        /// Uses reflection to add the content of the assembly that contains the given type
        /// </summary>
        ICatalogBuilder WithAssemblyOf(Type type);

        /// <summary>
        /// Uses reflection to add the content of the assembly that contains the given type
        /// </summary>
        ICatalogBuilder WithAssemblyOf<T>();

        /// <summary>
        /// Uses reflection to add a type
        /// </summary>
        ICatalogBuilder WithType(Type type);

        /// <summary>
        /// Uses reflection to add a type
        /// </summary>
        ICatalogBuilder WithType<T>();

        /// <summary>
        /// Adds a registration
        /// </summary>
        ICatalogBuilder WithRegistration(IRegistration registration);

        /// <summary>
        /// Adds an instance
        /// </summary>
        ICatalogBuilder WithInstance<T>(T instance);

        /// <summary>
        /// Adds a factory
        /// </summary>
        ICatalogBuilder WithFactory<T>(IFactory factory);
    }
}