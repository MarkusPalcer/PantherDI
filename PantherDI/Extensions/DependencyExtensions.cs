using System.Linq;
using PantherDI.Registry.Registration;

namespace PantherDI.Extensions
{
    public static class DependencyExtensions
    {
        public static Dependency ReplaceExpectedType<TNew>(this Dependency dependency)
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