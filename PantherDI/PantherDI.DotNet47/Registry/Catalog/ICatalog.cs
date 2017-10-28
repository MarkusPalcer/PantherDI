using System.Collections.Generic;
using PantherDI.Registry.Registration;

namespace PantherDI.Registry.Catalog
{
    /// <summary>
    /// The catalog describes all types that will be known to a created container
    /// </summary>
    public interface ICatalog 
    {
        /// <summary>
        /// Gets the registrations to add to the container when it's created
        /// </summary>
        IEnumerable<IRegistration> Registrations { get; }
    }
}