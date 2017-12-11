using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PantherDI.Attributes;
using PantherDI.Registry.Registration.Dependency;
using PantherDI.Resolved.Providers;
using PantherDI.Resolvers;
using PantherDI.Tests.Helpers;

namespace PantherDI.Tests.Resolvers
{
    [TestClass]
    public class ReflectionResolverTests
    {

        private abstract class AbstractType {}

        [TestMethod]
        public void RequestAbstractType()
        {
            var recursiveResolver = new Func<IDependency, IEnumerable<IProvider>>(_ =>
            {
                Assert.Fail();
                return Enumerable.Empty<IProvider>();
            });
            var sut = new ReflectionResolver();
            var result = sut.Resolve(recursiveResolver, new Dependency(typeof(AbstractType)));

            result.Should().BeEmpty();
        }

        private interface Interface { }

        [TestMethod]
        public void RequestInterface()
        {
            var recursiveResolver = new Func<IDependency, IEnumerable<IProvider>>(_ =>
            {
                Assert.Fail();
                return Enumerable.Empty<IProvider>();
            });
            var sut = new ReflectionResolver();
            var result = sut.Resolve(recursiveResolver, new Dependency(typeof(Interface)));

            result.Should().BeEmpty();
        }

        private class TestClass1 { }

        [TestMethod]
        public void RequestPlainType()
        {
            var recursiveResolver = new Func<IDependency, IEnumerable<IProvider>>(_ =>
            {
                Assert.Fail();
                return Enumerable.Empty<IProvider>();
            });
            var sut = new ReflectionResolver();
            var result = sut.Resolve(recursiveResolver, new Dependency(typeof(TestClass1))).ToArray();

            result.Should().HaveCount(1);
            result[0].UnresolvedDependencies.Should().BeEmpty();
            result[0].ResultType.Should().Be(typeof(TestClass1));
        }

        [TestMethod]
        public void RequestWithInterfaceWithoutImplementation()
        {
            var recursiveResolver = new Func<IDependency, IEnumerable<IProvider>>(_ =>
            {
                Assert.Fail();
                return Enumerable.Empty<IProvider>();
            });
            var sut = new ReflectionResolver();
            var result = sut.Resolve(recursiveResolver, new Dependency(typeof(Interface), typeof(TestClass1)));

            result.Should().BeEmpty();
        }

        private class TestClass2 : Interface { }

        [TestMethod]
        public void RequestWithInterfaceWithImplementation()
        {
            var recursiveResolver = new Func<IDependency, IEnumerable<IProvider>>(_ =>
            {
                Assert.Fail();
                return Enumerable.Empty<IProvider>();
            });
            var sut = new ReflectionResolver();
            var result = sut.Resolve(recursiveResolver, new Dependency(typeof(Interface), typeof(TestClass2))).ToArray();

            result.Should().HaveCount(1);
            result[0].UnresolvedDependencies.Should().BeEmpty();
            result[0].ResultType.Should().Be(typeof(TestClass2));
        }

        [TestMethod]
        public void RequestWithAbstractWithoutImplementation()
        {
            var recursiveResolver = new Func<IDependency, IEnumerable<IProvider>>(_ =>
            {
                Assert.Fail();
                return Enumerable.Empty<IProvider>();
            });
            var sut = new ReflectionResolver();
            var result = sut.Resolve(recursiveResolver, new Dependency(typeof(AbstractType), typeof(TestClass1)));

            result.Should().BeEmpty();
        }

        private class TestClass3 : AbstractType { }

        [TestMethod]
        public void RequestWithAbstractWithImplementation()
        {
            var recursiveResolver = new Func<IDependency, IEnumerable<IProvider>>(_ =>
            {
                Assert.Fail();
                return Enumerable.Empty<IProvider>();
            });
            var sut = new ReflectionResolver();
            var result = sut.Resolve(recursiveResolver, new Dependency(typeof(AbstractType), typeof(TestClass3))).ToArray();

            result.Should().HaveCount(1);
            result[0].UnresolvedDependencies.Should().BeEmpty();
            result[0].ResultType.Should().Be(typeof(TestClass3));
        }

        private class TestClass4
        {
            public TestClass4(Interface d) { }
        }


        [TestMethod]
        public void RequestWithUnresolvedDependency()
        {
            var recursiveResolver = new Func<IDependency, IEnumerable<IProvider>>(_ => Enumerable.Empty<IProvider>());

            var sut = new ReflectionResolver();
            var result = sut.Resolve(recursiveResolver, new Dependency(typeof(TestClass4))).ToArray();

            result.Should().HaveCount(1);
            result[0].UnresolvedDependencies.Should().HaveCount(1);
            result[0].UnresolvedDependencies.First().ExpectedType.Should().Be(typeof(Interface));
            result[0].UnresolvedDependencies.First().RequiredContracts.Should().BeEquivalentTo(typeof(Interface));
            result[0].ResultType.Should().Be(typeof(TestClass4));
        }

        [TestMethod]
        public void RequestWithResolvedDependency()
        {
            var recursiveResolver = new Func<IDependency, IEnumerable<IProvider>>(_ => new IProvider[]{new TestProvider(new TestClass2())});

            var sut = new ReflectionResolver();
            var result = sut.Resolve(recursiveResolver, new Dependency(typeof(TestClass4))).ToArray();

            result.Should().HaveCount(1);
            result[0].UnresolvedDependencies.Should().BeEmpty();
        }

        [Contract("A")]
        public class TestClass5 {}

        [TestMethod]
        public void RequestWithCustomContractAndUnfittingType()
        {
            var recursiveResolver = new Func<IDependency, IEnumerable<IProvider>>(_ =>
            {
                Assert.Fail();
                return Enumerable.Empty<IProvider>();
            });
            var sut = new ReflectionResolver();
            var result = sut.Resolve(recursiveResolver, new Dependency(typeof(TestClass1), "A")).ToArray();

            result.Should().HaveCount(0);
        }

        [TestMethod]
        public void RequestWithCustomContractAndFittingType()
        {
            var recursiveResolver = new Func<IDependency, IEnumerable<IProvider>>(_ =>
            {
                Assert.Fail();
                return Enumerable.Empty<IProvider>();
            });
            var sut = new ReflectionResolver();
            var result = sut.Resolve(recursiveResolver, new Dependency(typeof(TestClass5), "A")).ToArray();

            result.Should().HaveCount(1);
            result[0].UnresolvedDependencies.Should().BeEmpty();
            result[0].ResultType.Should().Be(typeof(TestClass5));
        }

    }
}