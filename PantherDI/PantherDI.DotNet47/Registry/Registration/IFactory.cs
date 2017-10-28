using System.Collections.Generic;

namespace PantherDI.Registry.Registration
{
    /// <summary>
    /// A factory for an <see cref="IRegistration"/> with no dependencies resolved.
    /// </summary>
    public interface IFactory 
    {
        /// <summary>
        /// Executes the factory, returning the created object
        /// </summary>
        /// <param name="resolvedDependencies">The resolved dependencies in the same order as given by the <see cref="Dependencies"/>-property</param>
        /// <returns>The instance this factory creates</returns>
        object Execute(object[] resolvedDependencies);

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> containing all dependencies that need to be resolved and provided to the <see cref="Execute"/>-method
        /// </summary>
        IEnumerable<IDependency> Dependencies { get; }
    }
}