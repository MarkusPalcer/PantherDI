using System.Collections.Generic;
using PantherDI.Registry.Catalog;
using PantherDI.Registry.Registration.Registration;

namespace PantherDI.Tests.Helpers
{
    public class Catalog : ICatalog
    {
        public Catalog(params IRegistration[] registrations)
        {
            Registrations = registrations;
        }

        public IEnumerable<IRegistration> Registrations { get; }
    }
}