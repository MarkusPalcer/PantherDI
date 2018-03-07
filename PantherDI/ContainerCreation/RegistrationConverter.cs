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
using PantherDI.Resolvers.Aggregation;

namespace PantherDI.ContainerCreation
{
    public class RegisteredFactory
    {
        public IFactory Factory;
        public IRegistration Registration;

        public IEnumerable<object> FulfilledContracts => Factory.FulfilledContracts.Concat(Registration.FulfilledContracts);
    }

    /// <summary>
    /// Converts registrations to providers
    /// </summary>
    public class RegistrationConverter
    {
        internal readonly Dictionary<object, List<RegisteredFactory>> _unprocessed;
        private readonly IResolver _resolvers;
        private readonly List<object> _resolutionStack = new List<object>();

        internal KnowledgeBase KnowledgeBase { get; } = new KnowledgeBase();

        public RegistrationConverter(ICatalog catalog, IResolver resolvers)
        {
            _resolvers = resolvers;

            _unprocessed = CreateProcessQueue(catalog);
        }

        public void ProcessAll()
        {
            while (_unprocessed.Any())
            {
                ProcessContract(_unprocessed.First().Key, ResolveDependency);
            }
        }

        public void ProcessContract(object contract, Func<IDependency, IEnumerable<IProvider>> dependencyResolver)
        {
            if (_resolutionStack.Contains(contract))
            {
                throw new CircularDependencyException();
            }

            _resolutionStack.Add(contract);

            // Process registrations and add to knowledge base
            while (_unprocessed.TryGetValue(contract, out var registrations))
            {
                ProcessRegistration(registrations.First(), dependencyResolver);
            }

            _resolutionStack.Remove(contract);
        }

        private void ProcessRegistration(RegisteredFactory registeredFactory, Func<IDependency, IEnumerable<IProvider>> dependencyResolver)
        {

            // Remove this registration completely (and remove empty entries)
            foreach (var contract in registeredFactory.FulfilledContracts)
            {
                if (_unprocessed.TryGetValue(contract, out var entries))
                {
                    entries.Remove(registeredFactory);
                    if (!entries.Any())
                    {
                        _unprocessed.Remove(contract);
                    }
                }
            }

            foreach (var provider in ProcessFactory(registeredFactory, dependencyResolver))
            {
                KnowledgeBase.Add(provider);
            }
        }

        public static IEnumerable<IProvider> ProcessFactory(RegisteredFactory registeredFactory, 
            Func<IDependency, IEnumerable<IProvider>> resolveDependency)
        {
            var registration = registeredFactory.Registration;
            var factory = registeredFactory.Factory;

            // Cache all providers for a contract
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
            ProcessContract(dependency.RequiredContracts.First(), ResolveDependency);
            return _resolvers.Resolve(ResolveDependency, dependency);
        }

        private static Dictionary<object, List<RegisteredFactory>> CreateProcessQueue(ICatalog catalog)
        {
            var unprocessed = new Dictionary<object, List<RegisteredFactory>>();

            // Create copies of all registrations and add the registered type to the fulfilled contracts in the process
            var registrations = catalog.Registrations
                .Select(RegistrationExtensions.Clone);

            foreach (var registration in registrations)
            {
                foreach (var factory in registration.Factories)
                {
                    var entry = new RegisteredFactory
                    {
                        Factory = factory,
                        Registration = registration
                    };

                    foreach (var contract in entry.FulfilledContracts)
                    {
                        if (!unprocessed.ContainsKey(contract))
                        {
                            unprocessed.Add(contract, new List<RegisteredFactory>());
                        }

                        unprocessed[contract].Add(entry);
                    }
                }

            }

            return unprocessed;
        }

    }
}