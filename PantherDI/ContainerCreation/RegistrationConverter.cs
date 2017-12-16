using System;
using System.Collections.Generic;
using System.Linq;
using PantherDI.Exceptions;
using PantherDI.Extensions;
using PantherDI.Registry.Catalog;
using PantherDI.Registry.Registration.Dependency;
using PantherDI.Registry.Registration.Factory;
using PantherDI.Registry.Registration.Registration;
using PantherDI.Resolved;
using PantherDI.Resolved.Providers;
using PantherDI.Resolvers;

namespace PantherDI.ContainerCreation
{
    /// <summary>
    /// Converts registrations to providers
    /// </summary>
    public class RegistrationConverter
    {
        private readonly Dictionary<object, List<IRegistration>> _unprocessed;
        private readonly MergedResolver _resolvers = new MergedResolver();
        private readonly List<object> _resolutionStack = new List<object>();

        internal KnowledgeBase KnowledgeBase { get; } = new KnowledgeBase();

        public RegistrationConverter(ICatalog catalog, params IResolver[] resolvers)
        {
            _resolvers.Add(KnowledgeBase);
            foreach (var resolver in resolvers)
            {
                _resolvers.Add(resolver);
            }

            _unprocessed = CreateProcessQueue(catalog);
        }

        public void ProcessAll()
        {
            while (_unprocessed.Any())
            {
                ProcessContract(_unprocessed.First().Key);
            }
        }

        private void ProcessContract(object contract)
        {
            if (_resolutionStack.Contains(contract))
            {
                throw new CircularDependencyException();
            }

            _resolutionStack.Add(contract);

            // Process registrations and add to knowledge base
            while (_unprocessed.TryGetValue(contract, out var registrations))
            {
                ProcessRegistration(registrations.First());
            }

            _resolutionStack.Remove(contract);
        }

        private void ProcessRegistration(IRegistration registration)
        {
            // Remove this registration completely (and remove empty entries)
            foreach (var contract in registration.FulfilledContracts)
            {
                if (_unprocessed.TryGetValue(contract, out var entries))
                {
                    entries.Remove(registration);
                    if (!entries.Any())
                    {
                        _unprocessed.Remove(contract);
                    }
                }
            }

            foreach (var factory in registration.Factories)
            {
                foreach (var provider in ProcessFactory(registration, factory, ResolveDependency))
                {
                    KnowledgeBase.Add(provider);
                }
            }
        }

        public static IEnumerable<IProvider> ProcessFactory(IRegistration registration, IFactory factory,
            Func<IDependency, IEnumerable<IProvider>> resolveDependency)
        {
            // Cache all providers for a contryt
            var allProviders = factory.Dependencies
                .Where(x => !x.Ignored)
                .ToDictionary(x => x, x => resolveDependency(x).ToArray())
                .Where(x => x.Value.Any());

            // Create all combinations possible
            var allCombinations = new List<Dictionary<IDependency, IProvider>> { new Dictionary<IDependency, IProvider>() };
            foreach (var pair in allProviders)
            {
                var newCombinations = new List<Dictionary<IDependency, IProvider>>();

                foreach (var oldCombination in allCombinations)
                {
                    foreach (var provider in pair.Value)
                    {
                        var newCombination = oldCombination.ToDictionary(x => x.Key, x => x.Value);
                        newCombination.Add(pair.Key, provider);

                        newCombinations.Add(newCombination);
                    }
                }

                allCombinations = newCombinations;
            }

            // Create a provider for each combination
            return allCombinations.Select(combination => new FactoryProvider(registration, factory, combination)).ToArray();
        }

        private IEnumerable<IProvider> ResolveDependency(IDependency dependency)
        {
            ProcessContract(dependency.RequiredContracts.First());
            return _resolvers.Resolve(ResolveDependency, dependency);
        }

        private static Dictionary<object, List<IRegistration>> CreateProcessQueue(ICatalog catalog)
        {
            var unprocessed = new Dictionary<object, List<IRegistration>>();

            // Create copies of all registrations and add the registered type to the fulfilled contracts in the process
            var registrations = catalog.Registrations
                .Select(RegistrationExtensions.Clone);

            foreach (var registration in registrations)
            {
                foreach (var contract in registration.FulfilledContracts)
                {
                    if (!unprocessed.ContainsKey(contract))
                    {
                        unprocessed.Add(contract, new List<IRegistration>());
                    }

                    unprocessed[contract].Add(registration);
                }
            }

            return unprocessed;
        }

    }
}