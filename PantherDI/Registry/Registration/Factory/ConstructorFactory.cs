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
    public class ConstructorFactory : MethodBaseFactory
    {
        private readonly ConstructorInfo _constructor;

        public ConstructorFactory(ConstructorInfo constructor) : base(constructor)
        {
            _constructor = constructor;
        }

        public override object Execute(object[] resolvedDependencies)
        {
            return _constructor.Invoke(resolvedDependencies);
        }
    }
}