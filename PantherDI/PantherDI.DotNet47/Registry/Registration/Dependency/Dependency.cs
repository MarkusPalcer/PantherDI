using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PantherDI.Registry.Registration.Dependency
{
    [DebuggerDisplay("Dependency<{ExpectedType}>({RequiredContracts})")]
    public class Dependency : IDependency
    {
        public Dependency(Type expectedType, params object[] requiredContracts)
        {
            if (expectedType == null) throw new ArgumentNullException(nameof(expectedType));

            if (!requiredContracts.Any())
            {
                requiredContracts = new object[] {expectedType};
            }

            ExpectedType = expectedType;
            RequiredContracts = requiredContracts;
        }

        public Type ExpectedType { get; }

        public IEnumerable<object> RequiredContracts { get; }

        public sealed class EqualityComparer : IEqualityComparer<IDependency>
        {
            public bool Equals(IDependency x, IDependency y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                if (!(x.ExpectedType == y.ExpectedType)) return false;
                if (x.RequiredContracts.Count() != y.RequiredContracts.Count()) return false;
                foreach (var pairs in x.RequiredContracts.Zip(y.RequiredContracts, Tuple.Create))
                {
                    if (!pairs.Item1.Equals(pairs.Item2))
                    {
                        return false;
                    }
                }

                return true;
            }

            public int GetHashCode(IDependency obj)
            {
                unchecked
                {
                    return obj.RequiredContracts.Aggregate(obj.ExpectedType.GetHashCode(), (current, contract) => (current * 397) ^ contract.GetHashCode());
                }
            }

            public static IEqualityComparer<IDependency> Instance { get; } = new EqualityComparer();
        }

        
    }
}