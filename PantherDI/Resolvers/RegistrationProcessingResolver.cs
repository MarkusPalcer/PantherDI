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
        private readonly RegistrationConverter _converter;

        public RegistrationProcessingResolver(RegistrationConverter converter)
        {
            _converter = converter;
        }

        #region Implementation of IResolver

        public IEnumerable<IProvider> Resolve(Func<IDependency, IEnumerable<IProvider>> dependencyResolver, IDependency dependency)
        {
            _converter.ProcessContract(dependency.RequiredContracts.First(), dependencyResolver);
            return _converter.KnowledgeBase.Resolve(dependencyResolver, dependency);
        }

        #endregion
    }
}