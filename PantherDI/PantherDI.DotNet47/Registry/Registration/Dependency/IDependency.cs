using System;
using System.Collections.Generic;
using PantherDI.Resolved;

namespace PantherDI.Registry.Registration.Dependency
{
    /// <summary>
    /// Describes a dependency, that is a parameter of a <see cref="IFactory"/> or <see cref="IProvider"/>
    /// </summary>
    public interface IDependency
    {
        /// <summary>
        /// Gets the type as which the dependency will be used.
        /// It needs to be assignable to this type in order to serve as dependency.
        /// </summary>
        Type ExpectedType { get; }
        
        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> containing all contracts that need to be fulifilled in order to satisfy this dependency
        /// </summary>
        IEnumerable<object> RequiredContracts { get; }
    }
}