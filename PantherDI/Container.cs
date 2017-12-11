using System;
using System.Collections.Generic;
using System.Linq;
using PantherDI.Exceptions;
using PantherDI.Registry.Registration.Dependency;
using PantherDI.Resolved;
using PantherDI.Resolved.Providers;
using PantherDI.Resolvers;

namespace PantherDI
{
    public class Container : IContainer
    {
        internal readonly IKnowledgeBase _knowledgeBase;
        private readonly MergedResolver _rootResolver;
        private readonly Dictionary<Type, object> _singletons = new Dictionary<Type, object>();

        public Container(IKnowledgeBase knowledgeBase, IEnumerable<IResolver> resolvers)
        {
            if (resolvers == null) throw new ArgumentNullException(nameof(resolvers));
            _knowledgeBase = knowledgeBase ?? throw new ArgumentNullException(nameof(knowledgeBase));
            _rootResolver = new MergedResolver(resolvers);
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

        private IEnumerable<IProvider> ResolveInternal(IDependency dependency)
        {
            var result = _knowledgeBase.Resolve(ResolveInternal, dependency).ToArray();
            if (result.Any())
            {
                return WrapSingletonProviders(result);
            }

            // The provider is not yet in the knowledge base. Resolve it and then store it there, so it is cached
            result = _rootResolver.Resolve(ResolveInternal, dependency).ToArray();
            foreach (var provider in result)
            {
                _knowledgeBase.Add(provider);
            }

            return WrapSingletonProviders(result);
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