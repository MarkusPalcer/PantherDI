using System;
using System.Collections.Generic;
using PantherDI.Registry.Registration;

namespace PantherDI.Tests.Helpers
{
    public class Factory : IFactory
    {
        private readonly Func<object[], object> _executor;

        public object Execute(object[] resolvedDependencies)
        {
            return _executor(resolvedDependencies);
        }

        public Factory(Func<object[], object> executor, params IDependency[] dependencies)
        {
            _executor = executor;
            Dependencies = dependencies;
        }

        public IEnumerable<IDependency> Dependencies { get; }
    }
}