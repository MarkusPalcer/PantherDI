using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PantherDI.Registry.Registration.Dependency;
using PantherDI.Resolved.Providers;

namespace PantherDI.Resolvers
{
    /// <summary>
    /// Resolves the given dependency by selecting the first resolver that produces an actual result
    /// </summary>
    public class MergedResolver : IList<IResolver>, IResolver
    {
        private readonly List<IResolver> _resolvers = new List<IResolver>();

        public MergedResolver() : this(Enumerable.Empty<IResolver>())
        {
        }

        public MergedResolver(IEnumerable<IResolver> resolver)
        {
            _resolvers.AddRange(resolver);
        }

        #region IList<IResolver>
        public IEnumerator<IResolver> GetEnumerator()
        {
            return _resolvers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _resolvers).GetEnumerator();
        }

        public void Add(IResolver item)
        {
            _resolvers.Add(item);
        }

        public void Clear()
        {
            _resolvers.Clear();
        }

        public bool Contains(IResolver item)
        {
            return _resolvers.Contains(item);
        }

        public void CopyTo(IResolver[] array, int arrayIndex)
        {
            _resolvers.CopyTo(array, arrayIndex);
        }

        public bool Remove(IResolver item)
        {
            return _resolvers.Remove(item);
        }

        public int Count => _resolvers.Count;

        public bool IsReadOnly => true;

        public int IndexOf(IResolver item)
        {
            return _resolvers.IndexOf(item);
        }

        public void Insert(int index, IResolver item)
        {
            _resolvers.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _resolvers.RemoveAt(index);
        }

        public IResolver this[int index]
        {
            get => _resolvers[index];
            set => _resolvers[index] = value;
        }
        #endregion

        public IEnumerable<IProvider> Resolve(Func<IDependency, IEnumerable<IProvider>> dependencyResolver, IDependency dependency)
        {
            return _resolvers
                .Select(r => r.Resolve(dependencyResolver, dependency))
                .FirstOrDefault(r => r != null && r.Any()) ?? Enumerable.Empty<IProvider>();
        }
    }
}