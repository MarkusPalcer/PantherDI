using System;

namespace PantherDI.Attributes
{
    /// <summary>
    /// Specifies that something should not be considered by reflection.
    /// 
    /// If placed on a class, the type will not be registered, regardless of fulfilled contracts
    /// If placed on a constructor, it will not be used as factory for the type
    /// If placed on a parameter of a factory, that parameter will not be resolved when converting the factory to a provider
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Parameter, Inherited = false)]
    public class IgnoreAttribute : Attribute
    {
    }
}