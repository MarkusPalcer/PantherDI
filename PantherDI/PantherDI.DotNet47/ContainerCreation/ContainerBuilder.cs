using System.Collections.Generic;
using System.Linq;
using PantherDI.Exceptions;
using PantherDI.Registry.Catalog;
using PantherDI.Registry.Registration;
using PantherDI.Registry.Registration.Dependency;
using PantherDI.Registry.Registration.Registration;
using PantherDI.Resolved;
using PantherDI.Resolvers;

namespace PantherDI.ContainerCreation
{
    public class ContainerBuilder
    {
        private readonly ICatalog _catalog;
        private Dictionary<object, List<IRegistration>> _unprocessed;
        private readonly MergedResolver _resolvers = new MergedResolver();
        private readonly List<object> _resolutionStack = new List<object>();
        internal KnowledgeBase KnowledgeBase { get; } = new KnowledgeBase();

        public ContainerBuilder(ICatalog catalog)
        {
            _catalog = catalog;
            _resolvers.Add(KnowledgeBase);
        }

        public IContainer Build()
        {
            _unprocessed = CreateProcessQueue(_catalog);
            CreateKnowledgeBase();
            return new Container(KnowledgeBase, _resolvers.Except(new IResolver[]{KnowledgeBase}));
        }

        private void Process(object contract)
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
                ProcessFactory(registration, factory);
            }
        }

        private void ProcessFactory(IRegistration registration, IFactory factory)
        {
            // Cache all providers for a contryt
            var allProviders = factory.Dependencies.ToDictionary(x => x, x => ResolveDependency(x).ToArray());

            // Create all combinations possible
            var allCombinations = new List<Dictionary<IDependency, IProvider>> {new Dictionary<IDependency, IProvider>()};
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
            foreach (var provider in allCombinations.Select(
                combination => new FactoryProvider(registration, factory, combination)))
            {
                KnowledgeBase.Add(provider);
            }
        }

        private IEnumerable<IProvider> ResolveDependency(IDependency dependency)
        {
            Process(dependency.RequiredContracts.First());
            return _resolvers.Resolve(ResolveDependency, dependency);
        }

        private void CreateKnowledgeBase()
        {
            while (_unprocessed.Any())
            {
                Process(_unprocessed.First().Key);
            }
        }

        private static Dictionary<object, List<IRegistration>> CreateProcessQueue(ICatalog catalog)
        {
            var unprocessed = new Dictionary<object, List<IRegistration>>();

            // Create copies of all registrations and add the registered type to the fulfilled contracts in the process
            var registrations = catalog.Registrations
                .Select(x => new ManualRegistration(x.RegisteredType, new HashSet<object>(x.FulfilledContracts){x.RegisteredType}, new HashSet<IFactory>(x.Factories)));

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