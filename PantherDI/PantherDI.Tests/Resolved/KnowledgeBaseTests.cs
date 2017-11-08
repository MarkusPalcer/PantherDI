using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PantherDI.Registry.Registration.Dependency;
using PantherDI.Resolved;

namespace PantherDI.Tests.Resolved
{
    [TestClass]
    public class KnowledgeBaseTests
    {
        [TestMethod]
        public void TryingToGetUnknownContractYieldsEmptyListOfProviders()
        {
            var sut = new KnowledgeBase();

            sut["A"].Should().BeEmpty();
        }

        [TestMethod]
        public void TryingToGetKnownContractReturnsProviders()
        {
            var sut = new KnowledgeBase();

            var p1 = new Mock<IProvider>(MockBehavior.Strict);
            p1.Setup(x => x.FulfilledContracts).Returns(new HashSet<object>(new[] { "A", "B" }));
            var p2 = new Mock<IProvider>(MockBehavior.Strict);
            p2.Setup(x => x.FulfilledContracts).Returns(new HashSet<object>(new[] { "A", "C" }));

            sut.Add(p1.Object);
            sut.Add(p2.Object);

            sut["A"].Should().BeEquivalentTo(p1.Object, p2.Object);
            sut["B"].Should().BeEquivalentTo(p1.Object);
            sut["C"].Should().BeEquivalentTo(p2.Object);
            sut["UnkownContract"].Should().BeEmpty();
        }

        [TestMethod]
        public void NoFindingsOnEmptyKnowledgeBase()
        {
            var sut = new KnowledgeBase();
            sut.Resolve(null, new Dependency(typeof(object), typeof(object))).Should().BeEmpty();
        }

        [TestMethod]
        public void KnowledgeBaseContentIsReturned()
        {
            var p1 = CreateMockedProvider("A", "B");
            var p2 = CreateMockedProvider("A", "C");

            var sut = new KnowledgeBase();
            sut.Add(p1);
            sut.Add(p2);
            sut.Resolve(null, new Dependency(typeof(object), "A")).Should()
                .BeEquivalentTo(p1, p2);

            sut.Resolve(null, new Dependency(typeof(object), "A", "B")).Should()
                .BeEquivalentTo(p1);

            sut.Resolve(null, new Dependency(typeof(object), "A", "C")).Should()
                .BeEquivalentTo(p2);

            sut.Resolve(null, new Dependency(typeof(KnowledgeBaseTests), "A")).Should()
                .BeEquivalentTo(p1, p2);

            sut.Resolve(null, new Dependency(typeof(string), "A")).Should()
                .BeEmpty();
        }

        [TestMethod]
        public void OmittingTheDependencyThrows()
        {
            var sut = new KnowledgeBase();

            sut.Invoking(x => x.Resolve(null, null)).ShouldThrow<ArgumentNullException>();
        }

        private static IProvider CreateMockedProvider(params string[] contracts)
        {
            var p1 = new Mock<IProvider>(MockBehavior.Strict);
            p1.Setup(x => x.FulfilledContracts).Returns(new HashSet<object>(contracts));
            p1.Setup(x => x.ResultType).Returns(typeof(KnowledgeBaseTests).GetTypeInfo());

            return p1.Object;
        }
    }
}