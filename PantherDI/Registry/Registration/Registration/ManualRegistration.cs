using System;
using System.Collections.Generic;
using System.Linq;
using PantherDI.Registry.Registration.Factory;

namespace PantherDI.Registry.Registration.Registration
{
    internal class ManualRegistration : IRegistration
    {
        public Type RegisteredType { get; set; }
        public ISet<object> FulfilledContracts { get; }
        public ISet<IFactory> Factories { get; }
        public bool Singleton { get; set; }

        public Dictionary<string, object> Metadata { get; }

        IReadOnlyDictionary<string, object> IRegistration.Metadata => Metadata;

        public ManualRegistration()
        {
            FulfilledContracts = new HashSet<object>();
            Factories = new HashSet<IFactory>();
            Metadata = new Dictionary<string, object>();
        }

        public ManualRegistration(ISet<object> fulfilledContracts, ISet<IFactory> factories, IEnumerable<KeyValuePair<string, object>> metadata)
        {
            FulfilledContracts = fulfilledContracts;
            Factories = factories;
            Metadata = metadata?.ToDictionary(x => x.Key, x=> x.Value) ?? new Dictionary<string, object>();
        }
    }
}