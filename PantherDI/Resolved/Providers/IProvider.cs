﻿using System;
using System.Collections.Generic;
using PantherDI.Registry.Registration;
using PantherDI.Registry.Registration.Registration;

namespace PantherDI.Resolved.Providers
{
    /// <summary>
    /// Represents a factory which has had its dependencies resolved and is now ready for use within a container
    /// </summary>
    public interface IProvider
    {
        /// <summary>
        /// All dependencies which cannot be resolved by the container
        /// </summary>
        HashSet<Dependency> UnresolvedDependencies { get; }

        /// <summary>
        /// The type of the actual implementation as provided by the <see cref="IRegistration"/>
        /// </summary>
        Type ResultType { get; }

        /// <summary>
        /// The contracts this provider fulfills as provided by the <see cref="IRegistration"/>
        /// </summary>
        ISet<object> FulfilledContracts { get; }

        /// <summary>
        /// Creates an instance of the provided type
        /// </summary>
        /// <param name="resolvedDependencies">The resolved objects for the dependencies which could not be resolved by the container.</param>
        object CreateInstance(Dictionary<Dependency, object> resolvedDependencies);

        /// <summary>
        /// Gets a value indicating whether only a single instance of this registered type should be created
        /// </summary>
        bool Singleton { get; }

        /// <summary>
        /// Gets a dictionary of all metadata associated with the created type
        /// </summary>
        IReadOnlyDictionary<string, object> Metadata { get; }

        /// <summary>
        /// Gets the priority of this provider
        /// </summary>
        int Priority { get; }
    }
}