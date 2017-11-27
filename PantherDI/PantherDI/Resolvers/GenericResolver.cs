using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PantherDI.Registry.Registration.Dependency;
using PantherDI.Resolved.Providers;

namespace PantherDI.Resolvers
{
    public class GenericResolver : IResolver
    {
        private readonly Type _genericType;
        private readonly Type _resolverType;

        public GenericResolver(Type genericType, Type resolverType)
        {
            _genericType = genericType;
            _resolverType = resolverType;
            
        }

        public IEnumerable<IProvider> Resolve(Func<IDependency, IEnumerable<IProvider>> dependencyResolver, IDependency dependency)
        {
            // Only handle The registered type
            if (dependency.ExpectedType.GetTypeInfo().IsGenericType &&
                dependency.ExpectedType.GetTypeInfo().GetGenericTypeDefinition() != _genericType)
            {
                return Enumerable.Empty<IProvider>();
            }

            var innerResolver = (IResolver)_resolverType.MakeGenericType(dependency.ExpectedType.GenericTypeArguments).GetTypeInfo().DeclaredConstructors.First().Invoke(new object[] { });

            return innerResolver.Resolve(dependencyResolver, dependency);
        }
    }
}