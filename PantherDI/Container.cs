using System;
using System.Collections.Generic;
using System.Linq;
using PantherDI.ContainerCreation;
using PantherDI.Exceptions;
using PantherDI.Extensions;
using PantherDI.Registry.Catalog;
using PantherDI.Registry.Registration;
using PantherDI.Registry.Registration.Factory;
using PantherDI.Registry.Registration.Registration;
using PantherDI.Resolved;
using PantherDI.Resolved.Providers;
using PantherDI.Resolvers;

namespace PantherDI
{
    public class Container : IContainer
    {
        private readonly  ProviderCache _cache = new ProviderCache();
        internal readonly IResolver RootResolver;
        private readonly Dictionary<Type, object> _singletons = new Dictionary<Type, object>();

        public Container(IResolver rootResolver)
        {
            RootResolver = rootResolver ?? throw new ArgumentNullException(nameof(rootResolver));
        }

        public MultiGenerationConfiguration MultiGenerationConfiguration { get; set; }

        public T Resolve<T>(params object[] contracts)
        {
            var providers = ResolveInternal(new Dependency(typeof(T),contracts))
                .Where(p => !p.UnresolvedDependencies.Any())
                .ToArray();

            if (providers.Length == 0)
            {
                throw new NoSuitableRegistrationException();
            }
            if (providers.Length > 1)
            {
                throw new TooManySuitableRegistrationsException();
            }

            return (T)providers.Single().CreateInstance(new Dictionary<Dependency, object>());
        }
        

        public IResolver AsResolver()
        {
            return new ContainerResolver(this);
        }

        public ICatalog AsCatalog()
        {
            return new ContainerCatalog(this);
        }

        private IEnumerable<IProvider> ResolveInternal(Dependency dependency)
        {
            return ResolveInternal(dependency, ResolveInternal);
        }

        internal IEnumerable<IProvider> ResolveInternal(Dependency dependency, Func<Dependency, IEnumerable<IProvider>> dependencyResolver)
        {
            var result = _cache[dependency];
            if (result != null)
            {
                return result;
            }

            // The provider is not yet in the cache. Resolve it and then store it there, so it is cached
            result = WrapSingletonProviders(RootResolver.Resolve(dependencyResolver, dependency)).ToArray();
            _cache[dependency] = result;
            return result;
        }

        private IEnumerable<IProvider> WrapSingletonProviders(IEnumerable<IProvider> source)
        {
            foreach (var provider in source)
            {
                if (provider.Singleton)
                {
                    yield return new SingletonProvider(provider, _singletons);
                }
                else
                {
                    yield return provider;
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///   <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var disposable in _singletons.Values.OfType<IDisposable>())
                {
                    disposable.Dispose();
                }
                _singletons.Clear();
            }
        }

        private class ContainerResolver : IResolver
        {
            private readonly Container _cnt;

            public ContainerResolver(Container cnt)
            {
                _cnt = cnt;
            }

            #region Implementation of IResolver

            public IEnumerable<IProvider> Resolve(Func<Dependency, IEnumerable<IProvider>> dependencyResolver, Dependency dependency)
            {
                return _cnt.ResolveInternal(dependency, dependencyResolver).SelectMany(resolver => RegistrationConverter.ProcessProvider(resolver, dependencyResolver));
            }

            #endregion
        }

        private class ContainerCatalog : ICatalog
        {
            private readonly Dictionary<Type, ManualRegistration> _registrations = new Dictionary<Type, ManualRegistration>();

            public ContainerCatalog(Container container)
            {
                ProcessResolver(container.RootResolver);
            }

            private void ProcessResolver(IResolver resolver)
            {
                switch (resolver)
                {
                    case KnowledgeBase knowledgeBase:
                    {
                        ProcessKnowledgeBase(knowledgeBase);
                        break;
                    }
                    case IEnumerable<IResolver> resolvers:
                    {
                        resolvers.ForEach(ProcessResolver);
                        break;
                    }
                    case RegistrationProcessingResolver registrationProcessingResolver:
                    {
                        ProcessConverter(registrationProcessingResolver.Converter);
                        break;
                    }
                }
            }

            private void ProcessKnowledgeBase(KnowledgeBase knowledgeBase)
            {
                var providers = knowledgeBase.KnownProviders.ToDictionary(x => x.Key, x=> x.Value);

                while (providers.Any())
                {
                    var item = providers[providers.Keys.First()].First();

                    item.FulfilledContracts.ForEach(x =>
                    {
                        var itemsForContract = providers[x];
                        itemsForContract.Remove(item);
                        if (!itemsForContract.Any()) providers.Remove(x);
                    });


                    if (!_registrations.TryGetValue(item.ResultType, out var registration))
                    {
                        registration = new ManualRegistration(null, null, item.Metadata)
                        {
                            RegisteredType = item.ResultType,
                            Singleton = item.Singleton
                        };

                        _registrations[item.ResultType] = registration;
                    }

                    registration.Factories.Add(new ProviderFactory(item));
                }
            }

            private void ProcessConverter(RegistrationConverter converter)
            {
                ProcessUnregistered(converter._unprocessed);
                ProcessKnowledgeBase(converter.KnowledgeBase);
            }

            private void ProcessUnregistered(Dictionary<object, List<RegisteredFactory>> unregistered)
            {
                unregistered.SelectMany(x => x.Value).Select(x => x.Registration).ForEach(x =>
                {
                    if (_registrations.ContainsKey(x.RegisteredType)) return;
                    _registrations[x.RegisteredType] = x.Clone();
                });
            }

            private class ProviderFactory : IFactory
            {
                private readonly IProvider _provider;

                public ProviderFactory(IProvider provider)
                {
                    _provider = provider;
                    FulfilledContracts = provider.FulfilledContracts;
                    Dependencies = provider.UnresolvedDependencies.ToArray();

                }

                #region Implementation of IFactory

                public object Execute(object[] resolvedDependencies)
                {
                    return _provider.CreateInstance(Dependencies.Zip(resolvedDependencies, Tuple.Create).ToDictionary(x => x.Item1, x => x.Item2));
                }

                public IEnumerable<Dependency> Dependencies { get; }
                public IEnumerable<object> FulfilledContracts { get; }
                public int? Priority => _provider.Priority;

                #endregion
            }

            #region Implementation of ICatalog

            public IEnumerable<IRegistration> Registrations => _registrations.Values;

            #endregion
        }
    }
}