using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PantherDI.Registry.Registration.Dependency;
using PantherDI.Registry.Registration.Registration;
using PantherDI.Resolved;
using PantherDI.Tests.Helpers;

namespace PantherDI.Tests.Resolved.Providers
{
    [TestClass]
    public class FactoryProviderTests
    {
        private const string FulfilledContract = "Fulfilled Contract";

        [TestMethod]
        public void NoDependencies()
        {
            var registration = new ManualRegistration
            {
                RegisteredType = typeof(string),
                FulfilledContracts = { FulfilledContract }
            };

            var factoryResult = Guid.NewGuid().ToString();

            var factory = new Factory(deps =>
            {
                deps.Should().BeEmpty();
                return factoryResult;
            });

            var sut = new FactoryProvider(registration, factory, new Dictionary<IDependency, IProvider>());

            sut.FulfilledContracts.Should().BeEquivalentTo(FulfilledContract, typeof(string));
            sut.ResultType.Should().Be(typeof(string).GetTypeInfo());
            sut.UnresolvedDependencies.Should().BeEmpty();

            var result = sut.CreateInstance(new Dictionary<IDependency, object>());
            result.Should().Be(factoryResult);
        }

        [TestMethod]
        public void AllDependenciesFulfilled()
        {
            var registration = new ManualRegistration
            {
                RegisteredType = typeof(string),
                FulfilledContracts = { FulfilledContract }
            };

            var factoryResult = Guid.NewGuid().ToString();

            var p1 = new TestProvider("Provider result 1");
            var p2 = new TestProvider("Provider result 2");
            var p3 = new TestProvider("Provider result 3");

            var factory = new Factory(deps =>
            {
                deps.Should().BeEquivalentTo(new object[]{"Provider result 1", "Provider result 2", "Provider result 3"});
                return factoryResult;
            }, 
            new Dependency(typeof(string), "Dependency 1"), 
            new Dependency(typeof(string), "Dependency 2"), 
            new Dependency(typeof(string), "Dependency 3"));

            var sut = new FactoryProvider(registration, factory, new Dictionary<IDependency, IProvider>
            {
                { new Dependency(typeof(string), "Dependency 1"), p1},
                { new Dependency(typeof(string), "Dependency 2"), p2},
                { new Dependency(typeof(string), "Dependency 3"), p3},
            });

            sut.FulfilledContracts.Should().BeEquivalentTo(FulfilledContract, typeof(string));
            sut.ResultType.Should().Be(typeof(string).GetTypeInfo());
            sut.UnresolvedDependencies.Should().BeEmpty();

            var result = sut.CreateInstance(new Dictionary<IDependency, object>());
            result.Should().Be(factoryResult);

            p1.InvocationCounter.Should().Be(1);
            p2.InvocationCounter.Should().Be(1);
            p3.InvocationCounter.Should().Be(1);
        }

        [TestMethod]
        public void UnfulfilledDependencies()
        {
            var registration = new ManualRegistration
            {
                RegisteredType = typeof(string),
                FulfilledContracts = { FulfilledContract }
            };

            var factoryResult = Guid.NewGuid().ToString();

            var p1 = new TestProvider(_ => "Provider result 1");
            var p2 = new TestProvider(_ => "Provider result 2");

            var factory = new Factory(deps =>
                {
                    deps.Should().BeEquivalentTo(new object[]{"Provider result 1", "Provider result 2", "Passed value"});
                    return factoryResult;
                },
                new Dependency(typeof(string), "Dependency 1"),
                new Dependency(typeof(string), "Dependency 2"),
                new Dependency(typeof(string), "Dependency 3"));

            var sut = new FactoryProvider(registration, factory, new Dictionary<IDependency, IProvider>
            {
                { new Dependency(typeof(string), "Dependency 1"), p1},
                { new Dependency(typeof(string), "Dependency 2"), p2},
            });

            sut.FulfilledContracts.Should().BeEquivalentTo(FulfilledContract, typeof(string));
            sut.ResultType.Should().Be(typeof(string).GetTypeInfo());
            sut.UnresolvedDependencies.Count.Should().Be(1);
            Dependency.EqualityComparer.Instance.Equals(sut.UnresolvedDependencies.Select(x => x).First(),
                new Dependency(typeof(string), "Dependency 3")).Should().BeTrue();

            var result = sut.CreateInstance(new Dictionary<IDependency, object>
            {
                {new Dependency(typeof(string), "Dependency 3"),  "Passed value"}
            });
            result.Should().Be(factoryResult);

            p1.InvocationCounter.Should().Be(1);
            p2.InvocationCounter.Should().Be(1);
        }
    }
}