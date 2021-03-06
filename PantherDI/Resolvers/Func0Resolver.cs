﻿using System;
using System.Collections.Generic;
using System.Linq;
using PantherDI.Extensions;
using PantherDI.Registry.Registration;
using PantherDI.Resolved.Providers;

namespace PantherDI.Resolvers
{
    public class Func0Resolver : GenericResolver
    {
        public Func0Resolver() : base(typeof(Func<>), typeof(InnerResolver<>))
        {
        }

        private class InnerResolver<T> : IResolver
        {
            public IEnumerable<IProvider> Resolve(Func<Dependency, IEnumerable<IProvider>> dependencyResolver, Dependency dependency)
            {
                return dependencyResolver(dependency.ReplaceExpectedType<T>())
                    .Select(provider => DelegateProvider.WrapProvider<Func<T>>(objects => (Func<T>) (() => (T) provider.CreateInstance(objects)), provider));
            }
        }
    }
}