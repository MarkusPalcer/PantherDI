using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PantherDI.ContainerCreation;
using PantherDI.Registry.Registration;
using PantherDI.Registry.Registration.Registration;
using PantherDI.Resolved.Providers;

namespace PantherDI.Resolvers
{
    /// <summary>
    /// Tries to resolve a given type by processing its constructors and attributes via reflection.
    /// </summary>
    public class ReflectionResolver : IResolver
    {
        #region Implementation of IResolver

        public IEnumerable<IProvider> Resolve(Func<Dependency, IEnumerable<IProvider>> dependencyResolver, Dependency dependency)
        {
            var requiredTypes = dependency.RequiredContracts.Select(x => x as Type).Where(x => x != null).ToArray();
            var allowedTypes = new HashSet<Type>(requiredTypes) {dependency.ExpectedType};
            var foundTypes = allowedTypes
                .Select(t => t.GetTypeInfo())
                .Where(t => !t.IsAbstract)
                .Where(t => !t.IsInterface)
                .Where(dependency.ExpectedType.GetTypeInfo().IsAssignableFrom)
                .ToArray();
            var registrations = foundTypes
                .Select(x => new TypeRegistration(x)).ToArray();

            foreach (var registration in registrations.Where(x => x.FulfilledContracts.IsSubsetOf(dependency.RequiredContracts)))
            {
                foreach (var factory in registration.Factories)
                {
                    foreach (var provider in RegistrationConverter.ProcessFactory(new RegisteredFactory
                    {
                        Factory = factory,
                        Registration = registration
                    }, dependencyResolver))
                    {
                        yield return provider;
                    }
                }
            }
        }

        #endregion
    }
}