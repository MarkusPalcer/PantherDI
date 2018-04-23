using System.Collections.Generic;
using System.Linq;
using PantherDI.Registry.Registration.Factory;
using PantherDI.Registry.Registration.Registration;

namespace PantherDI.Extensions
{
    public static class RegistrationExtensions
    {
        internal static ManualRegistration Clone(this IRegistration x)
        {
            return new ManualRegistration(new HashSet<object>(x.FulfilledContracts) { x.RegisteredType }, new HashSet<IFactory>(x.Factories), x.Metadata.ToDictionary(m => m.Key, m => m.Value))
            {
                RegisteredType = x.RegisteredType,
                Singleton = x.Singleton,
                Priority = x.Priority
            };
        }
    }
}