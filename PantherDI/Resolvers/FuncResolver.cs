using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PantherDI.Extensions;
using PantherDI.Registry.Registration.Dependency;
using PantherDI.Resolved.Providers;

namespace PantherDI.Resolvers
{
    public static class FuncResolver
    {
        internal static Func<Dictionary<IDependency, object>, Func<object[], T>> ProcessProvider<T>(Type[] givenTypes, IProvider provider)
        {
            var matches = true;
            var givenDependencies = new Dictionary<IDependency, int>();

            foreach (var type in givenTypes.Select((x, i) => new {Type = x, Index = i}))
            {
                var dependenciesForType = provider.UnresolvedDependencies.Where(x => x.ExpectedType.GetTypeInfo().IsAssignableFrom(type.Type.GetTypeInfo()))
                                                  .ToArray();

                if (!dependenciesForType.Any())
                {
                    matches = false;
                    break;
                }

                foreach (var dependency in dependenciesForType)
                {
                    givenDependencies[dependency] = type.Index;
                }
            }

            if (!matches)
            {
                return null;
            }

            Func<object[], T> Result(Dictionary<IDependency, object> objects)
            {
                T Creator(object[] @params)
                {
                    objects = objects.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                    foreach (var dependency in givenDependencies)
                    {
                        objects[dependency.Key] = @params[dependency.Value];
                    }

                    return (T) provider.CreateInstance(objects);
                }

                return Creator;
            }

            return Result;
        }
    }
}