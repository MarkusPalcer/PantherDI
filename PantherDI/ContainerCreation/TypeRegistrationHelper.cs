using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PantherDI.Extensions;
using PantherDI.Registry.Registration.Factory;
using PantherDI.Registry.Registration.Registration;

namespace PantherDI.ContainerCreation
{
    /// <summary>
    /// Provides a fluent interface for configuring a registered type
    /// </summary>
    public class TypeRegistrationHelper : ContainerBuilder.IRegistrationHelper
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
        public bool UseReflectionForContracts { get; set; } = false;

        /// <summary>
        /// Whether to register the types' constructors as factories
        /// </summary>
        public bool UseConstructorsAsFactories { get; set; } = false;

        /// <summary>
        /// Whether to scan the type for metadata using reflection
        /// </summary>
        public bool UseReflectionForMetadata { get; set; } = false;

        /// <summary>
        /// Whether the given type should be treated as a singleton
        /// </summary>
        public bool IsSingleton { get; set; } = false;

        /// <summary>
        /// Gets the dictionary of metadata for the registered type
        /// </summary>
        public Dictionary<string, object> Metadata => _registration.Metadata;

        /// <summary>
        /// The factories that create the registered type
        /// </summary>
        public ISet<IFactory> Factories => _registration.Factories;

        void ContainerBuilder.IRegistrationHelper.RegisterTo(ContainerBuilder cb)
        {
            if (UseReflectionForContracts)
            {
                foreach (var contract in _type.GetTypeInfo().GetFulfilledContracts())
                {
                    Contracts.Add(contract);
                }
            }

            if (UseConstructorsAsFactories)
            {
                foreach (var factory in _type.GetTypeInfo().DeclaredConstructors.Select(x => new ConstructorFactory(x)))
                {
                    Factories.Add(factory);
                }
            }

            if (UseReflectionForMetadata)
            {
                TypeRegistration.CollectMetadata(_type, Metadata);
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
        public TypeRegistrationHelper WithContractsViaReflection()
        {
            UseReflectionForContracts = true;
            return this;
        }

        /// <summary>
        /// Enables the use of reflection to add the types' constructors as factories
        /// </summary>
        public TypeRegistrationHelper WithConstructors()
        {
            UseConstructorsAsFactories = true;
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
        /// Enables searching for metadata via reflection
        /// </summary>
        public TypeRegistrationHelper WithMetadataViaReflection()
        {
            UseReflectionForMetadata = true;
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