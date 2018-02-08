using System.Collections.Generic;
using PantherDI.Registry.Registration.Factory;
using PantherDI.Registry.Registration.Registration;

namespace PantherDI.ContainerCreation
{
    public class InstanceRegistrationHelper<T> : IRegistrationHelper {
        private readonly T _instance;

        private readonly List<object> _contracts = new List<object>();

        internal InstanceRegistrationHelper(T instance)
        {
            _instance = instance;
        }

        void IRegistrationHelper.RegisterTo(ICatalogBuilder builder)
        {
            builder.WithRegistration(new ManualRegistration(new HashSet<object>(),
                                               new HashSet<IFactory>(new[] {new InstanceFactory<T>(_instance, _contracts.ToArray())}),
                                               new Dictionary<string, object>())
            {
                RegisteredType = typeof(T),
                Singleton = false
            });
        }

        public InstanceRegistrationHelper<T> WithContract(object contract)
        {
            _contracts.Add(contract);
            return this;
        }
    }
}