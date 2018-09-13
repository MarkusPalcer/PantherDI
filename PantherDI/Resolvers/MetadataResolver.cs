using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PantherDI.Attributes;
using PantherDI.Extensions;
using PantherDI.Registry.Registration;
using PantherDI.Resolved.Providers;

namespace PantherDI.Resolvers
{
    /// <summary>
    /// Resolves <see cref="Lazy{T,TMetadata}"/> by returning a <see cref="Lazy{T,TMetadata}"/> for each provider for <code>T</code>
    /// that has no leftover dependencies and mapping the registration metadata to <code>TMetadata</code>
    /// </summary>
    public class MetadataResolver : GenericResolver
    {
        public MetadataResolver() : base(typeof(Lazy<,>), typeof(InnerResolver<,>)) { }

        public class InnerResolver<T, TMetadata> : IResolver where TMetadata : new()
        {
            #region Implementation of IResolver

            public IEnumerable<IProvider> Resolve(Func<Dependency, IEnumerable<IProvider>> dependencyResolver, Dependency dependency)
            {
                var requestedMetadata = typeof(TMetadata)
                    .GetTypeInfo()
                    .DeclaredProperties
                    .Where(x => !x.IsSpecialName)
                    .Where(x => !x.GetCustomAttributes<IgnoreAttribute>().Any())
                    .Where(x => x.CanWrite)
                    .ToDictionary<PropertyInfo, string, Action<TMetadata, object>>(property => property.GetCustomAttributes<MetadataAttribute>().SingleOrDefault()?.Key ?? property.Name,
                                                                                   property => ((metadata, value) => property.SetValue(metadata, value)));

                var providers = dependencyResolver(dependency.ReplaceExpectedType<T>()).ToArray();

                foreach (var provider in providers)
                {
                    var metadataSetters = new List<Action<TMetadata>>();

                    foreach (var entry in requestedMetadata)
                    {
                        if (provider.Metadata.TryGetValue(entry.Key, out var value))
                        {
                            metadataSetters.Add(x => entry.Value(x, value));
                        }
                    }

                    object ProviderFunction(Dictionary<Dependency, object> x)
                    {
                        var result = new Lazy<T, TMetadata>(() => (T) provider.CreateInstance(x));

                        foreach (var metadataSetter in metadataSetters)
                        {
                            metadataSetter(result.Metadata);
                        }

                        return result;
                    }

                    var p = new DelegateProvider(ProviderFunction, provider.Metadata)
                    {
                        FulfilledContracts = new HashSet<object>(provider.FulfilledContracts),
                        UnresolvedDependencies = provider.UnresolvedDependencies,
                        ResultType = typeof(Lazy<T>),
                        Singleton = provider.Singleton
                    };

                    yield return p;
                }
            }

            #endregion
        }
    }
}