using System.Collections.Generic;
using FluentAssertions.Execution;
using PantherDI.Registry.Registration.Dependency;
using PantherDI.Registry.Registration.Registration;
using PantherDI.Resolved.Providers;

namespace PantherDI.Tests.Helpers
{
    public class DependencyResolverDictionary : Dictionary<IDependency, IEnumerable<IProvider>>
    {
        private Dictionary<IDependency, int> Calls { get; } = new Dictionary<IDependency, int>(new Dependency.EqualityComparer());

        public DependencyResolverDictionary() : base(new Dependency.EqualityComparer())
        {
        }
        
        public IEnumerable<IProvider> Execute(IDependency key)
        {
            Calls[key] = CallsFor(key) + 1;

            return TryGetValue(key, out var value)
                       ? value
                       : throw new AssertionFailedException($"dependencyResolver called with unexpected dependency: {key}");
        }

        public int CallsFor(IDependency key)
        {
            return Calls.TryGetValue(key, out var value) ? value : 0;
        }
    }
}