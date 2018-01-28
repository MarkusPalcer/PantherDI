using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PantherDI.Registry.Registration.Dependency;
using PantherDI.Resolved.Providers;
using PantherDI.Resolvers;
using PantherDI.Resolvers.Aggregation;

namespace PantherDI.Tests.Resolvers
{
    [TestClass]
    public class FirstMatchResolverTests
    {
        [TestMethod]
        public void NoRegisteredResolverResolvesToNothing()
        {
            var sut = new FirstMatchResolver();
            sut.Resolve(null, new Dependency(typeof(string), "A")).Should().BeEmpty();
        }

        [TestMethod]
        public void AllRegisteredResolversReturnEmptyListsReturnsNothing()
        {
            var sut = new FirstMatchResolver();
            sut.Resolve(null, new Dependency(typeof(string), "A")).Should().BeEmpty();

            var resolverMock1 = new Mock<IResolver>(MockBehavior.Strict);
            resolverMock1
                .Setup(x => x.Resolve(It.IsAny<Func<IDependency, IEnumerable<IProvider>>>(),It.IsAny<IDependency>()))
                .Returns(Enumerable.Empty<IProvider>());
            sut.Add(resolverMock1.Object);

            var resolverMock2 = new Mock<IResolver>(MockBehavior.Strict);
            resolverMock2
                .Setup(x => x.Resolve(It.IsAny<Func<IDependency, IEnumerable<IProvider>>>(), It.IsAny<IDependency>()))
                .Returns(Enumerable.Empty<IProvider>());
            sut.Add(resolverMock2.Object);

            sut.Resolve(null, new Dependency(typeof(string), "A")).Should().BeEmpty();

            resolverMock1.VerifyAll();
            resolverMock2.VerifyAll();
        }

        [TestMethod]
        public void FirstResultIsTaken()
        {
            var sut = new FirstMatchResolver();
            sut.Resolve(null, new Dependency(typeof(string), "A")).Should().BeEmpty();


            var providers = new[] {new Mock<IProvider>()}.Select(x => x.Object);

            var resolverMock1 = new Mock<IResolver>(MockBehavior.Strict);
            resolverMock1
                .Setup(x => x.Resolve(It.IsAny<Func<IDependency, IEnumerable<IProvider>>>(), It.IsAny<IDependency>()))
                .Returns(providers);
            sut.Add(resolverMock1.Object);

            var resolverMock2 = new Mock<IResolver>(MockBehavior.Strict);
            sut.Add(resolverMock2.Object);

            sut.Resolve(null, new Dependency(typeof(string), "A")).Should().BeEquivalentTo(providers);

            resolverMock1.VerifyAll();
            resolverMock2.VerifyAll();
        }

        [TestMethod]
        public void NextResolverIsCalledWhenPreviousDoesNotGiveResult()
        {
            var sut = new FirstMatchResolver();
            sut.Resolve(null, new Dependency(typeof(string), "A")).Should().BeEmpty();

            var providers = new[] { new Mock<IProvider>() }.Select(x => x.Object);

            var resolverMock1 = new Mock<IResolver>(MockBehavior.Strict);
            resolverMock1
                .Setup(x => x.Resolve(It.IsAny<Func<IDependency, IEnumerable<IProvider>>>(), It.IsAny<IDependency>()))
                .Returns(Enumerable.Empty<IProvider>());
            sut.Add(resolverMock1.Object);

            var resolverMock2 = new Mock<IResolver>(MockBehavior.Strict);
            resolverMock2
                .Setup(x => x.Resolve(It.IsAny<Func<IDependency, IEnumerable<IProvider>>>(), It.IsAny<IDependency>()))
                .Returns(providers);
            sut.Add(resolverMock2.Object);

            sut.Resolve(null, new Dependency(typeof(string), "A")).Should().BeEquivalentTo(providers);

            resolverMock1.VerifyAll();
            resolverMock2.VerifyAll();
        }
    }
}