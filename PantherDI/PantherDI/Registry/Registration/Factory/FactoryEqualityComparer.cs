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

            return x.Dependencies.All(xd => y.Dependencies.Any(yd =>
                _dependencyComparer.GetHashCode(xd) == _dependencyComparer.GetHashCode(yd) &&
                _dependencyComparer.Equals(xd, yd)));
        }

        public int GetHashCode(IFactory obj)
        {
            return obj.Dependencies.Aggregate(0, (current, item) => (current * 397) ^ _dependencyComparer.GetHashCode(item));
        }

        #endregion
    }
}