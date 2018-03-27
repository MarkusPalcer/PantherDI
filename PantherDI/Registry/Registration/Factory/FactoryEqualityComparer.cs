using System.Collections.Generic;
using System.Linq;

namespace PantherDI.Registry.Registration.Factory
{
    public class FactoryEqualityComparer : IEqualityComparer<IFactory>
    {
        #region Implementation of IEqualityComparer<in IFactory>

        public bool Equals(IFactory x, IFactory y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;
            if (x.Dependencies.Count() != y.Dependencies.Count()) return false;
            if (x.FulfilledContracts.Count() != y.FulfilledContracts.Count()) return false;

            if (!x.Dependencies.All(xd => y.Dependencies.Any(yd => xd.GetHashCode() == yd.GetHashCode() &&
                                                                   xd.Equals(yd)))) return false;

            if (!x.FulfilledContracts.All(xd => y.FulfilledContracts.Any(yd => Equals(xd, yd)))) return false;

            return true;
        }

        public int GetHashCode(IFactory obj)
        {
            var result = obj.Dependencies.Aggregate(0, (current, item) => (current * 397) ^ item.GetHashCode());
            result = (result * 397) ^ obj.FulfilledContracts.GetHashCode();
            return result;
        }

        #endregion
    }
}