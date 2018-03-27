using System.Collections.Generic;
using System.Linq;
using PantherDI.Registry.Registration;
using PantherDI.Resolved.Providers;

namespace PantherDI.Extensions
{
    public static class ProviderExtensions
    {
        public static IProvider AddDependencies(this IProvider provider, Dictionary<Dependency, IProvider> dependencyProviders)
        {
            var originalUnresolvedDependencies = provider.UnresolvedDependencies;
            var unresolvedDependencies = new HashSet<Dependency>(originalUnresolvedDependencies);
            unresolvedDependencies.ExceptWith(dependencyProviders.Keys);

            object ProviderImplementation(Dictionary<Dependency, object> resolvedDependencies)
            {
                resolvedDependencies = resolvedDependencies.ToDictionary(x => x.Key, x => x.Value);
                foreach (var dependency in originalUnresolvedDependencies)
                {
                    resolvedDependencies[dependency] = dependencyProviders[dependency].CreateInstance(resolvedDependencies);
                }

                return provider.CreateInstance(resolvedDependencies);
            };

            var p = new DelegateProvider(ProviderImplementation, provider.Metadata)
            {
                FulfilledContracts = new HashSet<object>(provider.FulfilledContracts),
                UnresolvedDependencies = unresolvedDependencies,
                ResultType = provider.ResultType,
                Singleton = provider.Singleton
            };

            return p;
        }
    }
}