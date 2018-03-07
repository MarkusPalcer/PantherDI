using System;
using System.Collections.Generic;
using System.Linq;
using PantherDI.ContainerCreation;
using PantherDI.Registry.Registration.Dependency;
using PantherDI.Resolved.Providers;

namespace PantherDI.Resolvers
{
    public class RegistrationProcessingResolver : IResolver
    {
        internal readonly RegistrationConverter Converter;

        public RegistrationProcessingResolver(RegistrationConverter converter)
        {
            Converter = converter;
        }

        #region Implementation of IResolver

        public IEnumerable<IProvider> Resolve(Func<IDependency, IEnumerable<IProvider>> dependencyResolver, IDependency dependency)
        {
            Converter.ProcessContract(dependency.RequiredContracts.First(), dependencyResolver);
            return Converter.KnowledgeBase.Resolve(dependencyResolver, dependency);
        }

        #endregion
    }
}