using System;
using System.Collections.Generic;
using System.Linq;
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
                var innerDependency = new Dependency(typeof(T));
                foreach (var contract in dependency.RequiredContracts.Where(x => !Equals(x, typeof(Func<T>))))
                {
                    innerDependency.RequiredContracts.Add(contract);
                }

                var providers = dependencyResolver(innerDependency).ToArray();

                foreach (var provider in providers)
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