using System;
using System.Collections.Generic;
using PantherDI.Registry.Registration.Factory;

namespace PantherDI.Registry.Registration.Registration
{
    /// <summary>
    /// Represents a registered type which will be known to a created container
    /// </summary>
    public interface IRegistration
    {
        /// <summary>
        /// Gets the actual type of the implementation
        /// </summary>
        Type RegisteredType { get; }

        /// <summary>
        /// Gets an <see cref="ISet{T}"/> which contains all contracts that this type fulfills
        /// </summary>
        ISet<object> FulfilledContracts { get; }

        /// <summary>
        /// Gets an <see cref="ISet{T}"/> which contains all <see cref="IFactory"/>'s usable to create this registered type
        /// </summary>
        ISet<IFactory> Factories { get; }

        /// <summary>
        /// Gets a value indicating whether only a single instance of this registered type should be created
        /// </summary>
        bool Singleton { get; }

        /// <summary>
        ///  Gets the metadata for this registered type
        /// </summary>
        IReadOnlyDictionary<string, object> Metadata { get; }

        /// <summary>
        /// The priority of this registration.
        /// </summary>
        /// <remarks>This priority value can be overridden in a specific factory</remarks>
        int Priority { get; }
    }
}