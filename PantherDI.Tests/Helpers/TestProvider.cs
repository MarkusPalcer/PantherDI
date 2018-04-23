using System;
using System.Collections.Generic;
using PantherDI.Registry.Registration;
using PantherDI.Resolved.Providers;

namespace PantherDI.Tests.Helpers
{
    public class TestProvider : IProvider

    {
        private readonly Func<Dictionary<Dependency, object>, object> _delegate;

        public TestProvider(object value) : this(_ => value)
        {
        }

        public TestProvider(Func<Dictionary<Dependency, object>, object> @delegate)
        {
            _delegate = @delegate;
        }

        public int InvocationCounter { get; set; }

        public HashSet<Dependency> UnresolvedDependencies { get; } = new HashSet<Dependency>();

        public Type ResultType { get; set; }

        public ISet<object> FulfilledContracts { get; } = new HashSet<object>();

        public object CreateInstance(Dictionary<Dependency, object> resolvedDependencies)
        {
            InvocationCounter++;
            return _delegate(resolvedDependencies);
        }

        public bool Singleton { get; set; }

        public Dictionary<string, object> Metadata { get; } = new Dictionary<string, object>();
        public int Priority { get; set; } = 0;

        IReadOnlyDictionary<string, object> IProvider.Metadata => Metadata;
    }
}