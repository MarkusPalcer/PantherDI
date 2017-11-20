using System;
using System.Collections.Generic;
using System.Linq;
using PantherDI.Registry.Registration.Dependency;
using PantherDI.Resolved.Providers;

namespace PantherDI.Resolvers
{
    public class Func1Resolver : GenericResolver
    {
        public Func1Resolver() : base(typeof(Func<,>), typeof(InnerResolver<,>))
        {
        }

        private class InnerResolver<TIn, T> : IResolver
        {
            public IEnumerable<IProvider> Resolve(Func<IDependency, IEnumerable<IProvider>> dependencyResolver, IDependency dependency)
            {
                var innerDependency = new Dependency(typeof(T), dependency.RequiredContracts);
                innerDependency.RequiredContracts.Remove(typeof(Func<T>));
                innerDependency.RequiredContracts.Add(typeof(T));

                foreach (var provider in dependencyResolver(innerDependency))
                {
                    var givenDependencies = provider.UnresolvedDependencies.Where(x => x.ExpectedType.IsAssignableFrom(typeof(TIn))).ToArray();

                    if (!givenDependencies.Any()) continue;

                    var p = new DelegateProvider(objects => (Func<TIn, T>)(x =>
                    {
                        objects = objects.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                        foreach (var givenDependency in givenDependencies)
                        {
                            objects[givenDependency] = x;
                        }

                        return (T)provider.CreateInstance(objects);
                    }))
                    {
                        FulfilledContracts = new HashSet<object>(provider.FulfilledContracts),
                        ResultType = typeof(Func<TIn, T>),
                        Singleton = provider.Singleton
                    };

                    foreach (var unresolvedDependency in provider.UnresolvedDependencies.Where(x => !x.ExpectedType.IsAssignableFrom(typeof(TIn))))
                    {
                        p.UnresolvedDependencies.Add(unresolvedDependency);
                    }

                    p.FulfilledContracts.Remove(typeof(T));
                    p.FulfilledContracts.Add(typeof(Func<TIn, T>));

                    yield return p;
                }
            }
        }
    }
}