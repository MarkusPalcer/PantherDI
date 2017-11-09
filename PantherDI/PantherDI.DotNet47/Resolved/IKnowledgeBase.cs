using System.Collections.Generic;
using PantherDI.Resolved.Providers;
using PantherDI.Resolvers;

namespace PantherDI.Resolved
{
    /// <summary>
    /// Contains all the <see cref="IProvider"/>'s a container knows, accessible by contract
    /// </summary>
    public interface IKnowledgeBase: IResolver
    {
        /// <summary>
        /// Gets all known providers that fulfill a given contract
        /// </summary>
        /// <param name="contract">The contract to search for</param>
        /// <returns>
        /// The list of providers that fulfill the given contract or an empty list if there are none.
        /// <code>null</code> is never returned.
        /// </returns>
        IEnumerable<IProvider> this[object contract] { get; }

        /// <summary>
        /// Adds a provider to the data structure
        /// </summary>
        /// <param name="provider">The provider to add</param>
        void Add(IProvider provider);
    }
}