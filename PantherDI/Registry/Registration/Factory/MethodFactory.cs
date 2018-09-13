using System.Reflection;

namespace PantherDI.Registry.Registration.Factory
{
    /// <summary>
    /// A factory that invokes a method specified by its <see cref="MethodInfo"/>
    /// </summary>
    public class MethodFactory : MethodBaseFactory
    {
        private readonly MethodInfo _method;

        public MethodFactory(MethodInfo method) : base(method)
        {
            _method = method;
        }

        #region Implementation of IFactory

        public override object Execute(object[] resolvedDependencies)
        {
            return _method.Invoke(null, resolvedDependencies);
        }
        #endregion
    }
}