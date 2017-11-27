using System;
using System.Collections.Generic;
using PantherDI.Registry.Registration.Dependency;
using PantherDI.Resolved.Providers;

namespace PantherDI.Resolvers
{
    public class Func0Resolver : GenericResolver
    {
        public Func0Resolver() : base(typeof(Func<>), typeof(InnerResolver<>))
        {
        }

        private class InnerResolver<T> : IResolver
        {
            public IEnumerable<IProvider> Resolve(Func<IDependency, IEnumerable<IProvider>> dependencyResolver, IDependency dependency)
            {
                var innerDependency = new Dependency(typeof(T), dependency.RequiredContracts);
                innerDependency.RequiredContracts.Remove(typeof(Func<T>));
                innerDependency.RequiredContracts.Add(typeof(T));

                foreach (var provider in dependencyResolver(innerDependency))
                {
                    var p = new DelegateProvider(objects => (Func<T>)(() => (T)provider.CreateInstance(objects)))
                    {
                        FulfilledContracts = new HashSet<object>(provider.FulfilledContracts),
                        UnresolvedDependencies = provider.UnresolvedDependencies,
                        ResultType = typeof(Func<T>),
                        Singleton = provider.Singleton
                    };

                    p.FulfilledContracts.Remove(typeof(T));
                    p.FulfilledContracts.Add(typeof(Func<T>));

                    yield return p;
                }
            }
        }
    }
}