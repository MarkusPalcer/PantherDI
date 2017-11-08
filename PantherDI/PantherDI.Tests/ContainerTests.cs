using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PantherDI.Exceptions;
using PantherDI.Registry.Registration.Dependency;
using PantherDI.Resolved;
using PantherDI.Resolvers;
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
            var kb = new Mock<IKnowledgeBase>(MockBehavior.Strict);
            var provider = new TestProvider(ProviderResult)
            {
                FulfilledContracts = {typeof(string)},
                ResultType = typeof(string).GetTypeInfo()
            };

            kb
                .Setup(x => x.Resolve(
                    It.IsAny<Func<IDependency, IEnumerable<IProvider>>>(),
                    It.Is<IDependency>(d => Dependency.EqualityComparer.Instance.Equals(d, new Dependency(typeof(string))))))
                .Returns(new IProvider[]{provider});

            var sut = new Container(kb.Object, Enumerable.Empty<IResolver>());

            sut.Resolve<string>().Should().Be(ProviderResult);
        }

        [TestMethod]
        public void FailOnNoRegistration()
        {
            var kb = new Mock<IKnowledgeBase>(MockBehavior.Strict);

            kb
                .Setup(x => x.Resolve(
                    It.IsAny<Func<IDependency, IEnumerable<IProvider>>>(),
                    It.Is<IDependency>(d => Dependency.EqualityComparer.Instance.Equals(d, new Dependency(typeof(string))))))
                .Returns(Enumerable.Empty<IProvider>());

            var sut = new Container(kb.Object, Enumerable.Empty<IResolver>());

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
                    It.IsAny<Func<IDependency, IEnumerable<IProvider>>>(),
                    It.Is<IDependency>(d => Dependency.EqualityComparer.Instance.Equals(d, new Dependency(typeof(string))))))
                .Returns(new IProvider[] { provider });

            var sut = new Container(kb.Object, Enumerable.Empty<IResolver>());

            sut.Invoking(x => x.Resolve<string>()).ShouldThrow<NoSuitableRegistrationException>();
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
                    It.IsAny<Func<IDependency, IEnumerable<IProvider>>>(),
                    It.Is<IDependency>(d => Dependency.EqualityComparer.Instance.Equals(d, new Dependency(typeof(string))))))
                .Returns(new IProvider[] { provider1, provider2 });

            var sut = new Container(kb.Object, Enumerable.Empty<IResolver>());

            sut.Resolve<string>().Should().Be(ProviderResult + "1");
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
                    It.IsAny<Func<IDependency, IEnumerable<IProvider>>>(),
                    It.Is<IDependency>(d => Dependency.EqualityComparer.Instance.Equals(d, new Dependency(typeof(string))))))
                .Returns(new IProvider[] { provider1, provider2 });

            var sut = new Container(kb.Object, Enumerable.Empty<IResolver>());

            sut.Invoking(x => x.Resolve<string>()).ShouldThrow<TooManySuitableRegistrationsException>();
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
                    It.IsAny<Func<IDependency, IEnumerable<IProvider>>>(),
                    It.Is<IDependency>(d => Dependency.EqualityComparer.Instance.Equals(d, new Dependency(typeof(string))))))
                .Returns(Enumerable.Empty<IProvider>());

            resolver2
                .Setup(x => x.Resolve(
                    It.IsAny<Func<IDependency, IEnumerable<IProvider>>>(),
                    It.Is<IDependency>(d => Dependency.EqualityComparer.Instance.Equals(d, new Dependency(typeof(string))))))
                .Returns(new IProvider[] { provider });

            kb.Setup(x => x.Add(provider));

            var sut = new Container(kb.Object, new[] {resolver2.Object });

            sut.Resolve<string>().Should().Be(ProviderResult);
        }
    }
}