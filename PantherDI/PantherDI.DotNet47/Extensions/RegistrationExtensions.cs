using System.Collections.Generic;
using PantherDI.Registry.Registration;
using PantherDI.Registry.Registration.Registration;

namespace PantherDI.Extensions
{
    public static class RegistrationExtensions
    {
        internal static ManualRegistration Clone(this IRegistration x)
        {
            return new ManualRegistration(new HashSet<object>(x.FulfilledContracts) { x.RegisteredType }, new HashSet<IFactory>(x.Factories))
            {
                RegisteredType = x.RegisteredType,
                Singleton = x.Singleton
            };
        }
    }
}