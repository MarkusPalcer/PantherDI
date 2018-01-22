using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PantherDI.Attributes;
using PantherDI.Registry.Registration.Dependency;

namespace PantherDI.Registry.Registration.Factory
{
    public abstract class MethodBaseFactory : IFactory
    {
        public MethodBaseFactory(MethodBase method)
        {
            Dependencies = method.GetParameters()
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

            FulfilledContracts = new HashSet<object>(method.GetCustomAttributes<ContractAttribute>().Select(x => x.Contract ?? method.DeclaringType));
        }

        public IEnumerable<IDependency> Dependencies { get; }
        public IEnumerable<object> FulfilledContracts { get; }

        public abstract object Execute(object[] resolvedDependencies);
    }


}