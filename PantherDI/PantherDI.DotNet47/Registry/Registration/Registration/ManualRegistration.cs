using System;
using System.Collections.Generic;

namespace PantherDI.Registry.Registration.Registration
{
    internal class ManualRegistration : IRegistration
    {
        public Type RegisteredType { get; set; }
        public ISet<object> FulfilledContracts { get; }
        public ISet<IFactory> Factories { get; }

        public ManualRegistration()
        {
            FulfilledContracts = new HashSet<object>();
            Factories = new HashSet<IFactory>();
        }

        public ManualRegistration(Type registeredType, ISet<object> fulfilledContracts, ISet<IFactory> factories)
        {
            RegisteredType = registeredType;
            FulfilledContracts = fulfilledContracts;
            Factories = factories;
        }
    }
}