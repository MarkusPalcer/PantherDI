using System.Collections.Generic;
using PantherDI.Registry.Registration.Registration;

namespace PantherDI.Registry.Registration.Factory
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
        IEnumerable<Dependency> Dependencies { get; }

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> containing contracts additional to the type itself
        /// </summary>
        IEnumerable<object> FulfilledContracts { get; }

        /// <summary>
        /// Gets the Priority of this factory.
        /// </summary>
        /// <remarks>If not given the priority of the registration will be used</remarks>
        int? Priority { get; }
    }
}