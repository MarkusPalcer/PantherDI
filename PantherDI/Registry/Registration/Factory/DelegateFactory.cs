using System;
using System.Collections.Generic;
using System.Linq;

namespace PantherDI.Registry.Registration.Factory
{
    /// <summary>
    /// A factory that invokes a delegate when executed
    /// </summary>
    public partial class DelegateFactory : IFactory
    {
        private readonly Func<object[], object> _delegate;

        public DelegateFactory(Func<object[], object> @delegate, IEnumerable<object> contracts, IEnumerable<Dependency> dependencies)
        {
            FulfilledContracts = contracts;
            _delegate = @delegate;
            Dependencies = dependencies;
        }

        private DelegateFactory(Func<object[], object> @delegate, IEnumerable<object> contracts, IEnumerable<Type> types)
            : this(@delegate, contracts, types.Select(x => new Dependency(x))) { }

        public static DelegateFactory Create<T>(Func<T> @delegate, params object[] contracts)
        {
            return new DelegateFactory(_ => @delegate(), contracts, Enumerable.Empty<Dependency>());
        }

        #region Implementation of IFactory

        public object Execute(object[] resolvedDependencies)
        {
            return _delegate(resolvedDependencies);
        }

        public IEnumerable<Dependency> Dependencies { get; }

        public IEnumerable<object> FulfilledContracts { get; }

        public int? Priority { get; set;  }

        #endregion
    }
}