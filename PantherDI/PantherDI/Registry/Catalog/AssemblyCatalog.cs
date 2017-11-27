using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static PantherDI.Extensions.TypeExtensions;
using PantherDI.Registry.Registration.Registration;

namespace PantherDI.Registry.Catalog
{
    public class AssemblyCatalog : ICatalog
    {
        public AssemblyCatalog(Assembly assembly)
        {
            Registrations = assembly.DefinedTypes
                .Select(x => new {Type = x, Contracts = x.ScanForContracts().ToArray()}).ToArray()
                .Where(x => x.Contracts.Any())
                .Where(x => x.Type.DeclaredConstructors.Any())
                .Where(x => !x.Type.IsAbstract)
                .Where(x => !x.Type.IsInterface)
                .Select(x => new TypeRegistration(x.Type, x.Contracts))
                .ToArray();
        }

        #region Implementation of ICatalog

        public IEnumerable<IRegistration> Registrations { get; }

        #endregion
    }
}