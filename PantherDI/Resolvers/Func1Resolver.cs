using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PantherDI.Extensions;
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
                foreach (var provider in dependencyResolver(dependency.ReplaceExpectedType<T>()).ToArray())
                {
                    var givenDependencies = provider.UnresolvedDependencies.Where(x => x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn).GetTypeInfo())).ToArray();

                    if (!givenDependencies.Any()) continue;

                    var p = DelegateProvider.WrapProvider<Func<TIn, T>>(objects => (Func<TIn, T>)(x =>
                           {
                               objects = objects.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                               foreach (var givenDependency in givenDependencies)
                               {
                                   objects[givenDependency] = x;
                               }

                               return (T)provider.CreateInstance(objects);
                           }), provider);

                    p.UnresolvedDependencies = new HashSet<IDependency>(provider.UnresolvedDependencies.Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn).GetTypeInfo())), new Dependency.EqualityComparer());

                    yield return p;
                }
            }
        }
    }
}