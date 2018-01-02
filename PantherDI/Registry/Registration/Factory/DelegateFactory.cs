using System;
using System.Collections.Generic;
using System.Linq;
using PantherDI.Registry.Registration.Dependency;

namespace PantherDI.Registry.Registration.Factory
{
    public partial class DelegateFactory : IFactory
    {
        private readonly Func<object[], object> _delegate;

        public DelegateFactory(Func<object[], object> @delegate, IEnumerable<object> contracts, IEnumerable<IDependency> dependencies)
        {
            Contracts = contracts;
            _delegate = @delegate;
            Dependencies = dependencies;
        }

        private DelegateFactory(Func<object[], object> @delegate, IEnumerable<object> contracts, IEnumerable<Type> types)
            : this(@delegate, contracts, types.Select(x => new Dependency.Dependency(x))) { }

        public static DelegateFactory Create<T>(Func<T> @delegate, params object[] contracts)
        {
            return new DelegateFactory(_ => @delegate(), contracts, Enumerable.Empty<Dependency.Dependency>());
        }

        #region Implementation of IFactory

        public object Execute(object[] resolvedDependencies)
        {
            return _delegate(resolvedDependencies);
        }

        public IEnumerable<IDependency> Dependencies { get; }

        public IEnumerable<object> Contracts { get; }

        #endregion
    }
}