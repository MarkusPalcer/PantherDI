using System;
using System.Collections.Generic;
using System.Reflection;
using PantherDI.Registry.Registration;

namespace PantherDI.Tests.Helpers
{
    public class Registration : IRegistration
    {
        public Type RegisteredType { get; set; }
        public ISet<object> FulfilledContracts { get; } = new HashSet<object>();
        public ISet<IFactory> Factories { get; } = new HashSet<IFactory>();
    }
}