using System;
using System.Collections.Generic;
using PantherDI.Registry.Registration;
using PantherDI.Registry.Registration.Dependency;
using PantherDI.Resolved;
using PantherDI.Resolved.Providers;

namespace PantherDI.Resolvers
{
    /// <summary>
    /// A strategy on how to resolve a dependency from the given providers in the knowledge base
    /// </summary>
    /// <remarks>The resolver can assume that the <see cref="IKnowledgeBase"/> already contains all necessary types.</remarks>
    public interface IResolver
    {
        /// <summary>
        /// Resolves all providers for a given dependency from the knowledge base
        /// </summary>
        /// <param name="dependencyResolver">A function to resolve transitive dependencies</param>
        /// <param name="dependency">The dependency to resolve</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> containing all instances of <see cref="IProvider"/> that match the given constraints.
        /// Will return <code>null</code> if this resolver is unable to handle the dependency at all
        /// </returns>
        /// 
        IEnumerable<IProvider> Resolve(Func<IDependency, IEnumerable<IProvider>> dependencyResolver, IDependency dependency);
    }
}