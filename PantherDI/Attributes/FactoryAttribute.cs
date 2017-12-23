using System;

namespace PantherDI.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class FactoryAttribute : Attribute
    {
    }
}