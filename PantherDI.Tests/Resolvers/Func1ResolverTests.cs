using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PantherDI.Comparers;
using PantherDI.Registry.Catalog;
using PantherDI.Registry.Registration;
using PantherDI.Resolved.Providers;
using PantherDI.Resolvers;
using PantherDI.Tests.Helpers;

namespace PantherDI.Tests.Resolvers
{
    // There are only tests for Func1Resolver, since
    // the same generic code is used for the other Func#Resolver classes.
    [TestClass]
    public class Func1ResolverTests
    {
        [TestMethod]
        public void NoRegistration()
        {
            var sut = new Func1Resolver();
            sut.Resolve(_ => Enumerable.Empty<IProvider>(), new Dependency(typeof(Func<Dependency, string>))).Should().BeEmpty();
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

            var sut = new Func1Resolver();
            var result = sut.Resolve(_ => registrations, new Dependency(typeof(SetComparer<string>))).ToArray();

            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WrongRegistration()
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

            var sut = new Func1Resolver();
            var result = sut.Resolve(_ => registrations, new Dependency(typeof(Func<Dependency, string>))).ToArray();

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
                    },
                    UnresolvedDependencies =
                    {
                        new Dependency(typeof(Dependency))
                    }
                }
            };

            var sut = new Func1Resolver();
            var result = sut.Resolve(_ => registrations, new Dependency(typeof(Func<Dependency, string>))).ToArray();

            result.Should().HaveCount(1);
            result[0].FulfilledContracts.Should().BeEquivalentTo(typeof(Func<Dependency, string>));
            result[0].UnresolvedDependencies.Should().BeEmpty();
            result[0].Singleton.Should().BeFalse();
            result[0].ResultType.Should().Be(typeof(Func<Dependency, string>));

            registrations[0].InvocationCounter.Should().Be(0);
            var instance = result[0].CreateInstance(new Dictionary<Dependency, object>());
            instance.Should().BeAssignableTo<Func<Dependency, string>>();
            registrations[0].InvocationCounter.Should().Be(0);
            instance.As<Func<Dependency, string>>()(new Dependency(typeof(string))).Should().Be("1");
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

            var sut = new Func1Resolver();
            var result = sut.Resolve(_ => registrations, new Dependency(typeof(Func<Dependency, string>))).ToArray();

            result.Should().BeEmpty();
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
                        new Dependency(typeof(Dependency))
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
                        new Dependency(typeof(Dependency)),
                        new Dependency(typeof(ICatalog))
                    }
                },
                new TestProvider("5")
                {
                    ResultType = typeof(string),
                    FulfilledContracts =
                    {
                        typeof(string)
                    },
                    UnresolvedDependencies =
                    {
                        new Dependency(typeof(ICatalog))
                    }
                },
            };

            var sut = new Func1Resolver();
            var result = sut.Resolve(_ => registrations, new Dependency(typeof(Func<Dependency, string>))).ToArray();

            registrations.Should().NotContain(x => x.InvocationCounter != 0);

            result.Should().HaveCount(2);
            var i = 0;
            foreach (var provider in result)
            {
                provider.FulfilledContracts.Should().BeEquivalentTo(new[] { typeof(Func<Dependency, string>) }, $"result[{i}] should have fulfilled contracts just as its source provider");
                provider.Singleton.Should().BeFalse($"result[{i}]");
                provider.ResultType.Should().Be(typeof(Func<Dependency, string>), $"result[{i}]");
                i++;
            }

            result[0].UnresolvedDependencies.Should().BeEmpty();
            result[1].UnresolvedDependencies.Should().HaveCount(1);
            result[1].UnresolvedDependencies.First().ExpectedType.Should().Be(typeof(ICatalog));
            result[1].UnresolvedDependencies.First().RequiredContracts.Should().BeEquivalentTo(typeof(ICatalog));

            var instances = result.Select(x => x.CreateInstance(new Dictionary<Dependency, object>())).ToArray();

            instances.Should().AllBeAssignableTo<Func<Dependency, string>>();
            registrations.Should().OnlyContain(x => x.InvocationCounter == 0);
            instances.Cast<Func<Dependency, string>>().Select(x => x(new Dependency(typeof(string)))).Should().BeEquivalentTo("3", "4");
            registrations.Select(x => x.InvocationCounter).Should().Equal(0,0,1,1,0);
        }
    }
}