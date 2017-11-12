﻿using System;
using System.Collections.Generic;
using System.Linq;
using PantherDI.Comparers;
using PantherDI.Registry.Registration.Dependency;
using PantherDI.Resolved.Providers;

namespace PantherDI.Resolvers
{
    public class EnumerableResolver : GenericResolver
    {
        public EnumerableResolver() : base(typeof(IEnumerable<>), typeof(InnerResolver<>))
        {
        }

        private class InnerResolver<T> : IResolver
        {
            public IEnumerable<IProvider> Resolve(Func<IDependency, IEnumerable<IProvider>> dependencyResolver, IDependency dependency)
            {
                var result = new List<EnumerableProvider<T>>();

                foreach (var provider in dependencyResolver(new Dependency(typeof(T), dependency.RequiredContracts)))
                {
                    var enumerableProvider = result.FirstOrDefault(x => SetComparer<IDependency>.Instance.Equals(x.UnresolvedDependencies, provider.UnresolvedDependencies));
                    if (enumerableProvider == null)
                    {
                        result.Add(new EnumerableProvider<T>(provider));
                    }
                    else
                    {
                        enumerableProvider.Add(provider);
                    }
                }

                return result;
            }
        }

        private class EnumerableProvider<T> : IProvider
        {
            private readonly List<IProvider> _innerProviders = new List<IProvider>();

            public EnumerableProvider(IProvider provider)
            {
                _innerProviders.Add(provider);
                FulfilledContracts = new HashSet<object>(_innerProviders[0].FulfilledContracts);
                FulfilledContracts.Remove(_innerProviders[0].ResultType);
                FulfilledContracts.Add(ResultType);
            }

            public void Add(IProvider provider)
            {
                FulfilledContracts.IntersectWith(provider.FulfilledContracts);
                FulfilledContracts.Add(ResultType);
                _innerProviders.Add(provider);
            }

            public ISet<object> FulfilledContracts { get; }

            public HashSet<IDependency> UnresolvedDependencies => _innerProviders[0].UnresolvedDependencies;

            public Type ResultType => typeof(IEnumerable<>).MakeGenericType(_innerProviders[0].ResultType);

            public bool Singleton => _innerProviders.All(x => x.Singleton);

            public object CreateInstance(Dictionary<IDependency, object> resolvedDependencies)
            {
                IEnumerable<object> Creator()
                {
                    foreach (var provider in _innerProviders)
                    {
                        yield return provider.CreateInstance(resolvedDependencies);
                    }
                }

                return Creator().Cast<T>().ToArray();
            }

            
        }
    }
}