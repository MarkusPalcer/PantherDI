using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PantherDI.Comparers;
using PantherDI.Registry.Registration;
using PantherDI.Resolved.Providers;
using PantherDI.Resolvers;
using PantherDI.Tests.Helpers;

namespace PantherDI.Tests.Resolvers
{
    [TestClass]
    public class Func0ResolverTests
    {
        [TestMethod]
        public void NoRegistration()
        {
            var sut = new Func0Resolver();
            sut.Resolve(_ => Enumerable.Empty<IProvider>(), new Dependency(typeof(Func<string>))).Should().BeEmpty();
        }

        [TestMethod]
        public void WrongRequest()
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

            var sut = new Func0Resolver();
            var result = sut.Resolve(_ => registrations, new Dependency(typeof(SetComparer<string>))).ToArray();

            result.Should().BeEmpty();
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

            var sut = new Func0Resolver();
            var result = sut.Resolve(_ => registrations, new Dependency(typeof(Func<string>))).ToArray();

            result.Should().HaveCount(1);
            result[0].FulfilledContracts.Should().BeEquivalentTo(typeof(Func<string>));
            result[0].UnresolvedDependencies.Should().BeEmpty();
            result[0].Singleton.Should().BeFalse();
            result[0].ResultType.Should().Be(typeof(Func<string>));

            registrations[0].InvocationCounter.Should().Be(0);
            var instance = result[0].CreateInstance(new Dictionary<Dependency, object>());
            instance.Should().BeAssignableTo<Func<string>>();
            registrations[0].InvocationCounter.Should().Be(0);
            instance.As<Func<string>>()().Should().Be("1");
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

            var sut = new Func0Resolver();
            var result = sut.Resolve(_ => registrations, new Dependency(typeof(Func<string>))).ToArray();

            result.Should().HaveCount(2);
            var i = 0;
            foreach (var provider in result)
            {
                provider.FulfilledContracts.Should().BeEquivalentTo(new[] { typeof(Func<string>) }, $"result[{i}] should have fulfilled contracts just as its source provider");
                provider.UnresolvedDependencies.Should().BeEmpty($"result[{i}]");
                provider.Singleton.Should().BeFalse($"result[{i}]");
                provider.ResultType.Should().Be(typeof(Func<string>), $"result[{i}]");
                i++;
            }

            registrations.Should().OnlyContain(x => x.InvocationCounter == 0);
            var instances = result.Select(x => x.CreateInstance(new Dictionary<Dependency, object>())).ToArray();
            instances.Should().AllBeAssignableTo<Func<string>>();
            registrations.Should().OnlyContain(x => x.InvocationCounter == 0);
            instances.Cast<Func<string>>().Select(x => x()).Should().BeEquivalentTo("1", "2");
            registrations.Should().OnlyContain(x => x.InvocationCounter == 1);
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

            var sut = new Func0Resolver();
            var result = sut.Resolve(_ => registrations, new Dependency(typeof(Func<string>))).ToArray();

            registrations.Should().NotContain(x => x.InvocationCounter != 0);

            result.Should().HaveCount(4);
            var i = 0;
            foreach (var provider in result)
            {
                provider.FulfilledContracts.Should().BeEquivalentTo(new[] { typeof(Func<string>) }, $"result[{i}] should have fulfilled contracts just as its source provider");
                provider.UnresolvedDependencies.Should().BeEquivalentTo(registrations[i].UnresolvedDependencies, $"result[{i}]");
                provider.Singleton.Should().BeFalse($"result[{i}]");
                provider.ResultType.Should().Be(typeof(Func<string>), $"result[{i}]");
                i++;
            }


            var instances = result.Select(x => x.CreateInstance(new Dictionary<Dependency, object>())).ToArray();

            instances.Should().AllBeAssignableTo<Func<string>>();
            registrations.Should().OnlyContain(x => x.InvocationCounter == 0);
            instances.Cast<Func<string>>().Select(x => x()).Should().BeEquivalentTo("1", "2", "3", "4");
            registrations.Should().OnlyContain(x => x.InvocationCounter == 1);
        }
    }
}