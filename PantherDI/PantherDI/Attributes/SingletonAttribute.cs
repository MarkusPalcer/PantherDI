using System;

namespace PantherDI.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class SingletonAttribute : Attribute
    {
    }
}