using System.Linq;
using PantherDI.Registry.Registration.Dependency;

namespace PantherDI.Extensions
{
    public static class DependencyExtensions
    {
        public static IDependency ReplaceExpectedType<TNew>(this IDependency dependency)
        {
            var result = new Dependency(typeof(TNew));
            foreach (var contract in dependency.RequiredContracts.Where(x => !Equals(x, dependency.ExpectedType)))
            {
                result.RequiredContracts.Add(contract);
            }

            return result;
        }
    }
}