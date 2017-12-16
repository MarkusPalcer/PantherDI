﻿using System;
using System.Collections.Generic;
using PantherDI.Registry.Registration.Dependency;

namespace PantherDI.Resolved.Providers
{
    public class SingletonProvider : IProvider
    {
        private readonly IProvider _originalProvider;
        private readonly Dictionary<Type, object> _cache;
        public HashSet<IDependency> UnresolvedDependencies => _originalProvider.UnresolvedDependencies;
        public Type ResultType => _originalProvider.ResultType;
        public ISet<object> FulfilledContracts => _originalProvider.FulfilledContracts;

        public object CreateInstance(Dictionary<IDependency, object> resolvedDependencies)
        {
            if (_cache.TryGetValue(ResultType, out var instance))
            {
                return instance;
            }

            instance = _originalProvider.CreateInstance(resolvedDependencies);
            _cache[ResultType] = instance;
            return instance;
        }

        // This provider does not need to be wrapped any further.
        public bool Singleton => false;

        public SingletonProvider(IProvider originalProvider, Dictionary<Type, object> cache)
        {
            _originalProvider = originalProvider;
            _cache = cache;
        }
    }
}