using System.Collections.Generic;
using System.Linq;

namespace PantherDI.Registry.Registration.Factory
{
    public class FactoryEqualityComparer : IEqualityComparer<IFactory>
    {
        readonly Dependency.Dependency.EqualityComparer _dependencyComparer = new Dependency.Dependency.EqualityComparer();

        #region Implementation of IEqualityComparer<in IFactory>

        public bool Equals(IFactory x, IFactory y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;
            if (x.Dependencies.Count() != y.Dependencies.Count()) return false;
            if (x.FulfilledContracts.Count() != y.FulfilledContracts.Count()) return false;

            if (!x.Dependencies.All(xd => y.Dependencies.Any(yd => _dependencyComparer.GetHashCode(xd) == _dependencyComparer.GetHashCode(yd) &&
                                                                   _dependencyComparer.Equals(xd, yd)))) return false;

            if (!x.FulfilledContracts.All(xd => y.FulfilledContracts.Any(yd => Equals(xd, yd)))) return false;

            return true;
        }

        public int GetHashCode(IFactory obj)
        {
            var result = obj.Dependencies.Aggregate(0, (current, item) => (current * 397) ^ _dependencyComparer.GetHashCode(item));
            result = (result * 397) ^ obj.FulfilledContracts.GetHashCode();
            return result;
        }

        #endregion
    }
}