using System;
using System.Collections.Generic;
using System.Linq;
using PantherDI.Registry.Registration.Dependency;
using PantherDI.Resolved.Providers;

namespace PantherDI.Resolvers.Aggregation
{
    /// <summary>
    /// Resolves the given dependency by merging all resolver results together
    /// </summary>
    public class AllMatchesResolver : List<IResolver>, IResolver
    {
        public AllMatchesResolver()
        {
        }

        public AllMatchesResolver(IEnumerable<IResolver> collection) : base(collection)
        {
        }

        #region Implementation of IResolver

        public IEnumerable<IProvider> Resolve(Func<IDependency, IEnumerable<IProvider>> dependencyResolver, IDependency dependency)
        {
            return this.SelectMany(x => x.Resolve(dependencyResolver, dependency) ?? Enumerable.Empty<IProvider>());
        }

        #endregion
    }
}