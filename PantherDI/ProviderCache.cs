using System;
using System.Collections.Generic;
using PantherDI.Comparers;
using PantherDI.Registry.Registration;
using PantherDI.Resolved.Providers;

namespace PantherDI
{
    internal class ProviderCache 
    {
        private readonly Dictionary<Type, Dictionary<ISet<object>, IProvider[]>> _data = new Dictionary<Type, Dictionary<ISet<object>, IProvider[]>>();

        public IProvider[] this[Dependency key]
        {
            get
            {
                if (!_data.TryGetValue(key.ExpectedType, out var typeEntry))
                {
                    return null;
                }

                if (!typeEntry.TryGetValue(key.RequiredContracts, out var results))
                {
                    return null;
                }

                return results;
            }

            set
            {
                if (!_data.TryGetValue(key.ExpectedType, out var typeEntry))
                {
                    typeEntry = new Dictionary<ISet<object>, IProvider[]>(SetComparer<object>.Instance);
                    _data[key.ExpectedType] = typeEntry;
                }

                typeEntry[key.RequiredContracts] = value;
            }
        }
    }
}