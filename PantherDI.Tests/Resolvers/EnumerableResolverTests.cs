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
            var registrations = new[]
            {
                new TestProvider("1")
                {
                    ResultType = typeof(string),
                    FulfilledContracts =
                    {
                        typeof(string)
                    }
                }
            };

            var dependencyResolver = new DependencyResolverDictionary()
            {
                {new Dependency(typeof(String)), registrations}
            };

            var sut = new EnumerableResolver();
            var result = sut.Resolve(dependencyResolver.Execute, new Dependency(typeof(IEnumerable<string>))).ToArray();

            result.Should().HaveCount(1);
            result[0].FulfilledContracts.Should().BeEquivalentTo(typeof(IEnumerable<string>));
            result[0].UnresolvedDependencies.Should().BeEmpty();
            result[0].Singleton.Should().BeFalse();
            result[0].ResultType.Should().Be(typeof(IEnumerable<string>));
            result[0].Priority.Should().Be(0);

            registrations[0].InvocationCounter.Should().Be(0);
            var instance = result[0].CreateInstance(new Dictionary<Dependency, object>());
            instance.Should().BeAssignableTo<IEnumerable<string>>();
            instance.As<IEnumerable<string>>().Should().HaveCount(1);
            instance.As<IEnumerable<string>>().Should().BeEquivalentTo("1");
            registrations[0].InvocationCounter.Should().Be(1);
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

            var dependencyResolver = new DependencyResolverDictionary()
            {
                {new Dependency(typeof(String)), registrations}
            };

            var sut = new EnumerableResolver();
            var result = sut.Resolve(dependencyResolver.Execute, new Dependency(typeof(SetComparer<string>))).ToArray();

            result.Should().BeEmpty();
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

            var dependencyResolver = new DependencyResolverDictionary()
            {
                {new Dependency(typeof(String)), registrations}
            };

            var sut = new EnumerableResolver();
            var result = sut.Resolve(dependencyResolver.Execute, new Dependency(typeof(IEnumerable<string>))).ToArray();

            result.Should().HaveCount(1);
            result[0].FulfilledContracts.Should().BeEquivalentTo(typeof(IEnumerable<string>));
            result[0].UnresolvedDependencies.Should().BeEmpty();
            result[0].Singleton.Should().BeFalse();
            result[0].ResultType.Should().Be(typeof(IEnumerable<string>));
            result[0].Priority.Should().Be(0);
            
            registrations[0].InvocationCounter.Should().Be(0);
            var instance = result[0].CreateInstance(new Dictionary<Dependency, object>());
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

            var dependencyResolver = new DependencyResolverDictionary()
            {
                {new Dependency(typeof(String)), registrations}
            };

            var sut = new EnumerableResolver();
            var result = sut.Resolve(dependencyResolver.Execute, new Dependency(typeof(IEnumerable<string>))).ToArray();

            registrations.Should().NotContain(x => x.InvocationCounter != 0);

            

            result.Should().HaveCount(2);
            result[0].FulfilledContracts.Should().BeEquivalentTo(typeof(IEnumerable<string>));
            result[0].UnresolvedDependencies.Should().BeEmpty();
            result[0].Singleton.Should().BeFalse();
            result[0].ResultType.Should().Be(typeof(IEnumerable<string>));
            result[0].Priority.Should().Be(0);

            var instance = result[0].CreateInstance(new Dictionary<Dependency, object>());
            instance.Should().BeAssignableTo<IEnumerable<string>>();
            instance.As<IEnumerable<string>>().Should().HaveCount(2);
            instance.As<IEnumerable<string>>().Should().BeEquivalentTo("1", "2");

            result[1].FulfilledContracts.Should().BeEquivalentTo(typeof(IEnumerable<string>));
            result[1].UnresolvedDependencies.Should().HaveCount(1);
            result[1].UnresolvedDependencies.ElementAt(0).Should().Be(new Dependency(typeof(IContainer)));
            result[1].Singleton.Should().BeFalse();
            result[1].ResultType.Should().Be(typeof(IEnumerable<string>));
            result[1].Priority.Should().Be(0);
            instance = result[1].CreateInstance(new Dictionary<Dependency, object>());
            instance.As<IEnumerable<string>>().Should().HaveCount(2);
            instance.As<IEnumerable<string>>().Should().BeEquivalentTo("3", "4");

            registrations.Should().NotContain(x => x.InvocationCounter != 1);

        }

        [TestMethod]
        public void MultipleRegistrationsMultiplePriorities()
        {
            var registrations = new[]{
                new TestProvider("1")
                {
                    ResultType = typeof(string),
                    FulfilledContracts =
                    {
                        typeof(string)
                    },
                    Priority = 0
                },
                new TestProvider("2")
                {
                    ResultType = typeof(string),
                    FulfilledContracts =
                    {
                        typeof(string)
                    },
                    Priority = 0
                },
                new TestProvider("3")
                {
                    ResultType = typeof(string),
                    FulfilledContracts =
                    {
                        typeof(string)
                    },
                    Priority = 1
                },
                new TestProvider("4")
                {
                    ResultType = typeof(string),
                    FulfilledContracts =
                    {
                        typeof(string)
                    },
                    Priority = 1
                },
            };

            var dependencyResolver = new DependencyResolverDictionary()
            {
                {new Dependency(typeof(String)), registrations}
            };

            var sut = new EnumerableResolver();
            var result = sut.Resolve(dependencyResolver.Execute, new Dependency(typeof(IEnumerable<string>))).ToArray();

            registrations.Should().NotContain(x => x.InvocationCounter != 0);
            

            result.Should().HaveCount(2);
            result[0].FulfilledContracts.Should().BeEquivalentTo(typeof(IEnumerable<string>));
            result[0].UnresolvedDependencies.Should().BeEmpty();
            result[0].Singleton.Should().BeFalse();
            result[0].ResultType.Should().Be(typeof(IEnumerable<string>));
            result[0].Priority.Should().Be(0);

            var instance = result[0].CreateInstance(new Dictionary<Dependency, object>());
            instance.Should().BeAssignableTo<IEnumerable<string>>();
            instance.As<IEnumerable<string>>().Should().HaveCount(2);
            instance.As<IEnumerable<string>>().Should().BeEquivalentTo("1", "2");

            result[1].FulfilledContracts.Should().BeEquivalentTo(typeof(IEnumerable<string>));
            result[1].UnresolvedDependencies.Should().BeEmpty();
            result[1].Singleton.Should().BeFalse();
            result[1].ResultType.Should().Be(typeof(IEnumerable<string>));
            result[1].Priority.Should().Be(1);
            instance = result[1].CreateInstance(new Dictionary<Dependency, object>());
            instance.As<IEnumerable<string>>().Should().HaveCount(2);
            instance.As<IEnumerable<string>>().Should().BeEquivalentTo("3", "4");

            registrations.Should().NotContain(x => x.InvocationCounter != 1);
        }
    }
}