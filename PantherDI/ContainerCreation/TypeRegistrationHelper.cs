using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PantherDI.Registry.Registration.Factory;
using PantherDI.Registry.Registration.Registration;

namespace PantherDI.ContainerCreation
{
    /// <summary>
    /// Provides a fluent interface for configuring a registered type
    /// </summary>
    public class TypeRegistrationHelper
    {
        private readonly Type _type;

        internal TypeRegistrationHelper(Type type)
        {
            _type = type;
            _registration = new ManualRegistration()
            {
                RegisteredType = _type
            };
        }

        private readonly ManualRegistration _registration;

        /// <summary>
        /// The contracts the registered type fulfills
        /// </summary>
        public ISet<object> Contracts => _registration.FulfilledContracts;

        /// <summary>
        /// Whether reflection should be used to determine which contracts this type fulfils
        /// </summary>
        public bool RegisterWithReflection { get; set; } = false;

        /// <summary>
        /// The factories that create the registered type
        /// </summary>
        public ISet<IFactory> Factories => _registration.Factories;

        /// <summary>
        /// Whether to register the types' constructors as factories
        /// </summary>
        public bool RegisterWithConstructors { get; set; } = false;

        /// <summary>
        /// Whether the given type should be treated as a singleton
        /// </summary>
        public bool IsSingleton { get; set; } = false;

        /// <summary>
        /// Gets the dictionary of metadata for the registered type
        /// </summary>
        public Dictionary<string, object> Metadata => _registration.Metadata;

        internal void RegisterTo(ContainerBuilder cb)
        {
            if (RegisterWithReflection)
            {
                cb.WithType(_type);
            }

            if (RegisterWithConstructors)
            {
                foreach (var factory in _type.GetTypeInfo().DeclaredConstructors.Select(x => new ConstructorFactory(x)))
                {
                    Factories.Add(factory);
                }
            }

            _registration.Singleton = IsSingleton;

            cb.Registrations.Add(_registration);
        }

        /// <summary>
        /// Adds the given type as contract
        /// </summary>
        public TypeRegistrationHelper As<TContract>()
        {
            if (_type.GetTypeInfo().IsSubclassOf(typeof(TContract)))
            {
                throw new ArgumentException($"${_type.Name} is not assignable to  ${typeof(TContract).Name}, so it can't be registered as such.");
            }

            Contracts.Add(typeof(TContract));

            return this;
        }

        /// <summary>
        /// Adds a contract to the list of fulfilled contracts
        /// </summary>
        public TypeRegistrationHelper WithContract(object contract)
        {
            Contracts.Add(contract);

            return this;
        }

        /// <summary>
        /// Enables the use of reflection to determine fulfilled contracts
        /// </summary>
        public TypeRegistrationHelper UsingReflection()
        {
            RegisterWithReflection = true;
            return this;
        }

        /// <summary>
        /// Enables the use of reflection to add the types' constructors as factories
        /// </summary>
        public TypeRegistrationHelper WithConstructors()
        {
            RegisterWithConstructors = true;
            return this;
        }

        /// <summary>
        /// Flags the type as singleton
        /// </summary>
        public TypeRegistrationHelper AsSingleton()
        {
            IsSingleton = true;
            return this;
        }

        /// <summary>
        /// Adds a factory for the registered type
        /// </summary>
        public TypeRegistrationHelper WithFactory(IFactory factory)
        {
            Factories.Add(factory);
            return this;
        }

        /// <summary>
        /// Adds a metadata entry 
        /// </summary>
        public TypeRegistrationHelper WithMetadata(string key, object value)
        {
            Metadata[key] = value;
            return this;
        }
    }
}