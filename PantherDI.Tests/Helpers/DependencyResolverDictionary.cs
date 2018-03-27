using System.Collections.Generic;
using FluentAssertions.Execution;
using PantherDI.Registry.Registration;
using PantherDI.Registry.Registration.Registration;
using PantherDI.Resolved.Providers;

namespace PantherDI.Tests.Helpers
{
    public class DependencyResolverDictionary : Dictionary<Dependency, IEnumerable<IProvider>>
    {
        private Dictionary<Dependency, int> Calls { get; } = new Dictionary<Dependency, int>();

        public IEnumerable<IProvider> Execute(Dependency key)
        {
            Calls[key] = CallsFor(key) + 1;

            return TryGetValue(key, out var value)
                       ? value
                       : throw new AssertionFailedException($"dependencyResolver called with unexpected dependency: {key}");
        }

        public int CallsFor(Dependency key)
        {
            return Calls.TryGetValue(key, out var value) ? value : 0;
        }
    }
}