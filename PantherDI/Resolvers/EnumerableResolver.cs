﻿using System;
using System.Collections.Generic;
using System.Linq;
using PantherDI.Comparers;
using PantherDI.Extensions;
using PantherDI.Registry.Registration;
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
            public IEnumerable<IProvider> Resolve(Func<Dependency, IEnumerable<IProvider>> dependencyResolver, Dependency dependency)
            {
                var result = new List<EnumerableProvider<T>>();

                foreach (var provider in dependencyResolver(dependency.ReplaceExpectedType<T>()))
                {
                    var enumerableProvider = result.FirstOrDefault(x => SetComparer<Dependency>.Instance.Equals(x.UnresolvedDependencies, provider.UnresolvedDependencies) && x.Priority.Equals(provider.Priority));

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
                Metadata = provider.Metadata.ToDictionary(x => x.Key, x => x.Value);
                Priority = provider.Priority;
            }

            public void Add(IProvider provider)
            {
                FulfilledContracts.IntersectWith(provider.FulfilledContracts);
                FulfilledContracts.Add(ResultType);
                _innerProviders.Add(provider);

                // Just keep metadata that are contained and equal in all added providers
                var entriesToRemove = Metadata.Where(entry =>
                                              {
                                                  if (!provider.Metadata.TryGetValue(entry.Key, out var value)) return true;
                                                  if (!Equals(value, entry.Value)) return true;
                                                  return false;
                                              })
                                              .Select(x => x.Key)
                                              .ToArray();

                foreach (var entryToRemove in entriesToRemove)
                {
                    Metadata.Remove(entryToRemove);
                }
            }

            public ISet<object> FulfilledContracts { get; }

            public HashSet<Dependency> UnresolvedDependencies => _innerProviders[0].UnresolvedDependencies;

            public Type ResultType => typeof(IEnumerable<>).MakeGenericType(_innerProviders[0].ResultType);

            public bool Singleton => _innerProviders.All(x => x.Singleton);
            IReadOnlyDictionary<string, object> IProvider.Metadata => Metadata;

            public int Priority { get; }

            private Dictionary<string, object> Metadata { get; }

            public object CreateInstance(Dictionary<Dependency, object> resolvedDependencies)
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