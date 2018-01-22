﻿using System.Collections.Generic;
using System.Linq;
using PantherDI.Registry.Registration.Dependency;

namespace PantherDI.Registry.Registration.Factory
{
    /// <inheritdoc />
    /// <summary>
    /// A factory that always returns the given instance
    /// </summary>
    public class InstanceFactory<T> : IFactory
    {
        private readonly T _instance;

        public InstanceFactory(T instance, object[] contracts)
        {
            _instance = instance;
            FulfilledContracts = new HashSet<object>(contracts);
        }

        #region Implementation of IFactory

        public object Execute(object[] resolvedDependencies)
        {
            return _instance;
        }

        public IEnumerable<IDependency> Dependencies => Enumerable.Empty<IDependency>();

        public IEnumerable<object> FulfilledContracts { get; }

        #endregion
    }
}