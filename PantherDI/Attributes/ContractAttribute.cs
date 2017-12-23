using System;

namespace PantherDI.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public class ContractAttribute : Attribute
    {
        public ContractAttribute() : this(null)
        {
        }

        public ContractAttribute(object contract)
        {
            Contract = contract;
        }

        public object Contract { get; }
    }
}