using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                var innerDependency = new Dependency(typeof(T));
                foreach (var contract in dependency.RequiredContracts.Where(x => !Equals(x, typeof(Func<TIn, T>))))
                {
                    innerDependency.RequiredContracts.Add(contract);
                }

                var providers = dependencyResolver(innerDependency).ToArray();

                foreach (var provider in providers)
                {
                    var givenDependencies = provider.UnresolvedDependencies.Where(x => x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn).GetTypeInfo())).ToArray();

                    if (!givenDependencies.Any()) continue;

                    var p = new DelegateProvider(objects => (Func<TIn, T>)(x =>
                    {
                        objects = objects.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                        foreach (var givenDependency in givenDependencies)
                        {
                            objects[givenDependency] = x;
                        }

                        return (T)provider.CreateInstance(objects);
                    }), provider.Metadata)
                    {
                        FulfilledContracts = new HashSet<object>(provider.FulfilledContracts),
                        ResultType = typeof(Func<TIn, T>),
                        Singleton = provider.Singleton
                    };

                    foreach (var unresolvedDependency in provider.UnresolvedDependencies.Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn).GetTypeInfo())))
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