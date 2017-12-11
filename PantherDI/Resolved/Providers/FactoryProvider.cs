using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PantherDI.Exceptions;
using PantherDI.Registry.Registration;
using PantherDI.Registry.Registration.Dependency;
using PantherDI.Registry.Registration.Registration;

namespace PantherDI.Resolved.Providers
{

    /// <summary>
    /// A provider that is backed by a factory.
    /// </summary>
    public class FactoryProvider : IProvider
    {
        private readonly IFactory _factory;
        private readonly Dictionary<IDependency, IProvider> _dependencyProviders;

        public HashSet<IDependency> UnresolvedDependencies { get; }

        public Type ResultType { get; }

        public ISet<object> FulfilledContracts { get; }


        public FactoryProvider(IRegistration providedRegistration, IFactory factory, Dictionary<IDependency, IProvider> dependencyProviders) 
        {
            _factory = factory;
            _dependencyProviders = new Dictionary<IDependency, IProvider>(dependencyProviders, new Dependency.EqualityComparer());

            UnresolvedDependencies = new HashSet<IDependency>(new Dependency.EqualityComparer());
            
            foreach (var dependency in _factory.Dependencies)
            {
                if (!_dependencyProviders.Any(x => new Dependency.EqualityComparer().Equals(dependency, x.Key)))
                {
                    UnresolvedDependencies.Add(dependency);
                }
            }

            ResultType = providedRegistration.RegisteredType;

            FulfilledContracts =
                new HashSet<object>(providedRegistration.FulfilledContracts) {providedRegistration.RegisteredType};

            Singleton = providedRegistration.Singleton;
        }

        public object CreateInstance(Dictionary<IDependency, object> resolvedDependencies)
        {
            resolvedDependencies = new Dictionary<IDependency, object>(resolvedDependencies, new Dependency.EqualityComparer());

            var resolvedParameters = new List<object>();
            foreach (var dependency in _factory.Dependencies)
            {
                var resolvedParameter = resolvedDependencies
                    .Where(x => new Dependency.EqualityComparer().Equals(dependency, x.Key))
                    .Select(x => x.Value)
                    .FirstOrDefault();
                if (resolvedParameter == null)
                {
                    resolvedParameter = _dependencyProviders
                        .Where(x => new Dependency.EqualityComparer().Equals(dependency, x.Key))
                        .Select(x => x.Value.CreateInstance(resolvedDependencies))
                        .FirstOrDefault();
                }

                if (resolvedParameter == null)
                {
                    throw new DependencyException("Unresolved Parameter. This is an internal error.");
                }

                resolvedParameters.Add(resolvedParameter);
            }

            return _factory.Execute(resolvedParameters.ToArray());
        }

        public bool Singleton { get; }
    }
}