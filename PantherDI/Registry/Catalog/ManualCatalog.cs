using System.Collections.Generic;
using PantherDI.Registry.Registration.Registration;

namespace PantherDI.Registry.Catalog
{
    public class ManualCatalog : ICatalog
    {
        public ManualCatalog(params IRegistration[] registrations)
        {
            Registrations = registrations;
        }

        public IEnumerable<IRegistration> Registrations { get; }
    }
}