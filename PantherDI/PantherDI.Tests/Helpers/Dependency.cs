using System;
using System.Collections.Generic;
using System.Reflection;
using PantherDI.Registry.Registration;

namespace PantherDI.Tests.Helpers
{
    public class Dependency : IDependency
    {
        public Dependency(Type expectedType, params object[] requiredContracts)
        {
            ExpectedType = expectedType;
            RequiredContracts = requiredContracts;
        }

        public Type ExpectedType { get; }
        public IEnumerable<object> RequiredContracts { get; }
    }
}