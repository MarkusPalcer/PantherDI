using System;
using System.Collections.Generic;
using PantherDI.Registry.Registration.Dependency;
using PantherDI.Registry.Registration.Factory;

namespace PantherDI.Tests.Helpers
{
    public class Factory : IFactory
    {
        private readonly Func<object[], object> _executor;
        private readonly HashSet<object> _contracts = new HashSet<object>();

        public object Execute(object[] resolvedDependencies)
        {
            return _executor(resolvedDependencies);
        }

        public Factory(Func<object[], object> executor, params IDependency[] dependencies)
        {
            _executor = executor;
            Dependencies = dependencies;
        }

        public Factory(params IDependency[] dependencies)
            : this(_ => null, dependencies)
        {
        }

        public IEnumerable<IDependency> Dependencies { get; }
        IEnumerable<object> IFactory.Contracts => _contracts;

        public HashSet<object> Contracts => _contracts;
    }
}