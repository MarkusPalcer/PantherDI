using System;
using System.Collections.Generic;
using System.Linq;
using PantherDI.Exceptions;
using PantherDI.Registry.Registration.Dependency;
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

            return (T)providers.Single().CreateInstance(new Dictionary<IDependency, object>());
        }

        private class ContainerResolver : IResolver
        {
            private readonly Container _cnt;

            public ContainerResolver(Container cnt)
            {
                _cnt = cnt;
            }

            #region Implementation of IResolver

            public IEnumerable<IProvider> Resolve(Func<IDependency, IEnumerable<IProvider>> dependencyResolver, IDependency dependency)
            {
                return _cnt.RootResolver.Resolve(dependencyResolver, dependency);
            }

            #endregion
        }

        public IResolver AsResolver()
        {
            return new ContainerResolver(this);
        }

        private IEnumerable<IProvider> ResolveInternal(IDependency dependency)
        {
            var result = _cache[dependency];
            if (result != null)
            {
                return result;
            }

            // The provider is not yet in the cache. Resolve it and then store it there, so it is cached
            result = WrapSingletonProviders(RootResolver.Resolve(ResolveInternal, dependency)).ToArray();
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
    }
}