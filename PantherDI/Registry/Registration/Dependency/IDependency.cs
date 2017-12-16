﻿using System;
using System.Collections.Generic;
using PantherDI.Resolved;
using PantherDI.Resolved.Providers;

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
        /// Gets an <see cref="ISet{T}"/> containing all contracts that need to be fulifilled in order to satisfy this dependency
        /// </summary>
        ISet<object> RequiredContracts { get; }

        /// <summary>
        /// Gets a value indicating whether this dependency should remain unresolved.
        /// </summary>
        bool Ignored { get; }
    }
}