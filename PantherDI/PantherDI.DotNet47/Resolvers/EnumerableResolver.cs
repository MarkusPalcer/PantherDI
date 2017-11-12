using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PantherDI.Comparers;
using PantherDI.Registry.Registration.Dependency;
using PantherDI.Resolved.Providers;

namespace PantherDI.Resolvers
{
    public class EnumerableResolver : IResolver
    {
        public IEnumerable<IProvider> Resolve(Func<IDependency, IEnumerable<IProvider>> dependencyResolver,
            IDependency dependency)
        {
            // Only handle IEnumerable<T>
            if (dependency.ExpectedType.GetTypeInfo().IsGenericType &&
                dependency.ExpectedType.GetTypeInfo().GetGenericTypeDefinition() != typeof(IEnumerable<>))
            {
                return Enumerable.Empty<IProvider>();
            }

            var innerType = dependency.ExpectedType.GenericTypeArguments[0];
            var innerDependency = new Dependency(innerType, dependency.RequiredContracts);

            var providerType = typeof(EnumerableProvider<>).MakeGenericType(innerType);
            var constructor = providerType.GetConstructors()[0];

            var result = new List<EnumerableProvider>();

            foreach (var provider in dependencyResolver(innerDependency))
            {
                var enumerableProvider = result.FirstOrDefault(x =>SetComparer<IDependency>.Instance.Equals(x.UnresolvedDependencies, provider.UnresolvedDependencies));
                if (enumerableProvider == null)
                {
                    result.Add((EnumerableProvider) constructor.Invoke(new object[] {provider}));
                }
                else
                {
                    enumerableProvider.Add(provider);
                }
            }

            return result;
        }

        private abstract class EnumerableProvider : IProvider
        {
            protected readonly List<IProvider> _innerProviders = new List<IProvider>();

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

            public abstract object CreateInstance(Dictionary<IDependency, object> resolvedDependencies);
        }

        private class EnumerableProvider<T> : EnumerableProvider
        {
            public EnumerableProvider(IProvider provider) : base(provider)
            {
            }

            public override object CreateInstance(Dictionary<IDependency, object> resolvedDependencies)
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