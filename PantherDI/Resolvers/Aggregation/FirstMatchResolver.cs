using System;
using System.Collections.Generic;
using System.Linq;
using PantherDI.Registry.Registration;
using PantherDI.Resolved.Providers;

namespace PantherDI.Resolvers.Aggregation
{
    /// <summary>
    /// Resolves the given dependency by selecting the first resolver that produces an actual result
    /// </summary>
    public class FirstMatchResolver : List<IResolver>, IResolver
    {
        public FirstMatchResolver()
        {
        }

        public FirstMatchResolver(IEnumerable<IResolver> collection) : base(collection)
        {
        }

        #region Implementation of IResolver

        public IEnumerable<IProvider> Resolve(Func<Dependency, IEnumerable<IProvider>> dependencyResolver, Dependency dependency)
        {
            return this
                       .Select(r => r.Resolve(dependencyResolver, dependency))
                       .FirstOrDefault(r => r != null && r.Any()) ?? Enumerable.Empty<IProvider>();
        }

        #endregion
    }
}