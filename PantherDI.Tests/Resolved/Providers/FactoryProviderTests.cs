﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PantherDI.Registry.Registration;
using PantherDI.Registry.Registration.Factory;
using PantherDI.Registry.Registration.Registration;
using PantherDI.Resolved.Providers;
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

            var factory = new DelegateFactory(deps =>
            {
                deps.Should().BeEmpty();
                return factoryResult;
            }, Enumerable.Empty<object>(), Enumerable.Empty<Dependency>());

            var sut = new FactoryProvider(registration, factory, new Dictionary<Dependency, IProvider>());

            sut.FulfilledContracts.Should().BeEquivalentTo(FulfilledContract, typeof(string));
            sut.ResultType.Should().Be(typeof(string).GetTypeInfo());
            sut.UnresolvedDependencies.Should().BeEmpty();

            var result = sut.CreateInstance(new Dictionary<Dependency, object>());
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

            var factory = new DelegateFactory(deps =>
            {
                deps.Should().BeEquivalentTo(new object[]{"Provider result 1", "Provider result 2", "Provider result 3"});
                return factoryResult;
            }, 
            Enumerable.Empty<object>(),
            new[] {
            new Dependency(typeof(string), "Dependency 1"), 
            new Dependency(typeof(string), "Dependency 2"), 
            new Dependency(typeof(string), "Dependency 3")});

            var sut = new FactoryProvider(registration, factory, new Dictionary<Dependency, IProvider>
            {
                { new Dependency(typeof(string), "Dependency 1"), p1},
                { new Dependency(typeof(string), "Dependency 2"), p2},
                { new Dependency(typeof(string), "Dependency 3"), p3},
            });

            sut.FulfilledContracts.Should().BeEquivalentTo(FulfilledContract, typeof(string));
            sut.ResultType.Should().Be(typeof(string).GetTypeInfo());
            sut.UnresolvedDependencies.Should().BeEmpty();

            var result = sut.CreateInstance(new Dictionary<Dependency, object>());
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

            var factory = new DelegateFactory(deps =>
                                              {
                                                  deps.Should().BeEquivalentTo(new object[] {"Provider result 1", "Provider result 2", "Passed value"});
                                                  return factoryResult;
                                              },
                                              Enumerable.Empty<object>(),
                                              new[]
                                              {
                                                  new Dependency(typeof(string), "Dependency 1"),
                                                  new Dependency(typeof(string), "Dependency 2"),
                                                  new Dependency(typeof(string), "Dependency 3")
                                              });


            var sut = new FactoryProvider(registration, factory, new Dictionary<Dependency, IProvider>
            {
                { new Dependency(typeof(string), "Dependency 1"), p1},
                { new Dependency(typeof(string), "Dependency 2"), p2},
            });

            sut.FulfilledContracts.Should().BeEquivalentTo(FulfilledContract, typeof(string));
            sut.ResultType.Should().Be(typeof(string).GetTypeInfo());
            sut.UnresolvedDependencies.Count.Should().Be(1);
            sut.UnresolvedDependencies.Select(x => x).First().Should().Be(new Dependency(typeof(string), "Dependency 3"));

            var result = sut.CreateInstance(new Dictionary<Dependency, object>
            {
                {new Dependency(typeof(string), "Dependency 3"),  "Passed value"}
            });
            result.Should().Be(factoryResult);

            p1.InvocationCounter.Should().Be(1);
            p2.InvocationCounter.Should().Be(1);
        }
    }
}