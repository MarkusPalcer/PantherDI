﻿using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PantherDI.Registry.Registration.Dependency;
using PantherDI.Resolved.Providers;
using PantherDI.Resolvers;
using PantherDI.Tests.Helpers;

namespace PantherDI.Tests.Resolvers
{
    [TestClass]
    public class EnumerableResolverTests
    {
        [TestMethod]
        public void NoRegistration()
        {
            var sut = new EnumerableResolver();
            sut.Resolve(_ => Enumerable.Empty<IProvider>(), new Dependency(typeof(IEnumerable<string>))).Should().BeEmpty();
        }

        [TestMethod]
        public void SingleRegistration()
        {
            var registrations = new[]{
                new TestProvider("1")
                {
                    ResultType = typeof(string),
                    FulfilledContracts =
                    {
                        typeof(string)
                    }
                }
            };

            var sut = new EnumerableResolver();
            var result = sut.Resolve(_ => registrations, new Dependency(typeof(IEnumerable<string>))).ToArray();

            result.Should().HaveCount(1);
            result[0].FulfilledContracts.Should().BeEquivalentTo(typeof(IEnumerable<string>));
            result[0].UnresolvedDependencies.Should().BeEmpty();
            result[0].Singleton.Should().BeFalse();
            result[0].ResultType.Should().Be(typeof(IEnumerable<string>));

            registrations[0].InvocationCounter.Should().Be(0);
            var instance = result[0].CreateInstance(new Dictionary<IDependency, object>());
            instance.Should().BeAssignableTo<IEnumerable<string>>();
            instance.As<IEnumerable<string>>().Should().HaveCount(1);
            instance.As<IEnumerable<string>>().Should().BeEquivalentTo("1");
            registrations[0].InvocationCounter.Should().Be(1);
        }

        [TestMethod]
        public void MultipleRegistrationsNoDependencies()
        {

            var registrations = new[]{
                new TestProvider("1")
                {
                    ResultType = typeof(string),
                    FulfilledContracts =
                    {
                        typeof(string)
                    }
                },
                new TestProvider("2")
                {
                    ResultType = typeof(string),
                    FulfilledContracts =
                    {
                        typeof(string)
                    }
                },
            };

            var sut = new EnumerableResolver();
            var result = sut.Resolve(_ => registrations, new Dependency(typeof(IEnumerable<string>))).ToArray();

            result.Should().HaveCount(1);
            result[0].FulfilledContracts.Should().BeEquivalentTo(typeof(IEnumerable<string>));
            result[0].UnresolvedDependencies.Should().BeEmpty();
            result[0].Singleton.Should().BeFalse();
            result[0].ResultType.Should().Be(typeof(IEnumerable<string>));
            
            registrations[0].InvocationCounter.Should().Be(0);
            var instance = result[0].CreateInstance(new Dictionary<IDependency, object>());
            instance.Should().BeAssignableTo<IEnumerable<string>>();
            instance.As<IEnumerable<string>>().Should().HaveCount(2);
            instance.As<IEnumerable<string>>().Should().BeEquivalentTo("1", "2");
            registrations[0].InvocationCounter.Should().Be(1);
            registrations[1].InvocationCounter.Should().Be(1);
        }

        [TestMethod]
        public void MultipleRegistrationsMultipleDependencies()
        {
            var registrations = new[]{
                new TestProvider("1")
                {
                    ResultType = typeof(string),
                    FulfilledContracts =
                    {
                        typeof(string)
                    }
                },
                new TestProvider("2")
                {
                    ResultType = typeof(string),
                    FulfilledContracts =
                    {
                        typeof(string)
                    }
                },
                new TestProvider("3")
                {
                    ResultType = typeof(string),
                    FulfilledContracts =
                    {
                        typeof(string)
                    }, 
                    UnresolvedDependencies =
                    {
                        new Dependency(typeof(IContainer))
                    }
                },
                new TestProvider("4")
                {
                    ResultType = typeof(string),
                    FulfilledContracts =
                    {
                        typeof(string)
                    },
                    UnresolvedDependencies =
                    {
                        new Dependency(typeof(IContainer))
                    }
                },
            };

            var sut = new EnumerableResolver();
            var result = sut.Resolve(_ => registrations, new Dependency(typeof(IEnumerable<string>))).ToArray();

            registrations.Should().NotContain(x => x.InvocationCounter != 0);

            

            result.Should().HaveCount(2);
            result[0].FulfilledContracts.Should().BeEquivalentTo(typeof(IEnumerable<string>));
            result[0].UnresolvedDependencies.Should().BeEmpty();
            result[0].Singleton.Should().BeFalse();
            result[0].ResultType.Should().Be(typeof(IEnumerable<string>));

            var instance = result[0].CreateInstance(new Dictionary<IDependency, object>());
            instance.Should().BeAssignableTo<IEnumerable<string>>();
            instance.As<IEnumerable<string>>().Should().HaveCount(2);
            instance.As<IEnumerable<string>>().Should().BeEquivalentTo("1", "2");

            result[1].FulfilledContracts.Should().BeEquivalentTo(typeof(IEnumerable<string>));
            result[1].UnresolvedDependencies.Should().HaveCount(1);
            new Dependency.EqualityComparer().Equals(result[1].UnresolvedDependencies.ElementAt(0), new Dependency(typeof(IContainer))).Should().BeTrue();
            result[1].Singleton.Should().BeFalse();
            result[1].ResultType.Should().Be(typeof(IEnumerable<string>));
            instance = result[1].CreateInstance(new Dictionary<IDependency, object>());
            instance.As<IEnumerable<string>>().Should().HaveCount(2);
            instance.As<IEnumerable<string>>().Should().BeEquivalentTo("3", "4");

            registrations.Should().NotContain(x => x.InvocationCounter != 1);
        }
    }
}