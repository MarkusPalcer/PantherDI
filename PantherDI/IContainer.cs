using System;

namespace PantherDI
{
    /// <summary>
    /// The core of PantherDI. It is where instances can be pulled from with their dependencies resolved
    /// </summary>
    public interface IContainer : IDisposable
    {
        /// <summary>
        /// Creates an instance of a type that satisfies the given constraints.
        /// </summary>
        /// <typeparam name="T">The expected return type. The resolved type needs to be assignable to this type</typeparam>
        /// <param name="contracts">
        /// The contracts the resolved type needs to fulfill.
        /// If none is given, the return type will be used as contract.
        /// </param>
        T Resolve<T>(params object[] contracts);
    }
}