using System.Collections.Generic;
using PantherDI.Registry.Registration.Registration;

namespace PantherDI.Registry.Catalog
{
    /// <summary>
    /// A catalog to be filled manually with <see cref="IRegistration"/>s
    /// </summary>
    public class ManualCatalog : ICatalog
    {
        public ManualCatalog(params IRegistration[] registrations)
        {
            Registrations = registrations;
        }

        public IEnumerable<IRegistration> Registrations { get; }
    }
}