using System;
using System.Collections.Generic;
using System.Linq;
using PantherDI.Extensions;
using PantherDI.Registry.Registration.Factory;
using PantherDI.Registry.Registration.Registration;

namespace PantherDI.Registry.Catalog
{
    public class MergedCatalog : ICatalog
    {
        public MergedCatalog(params ICatalog[] catalogs)
        {
            var registrations = new Dictionary<Type, ManualRegistration>();

            foreach (var registration in catalogs.SelectMany(x => x.Registrations))
            {
                if (!registrations.TryGetValue(registration.RegisteredType, out var mergedRegistration))
                {
                    mergedRegistration = registration.Clone();
                    registrations[registration.RegisteredType] = mergedRegistration;
                }
                else
                {
                    foreach (var contract in registration.FulfilledContracts)
                    {
                        mergedRegistration.FulfilledContracts.Add(contract);
                    }

                    mergedRegistration.Singleton |= registration.Singleton;

                    foreach (var factory in registration.Factories)
                    {
                        if (!mergedRegistration.Factories.Any(x => new FactoryEqualityComparer().Equals(x, factory)))
                        {
                            mergedRegistration.Factories.Add(factory);
                        }
                    }
                }
            }

            Registrations = registrations.Values;
        }

        #region Implementation of ICatalog

        public IEnumerable<IRegistration> Registrations { get; }

        #endregion


    }
}