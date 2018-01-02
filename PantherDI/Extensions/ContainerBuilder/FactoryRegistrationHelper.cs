using System;
using System.Collections.Generic;
using PantherDI.Registry.Registration.Dependency;
using PantherDI.Registry.Registration.Factory;
using PantherDI.Registry.Registration.Registration;

namespace PantherDI.Extensions.ContainerBuilder
{
    public class FactoryRegistrationHelper : ContainerCreation.ContainerBuilder.IRegistrationHelper
    {
        private readonly Type _registeredType;
        private readonly List<object> _contracts;
        private readonly List<IDependency> _dependencies;
        private bool _singleton;
        private readonly Func<object[], object> _delegate;

        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();

        public FactoryRegistrationHelper(Type registeredType, IFactory registeredFactory)
        {
            _contracts = new List<object>(registeredFactory.Contracts);
            _dependencies = new List<IDependency>(registeredFactory.Dependencies);
            _delegate = registeredFactory.Execute;
            _registeredType = registeredType;
        }

        public FactoryRegistrationHelper WithContracts(object[] contracts)
        {
            _contracts.AddRange(contracts);
            return this;
        }

        public FactoryRegistrationHelper WithContract(object contract)
        {
            _contracts.Add(contract);
            return this;
        }

        public FactoryRegistrationHelper WithMetadata(string key, object value)
        {
            Metadata[key] = value;
            return this;
        }

        public FactoryRegistrationHelper AsSingleton()
        {
            _singleton = true;
            return this;
        }

        #region Implementation of IRegistrationHelper

        public void RegisterTo(ContainerCreation.ContainerBuilder cb)
        {
            var wrapperFactory = new DelegateFactory(_delegate, _contracts, _dependencies);
            var wrapperRegistration = new ManualRegistration(new HashSet<object>(), new HashSet<IFactory>(new[] { wrapperFactory }), Metadata)
            {
                RegisteredType = _registeredType,
                Singleton = _singleton
            };

            cb.Registrations.Add(wrapperRegistration);
        }

        #endregion
    }
}