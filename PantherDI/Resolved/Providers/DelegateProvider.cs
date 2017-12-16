using System;
using System.Collections.Generic;
using PantherDI.Registry.Registration.Dependency;

namespace PantherDI.Resolved.Providers
{
    public class DelegateProvider : IProvider
    {
        private readonly Func<Dictionary<IDependency, object>, object> _delegate;

        public DelegateProvider(Func<Dictionary<IDependency, object>, object> @delegate)
        {
            _delegate = @delegate;
        }

        public HashSet<IDependency> UnresolvedDependencies { get; internal set; } = new HashSet<IDependency>(new Dependency.EqualityComparer());
        public Type ResultType { get; internal set; }
        public ISet<object> FulfilledContracts { get; internal set; } = new HashSet<object>();
        public object CreateInstance(Dictionary<IDependency, object> resolvedDependencies)
        {
            return _delegate(resolvedDependencies);
        }

        public bool Singleton { get; internal set; }
    }
}