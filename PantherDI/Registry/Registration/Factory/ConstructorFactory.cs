using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PantherDI.Attributes;
using PantherDI.Registry.Registration.Dependency;

namespace PantherDI.Registry.Registration.Factory
{
    /// <summary>
    /// A factory that invokes a constructor when executed
    /// </summary>
    public class ConstructorFactory : IFactory
    {
        private readonly ConstructorInfo _constructor;

        public ConstructorFactory(ConstructorInfo constructor)
        {
            _constructor = constructor;

            Dependencies = _constructor.GetParameters()
                .Select(parameter =>
                {
                    var result = new Dependency.Dependency(parameter.ParameterType);
                    result.RequiredContracts.Clear();
                    foreach (var attribute in parameter.GetCustomAttributes<ContractAttribute>())
                    {
                        result.RequiredContracts.Add(attribute.Contract ?? parameter.ParameterType);
                    }

                    if (!result.RequiredContracts.Any())
                    {
                        result.RequiredContracts.Add(parameter.ParameterType);
                    }

                    result.Ignored = parameter.GetCustomAttributes<IgnoreAttribute>().Any();

                    return result;
                }).ToArray();

            Contracts = new HashSet<object>(_constructor.GetCustomAttributes<ContractAttribute>().Select(x => x.Contract ?? _constructor.DeclaringType));
        }

        public object Execute(object[] resolvedDependencies)
        {
            return _constructor.Invoke(resolvedDependencies);
        }

        public IEnumerable<IDependency> Dependencies { get; }
        public IEnumerable<object> Contracts { get; }
    }
}