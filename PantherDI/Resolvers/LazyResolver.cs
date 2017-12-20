using System;
using System.Collections.Generic;
using PantherDI.Extensions;
using PantherDI.Registry.Registration.Dependency;
using PantherDI.Resolved.Providers;

namespace PantherDI.Resolvers
{
    public class LazyResolver : GenericResolver
    {
        public LazyResolver() : base(typeof(Lazy<>), typeof(InnerResolver<>))
        {
        }

        private class InnerResolver<T> : IResolver
        {
            public IEnumerable<IProvider> Resolve(Func<IDependency, IEnumerable<IProvider>> dependencyResolver, IDependency dependency)
            {
                foreach (var provider in dependencyResolver(dependency.ReplaceExpectedType<T>()))
                {
                    var p = new DelegateProvider(x => new Lazy<T>(() => (T)provider.CreateInstance(x)), provider.Metadata)
                    {
                        FulfilledContracts = new HashSet<object>(provider.FulfilledContracts),
                        UnresolvedDependencies = provider.UnresolvedDependencies,
                        ResultType = typeof(Lazy<T>),
                        Singleton = provider.Singleton
                    };

                    p.FulfilledContracts.Remove(typeof(T));
                    p.FulfilledContracts.Add(typeof(Lazy<T>));
                    
                    yield return p;
                }
            }
        }
    }
}