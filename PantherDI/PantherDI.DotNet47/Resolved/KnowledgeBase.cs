﻿using System;
using System.Collections.Generic;
using System.Linq;
using PantherDI.Registry.Registration.Dependency;
using System.Reflection;

namespace PantherDI.Resolved
{
    public class KnowledgeBase : IKnowledgeBase
    {
        internal Dictionary<object, List<IProvider>> KnownProviders { get; } = new Dictionary<object, List<IProvider>>();

        public IEnumerable<IProvider> this[object contract] 
            => KnownProviders.TryGetValue(contract, out var result) 
                ? result 
                : Enumerable.Empty<IProvider>();

        public void Add(IProvider provider)
        {
            foreach (var contract in provider.FulfilledContracts)
            {
                if (!KnownProviders.ContainsKey(contract))
                {
                    KnownProviders.Add(contract, new List<IProvider>());
                }

                KnownProviders[contract].Add(provider);
            }
        }

        public IEnumerable<IProvider> Resolve(Func<IDependency, IEnumerable<IProvider>> dependencyResolver, IDependency dependency)
        {
            if (dependency == null)
            {
                throw new ArgumentNullException(nameof(dependency));
            }

            return this[dependency.RequiredContracts.First()]
                .Where(x => x.FulfilledContracts.IsSupersetOf(dependency.RequiredContracts))
                .Where(x => dependency.ExpectedType.GetTypeInfo().IsAssignableFrom(x.ResultType))
                .ToArray();
        }
    }
}