using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PantherDI.Registry.Registration.Registration;

namespace PantherDI.Registry.Catalog
{
    public class TypeCatalog : ICollection<Type>, ICatalog
    {
        private readonly List<Type> _types = new List<Type>();

        #region Implementation of IEnumerable

        public IEnumerator<Type> GetEnumerator()
        {
            return _types.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _types).GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<Type>

        public void Add(Type item)
        {
            _types.Add(item);
        }

        public void Clear()
        {
            _types.Clear();
        }

        public bool Contains(Type item)
        {
            return _types.Contains(item);
        }

        public void CopyTo(Type[] array, int arrayIndex)
        {
            _types.CopyTo(array, arrayIndex);
        }

        public bool Remove(Type item)
        {
            return _types.Remove(item);
        }

        public int Count => _types.Count;

        public bool IsReadOnly => ((ICollection<Type>)_types).IsReadOnly;

        #endregion

        #region Implementation of ICatalog

        public IEnumerable<IRegistration> Registrations => this.Select(x => new TypeRegistration(x.GetTypeInfo()));

        #endregion
    }
}