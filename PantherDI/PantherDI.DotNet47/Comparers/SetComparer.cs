using System.Collections.Generic;

namespace PantherDI.Comparers
{
    public class SetComparer<T> : IEqualityComparer<ISet<T>>
    {
        public bool Equals(ISet<T> x, ISet<T> y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(null, x) || ReferenceEquals(null, y)) return false;
            return x.SetEquals(y);
        }

        public int GetHashCode(ISet<T> obj)
        {
            var result = 0;
            foreach (var item in obj)
            {
                result = (result * 397) ^ item.GetHashCode();
            }

            return result;
        }

        public static IEqualityComparer<ISet<T>> Instance { get; } = new SetComparer<T>();
    }
}