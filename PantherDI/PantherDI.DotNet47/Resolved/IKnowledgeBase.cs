using System.Collections.Generic;

namespace PantherDI.Resolved
{
    /// <summary>
    /// Contains all the <see cref="IProvider"/>'s a container knows, accessible by contract
    /// </summary>
    public interface IKnowledgeBase 
    {
        /// <summary>
        /// Gets all known providers that fulfill a given contract
        /// </summary>
        /// <param name="contract">The contract to search for</param>
        IEnumerable<IProvider> this[object contract] { get; }
    }
}