using System;
using System.Collections.Generic;
using PantherDI.Registry.Registration;

namespace PantherDI.Resolved.Providers
{
    public class DelegateProvider : IProvider
    {
        private readonly Func<Dictionary<Dependency, object>, object> _delegate;

        public DelegateProvider(Func<Dictionary<Dependency, object>, object> @delegate, IReadOnlyDictionary<string, object> metadata)
        {
            _delegate = @delegate;
            Metadata = metadata;
        }

        public static DelegateProvider WrapProvider<T>(Func<Dictionary<Dependency, object>, object> @delegate, IProvider provider)
        {
            var p = new DelegateProvider(@delegate, provider.Metadata)
            {
                FulfilledContracts = new HashSet<object>(provider.FulfilledContracts),
                UnresolvedDependencies = provider.UnresolvedDependencies,
                ResultType = typeof(T),
                Singleton = provider.Singleton
            };

            p.FulfilledContracts.Remove(provider.ResultType);
            p.FulfilledContracts.Add(typeof(T));

            return p;
        }

        public HashSet<Dependency> UnresolvedDependencies { get; internal set; } = new HashSet<Dependency>();
        public Type ResultType { get; internal set; }
        public ISet<object> FulfilledContracts { get; internal set; } = new HashSet<object>();
        public object CreateInstance(Dictionary<Dependency, object> resolvedDependencies)
        {
            return _delegate(resolvedDependencies);
        }

        public bool Singleton { get; internal set; }
        public IReadOnlyDictionary<string, object> Metadata { get; }
    }
}