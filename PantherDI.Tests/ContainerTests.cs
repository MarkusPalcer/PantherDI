using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PantherDI.ContainerCreation;
using PantherDI.Exceptions;
using PantherDI.Registry.Registration;
using PantherDI.Resolved;
using PantherDI.Resolved.Providers;
using PantherDI.Resolvers;
using PantherDI.Resolvers.Aggregation;
using PantherDI.Tests.Helpers;

namespace PantherDI.Tests
{
    [TestClass]
    public class ContainerTests
    {
        private const string ProviderResult = "Provider Result";

        [TestMethod]
        public void SucceedOnRegistration()
        {
            var kb = new Mock<IResolver>(MockBehavior.Strict);
            var provider = new TestProvider(ProviderResult)
            {
                FulfilledContracts = {typeof(string)},
                ResultType = typeof(string).GetTypeInfo()
            };

            kb
                .Setup(x => x.Resolve(
                    It.IsAny<Func<Dependency, IEnumerable<IProvider>>>(),
                    It.Is<Dependency>(d => d.Equals(new Dependency(typeof(string))))))
                .Returns(new IProvider[]{provider});

            var sut = new Container(kb.Object);

            sut.Resolve<string>().Should().Be(ProviderResult);
        }

        [TestMethod]
        public void FailOnNoRegistration()
        {
            var kb = new Mock<IKnowledgeBase>(MockBehavior.Strict);

            kb
                .Setup(x => x.Resolve(
                    It.IsAny<Func<Dependency, IEnumerable<IProvider>>>(),
                    It.Is<Dependency>(d => d.Equals(new Dependency(typeof(string))))))
                .Returns(Enumerable.Empty<IProvider>());

            var sut = new Container(kb.Object);

            sut.Invoking(x => x.Resolve<string>()).ShouldThrow<NoSuitableRegistrationException>();
        }

        [TestMethod]
        public void FailOnNoParameterlessRegistration()
        {
            var kb = new Mock<IKnowledgeBase>(MockBehavior.Strict);
            var provider = new TestProvider(ProviderResult)
            {
                FulfilledContracts = { typeof(string) },
                ResultType = typeof(string).GetTypeInfo(),
                UnresolvedDependencies = { new Dependency(typeof(IProvider)) }
            };

            kb
                .Setup(x => x.Resolve(
                    It.IsAny<Func<Dependency, IEnumerable<IProvider>>>(),
                    It.Is<Dependency>(d => d.Equals(new Dependency(typeof(string))))))
                .Returns(new IProvider[] { provider });

            var sut = new Container(kb.Object);

            sut.Invoking(x => x.Resolve<string>()).ShouldThrow<NoSuitableRegistrationException>();

            provider.InvocationCounter.Should().Be(0);
        }

        [TestMethod]
        public void SelectProviderWithoutParameters()
        {
            var kb = new Mock<IKnowledgeBase>(MockBehavior.Strict);
            var provider1 = new TestProvider(ProviderResult + "1")
            {
                FulfilledContracts = { typeof(string) },
                ResultType = typeof(string).GetTypeInfo()
            };
            var provider2 = new TestProvider(ProviderResult + "2")
            {
                FulfilledContracts = { typeof(string) },
                ResultType = typeof(string).GetTypeInfo(),
                UnresolvedDependencies = {new Dependency(typeof(IProvider))}
            };


            kb
                .Setup(x => x.Resolve(
                    It.IsAny<Func<Dependency, IEnumerable<IProvider>>>(),
                    It.Is<Dependency>(d => d.Equals(new Dependency(typeof(string))))))
                .Returns(new IProvider[] { provider1, provider2 });

            var sut = new Container(kb.Object);

            sut.Resolve<string>().Should().Be(ProviderResult + "1");
            provider1.InvocationCounter.Should().Be(1);
            provider2.InvocationCounter.Should().Be(0);
        }

        [TestMethod]
        public void MultipleRegistrationsFail()
        {
            var kb = new Mock<IKnowledgeBase>(MockBehavior.Strict);
            var provider1 = new TestProvider(ProviderResult + "1")
            {
                FulfilledContracts = { typeof(string) },
                ResultType = typeof(string).GetTypeInfo()
            };
            var provider2 = new TestProvider(ProviderResult + "2")
            {
                FulfilledContracts = { typeof(string) },
                ResultType = typeof(string).GetTypeInfo(),
            };
            
            kb
                .Setup(x => x.Resolve(
                    It.IsAny<Func<Dependency, IEnumerable<IProvider>>>(),
                    It.Is<Dependency>(d => d.Equals(new Dependency(typeof(string))))))
                .Returns(new IProvider[] { provider1, provider2 });

            var sut = new Container(kb.Object);

            sut.Invoking(x => x.Resolve<string>()).ShouldThrow<TooManySuitableRegistrationsException>();
            provider1.InvocationCounter.Should().Be(0);
            provider2.InvocationCounter.Should().Be(0);
        }

        [TestMethod]
        public void UnknownRegistrationsAreCached()
        {
            var kb = new Mock<IKnowledgeBase>(MockBehavior.Strict);
            var resolver2 = new Mock<IResolver>(MockBehavior.Strict);
            var provider = new TestProvider(ProviderResult)
            {
                FulfilledContracts = { typeof(string) },
                ResultType = typeof(string).GetTypeInfo()
            };

            kb
                .Setup(x => x.Resolve(
                    It.IsAny<Func<Dependency, IEnumerable<IProvider>>>(),
                    It.Is<Dependency>(d => d.Equals(new Dependency(typeof(string))))))
                .Returns(Enumerable.Empty<IProvider>());

            resolver2
                .Setup(x => x.Resolve(
                    It.IsAny<Func<Dependency, IEnumerable<IProvider>>>(),
                    It.Is<Dependency>(d => d.Equals(new Dependency(typeof(string))))))
                .Returns(new IProvider[] { provider });

            kb.Setup(x => x.Add(provider));

            var sut = new Container(new FirstMatchResolver(new[] { kb.Object, resolver2.Object }));

            sut.Resolve<string>().Should().Be(ProviderResult);
        }

        [TestMethod]
        public void Singletons()
        {
            var kb = new Mock<IKnowledgeBase>(MockBehavior.Strict);
            var provider = new TestProvider(ProviderResult)
            {
                FulfilledContracts = { typeof(string) },
                ResultType = typeof(string).GetTypeInfo(),
                Singleton = true
            };

            kb
                .Setup(x => x.Resolve(
                    It.IsAny<Func<Dependency, IEnumerable<IProvider>>>(),
                    It.Is<Dependency>(d => d.Equals(new Dependency(typeof(string))))))
                .Returns(new IProvider[] { provider });

            var sut = new Container(kb.Object);

            sut.Resolve<string>().Should().Be(ProviderResult);
            sut.Resolve<string>().Should().Be(ProviderResult);

            provider.InvocationCounter.Should().Be(1);
        }

        [TestMethod]
        public void DisposingSingletons()
        {
            var disposableMock = new Mock<IDisposable>(MockBehavior.Strict);
            var kb = new Mock<IKnowledgeBase>(MockBehavior.Strict);
            var provider = new TestProvider(disposableMock.Object)
            {
                FulfilledContracts = { typeof(IDisposable) },
                ResultType = typeof(IDisposable).GetTypeInfo(),
                Singleton = true
            };

            kb
                .Setup(x => x.Resolve(
                    It.IsAny<Func<Dependency, IEnumerable<IProvider>>>(),
                    It.Is<Dependency>(d => d.Equals(new Dependency(typeof(IDisposable))))))
                .Returns(new IProvider[] { provider });

            var sut = new Container(kb.Object);

            sut.Resolve<IDisposable>();

            disposableMock.Setup(x => x.Dispose());

            sut.Dispose();

            disposableMock.Verify(x => x.Dispose(), Times.Once);
        }

        [TestMethod]
        public void NotDisposingNonSingletons()
        {
            var disposableMock = new Mock<IDisposable>(MockBehavior.Strict);
            var kb = new Mock<IKnowledgeBase>(MockBehavior.Strict);
            var provider = new TestProvider(disposableMock.Object)
            {
                FulfilledContracts = { typeof(IDisposable) },
                ResultType = typeof(IDisposable).GetTypeInfo(),
                Singleton = false
            };

            kb
                .Setup(x => x.Resolve(
                    It.IsAny<Func<Dependency, IEnumerable<IProvider>>>(),
                    It.Is<Dependency>(d => d.Equals(new Dependency(typeof(IDisposable))))))
                .Returns(new IProvider[] { provider });

            var sut = new Container(kb.Object);

            sut.Resolve<IDisposable>();

            sut.Dispose();
        }

        private class TestType1 { }

        private class TestType2
        {
            private TestType1 dependency;

            public TestType2(TestType1 dependency)
            {
                this.dependency = dependency;
            }
        }

        private class TestType3
        {
            private TestType2 dependency;

            public TestType3(TestType2 dependency)
            {
                this.dependency = dependency;
            }
        }

        [TestMethod]
        public void ContainerAsCatalog()
        {
            var sut = new ContainerBuilder()
                      .WithType<TestType2>()
                      .WithType<TestType3>()
                      .Build()
                      .AsCatalog();

            // Expecting 3 registrations: The registered type and the container itself
            sut.Registrations.ToArray().Should().HaveCount(3);
            
            var registration = sut.Registrations.Single(x => x.RegisteredType == typeof(TestType3));
            registration.FulfilledContracts.Concat(registration.Factories.Single().FulfilledContracts).Should().BeEquivalentTo(typeof(TestType3));
            registration.Singleton.Should().BeFalse();
            registration.Factories.Single().Dependencies.Should().BeEquivalentTo(new Dependency(typeof(TestType1)));

            registration = sut.Registrations.Single(x => x.RegisteredType == typeof(TestType2));
            registration.FulfilledContracts.Concat(registration.Factories.Single().FulfilledContracts).Should().BeEquivalentTo(typeof(TestType2));
            registration.Singleton.Should().BeFalse();
            registration.Factories.Single().Dependencies.Should().BeEquivalentTo(new Dependency(typeof(TestType1)));
        }
    }
}