﻿using System;
using System.Collections.Generic;
using System.Reflection;
using PantherDI.Registry.Registration.Dependency;
using PantherDI.Resolved.Providers;
using PantherDI.Tests.Resolvers;

namespace PantherDI.Tests.Helpers
{
    public class TestProvider : IProvider

    {
        private readonly Func<Dictionary<IDependency, object>, object> _delegate;

        public TestProvider(object value) : this(_ => value)
        {
        }

        public TestProvider(Func<Dictionary<IDependency, object>, object> @delegate)
        {
            _delegate = @delegate;
        }

        public int InvocationCounter { get; set; } = 0;

        public HashSet<IDependency> UnresolvedDependencies { get; set;  } = new HashSet<IDependency>(new Dependency.EqualityComparer());

        public Type ResultType { get; set; }

        public ISet<object> FulfilledContracts { get; set; } = new HashSet<object>();

        public object CreateInstance(Dictionary<IDependency, object> resolvedDependencies)
        {
            InvocationCounter++;
            return _delegate(resolvedDependencies);
        }

        public bool Singleton { get; set; }
    }
}