using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PantherDI.Attributes;
using PantherDI.Extensions;
using PantherDI.Registry.Registration.Factory;
using PantherDI.Registry.Registration.Registration;

namespace PantherDI.Registry.Catalog
{
    /// <summary>
    /// A catalog that scans an <see cref="Assembly"/> for registrations using reflection and attributes
    /// </summary>
    public class AssemblyCatalog : ICatalog
    {
        public AssemblyCatalog(Assembly assembly)
        {
            var typeRegistrations = assembly.DefinedTypes
                                            .Select(x => new {Type = x, Contracts = x.ScanForContracts().ToArray()}).ToArray()
                                            .Where(x => x.Contracts.Any())
                                            .Where(x => x.Type.DeclaredConstructors.Any())
                                            .Where(x => !x.Type.IsAbstract)
                                            .Where(x => !x.Type.IsInterface)
                                            .Where(x => !x.Type.GetCustomAttributes<IgnoreAttribute>().Any())
                                            .Select(x => new TypeRegistration(x.Type, x.Contracts));

            var instanceRegistrationsViaProperties = assembly.DefinedTypes
                                                .SelectMany(t => t.DeclaredProperties)
                                                .Where(x => !x.IsSpecialName)
                                                .Where(x => x.GetMethod.IsStatic)
                                                .Select(x => Tuple.Create(x, x.GetCustomAttributes<ContractAttribute>()))
                                                .Where(x => x.Item2.Any())
                                                .Select(CreateInstanceRegistration);

            var instanceRegistrationsViaFields = assembly.DefinedTypes
                                                             .SelectMany(t => t.DeclaredFields)
                                                             .Where(x => !x.IsSpecialName)
                                                             .Where(x => x.IsStatic)
                                                             .Select(x => Tuple.Create(x, x.GetCustomAttributes<ContractAttribute>()))
                                                             .Where(x => x.Item2.Any())
                                                             .Select(CreateInstanceRegistration);

            var factoriesRegisteredViaMethods = assembly.DefinedTypes
                                                        .SelectMany(t => t.DeclaredMethods)
                                                        .Where(x => x.IsStatic && !x.IsSpecialName)
                                                        .Where(x => x.GetCustomAttributes<FactoryAttribute>().Any())
                                                        .Select(x => CreateRegistrationForFactory(new MethodFactory(x), x.ReturnType));

            Registrations = typeRegistrations
                .Concat(instanceRegistrationsViaProperties)
                .Concat(instanceRegistrationsViaFields)
                .Concat(factoriesRegisteredViaMethods)
                .ToArray();
        }

        private IRegistration CreateInstanceRegistration(Tuple<PropertyInfo, IEnumerable<ContractAttribute>> arg)
        {
            var property = arg.Item1;
            var contracts = new HashSet<object>(arg.Item2.Select(a => a.Contract ?? property.Name));

            return new ManualRegistration(new HashSet<object>(), new HashSet<IFactory>(new[] { new InstanceFactory<object>(property.GetValue(null), contracts.ToArray()) }), new Dictionary<string, object>())
            {
                RegisteredType = property.PropertyType,
                Singleton = false
            };
        }

        private IRegistration CreateRegistrationForFactory(IFactory factory, Type registeredType)
        {
            return new ManualRegistration(new HashSet<object>(), new HashSet<IFactory>(new[] {factory}), new Dictionary<string, object>())
            {
                RegisteredType = registeredType,
                Singleton = false
            };
        }

        private IRegistration CreateInstanceRegistration(Tuple<FieldInfo, IEnumerable<ContractAttribute>> arg)
        {
            var field = arg.Item1;
            var contracts = new HashSet<object>(arg.Item2.Select(a => a.Contract ?? field.Name));
            var factory = new InstanceFactory<object>(field.GetValue(null), contracts.ToArray());

            return CreateRegistrationForFactory(factory, field.FieldType);
        }

        #region Implementation of ICatalog

        public IEnumerable<IRegistration> Registrations { get; }

        #endregion
    }
}