using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PantherDI.ContainerCreation;
using PantherDI.Exceptions;
using PantherDI.Registry.Registration;
using PantherDI.Resolved.Providers;
using PantherDI.Tests.Helpers;
using PantherDI.Tests.Reflection;

namespace PantherDI.Tests
{
    [TestClass]
    public class ContainerResolverTests
    {
        private static IEnumerable<IProvider> FailingDependencyResolver(Dependency dependency)
        {
            throw new AssertionFailedException("Unexpected call to dependencyResolver");
        }

        public class TestType1 {}

        public class TestType2
        {
            public TestType1 Dependency;

            public TestType2(TestType1 dependency)
            {
                Dependency = dependency;
            }
        }

        [TestMethod]
        public void ResolveTypeWithoutDependencies()
        {
            var sut = new ContainerBuilder()
                      .WithType<TestType1>()
                      .Build()
                      .AsResolver();


            sut.Resolve(FailingDependencyResolver, new Dependency(typeof(TestType1)));
        }

        [TestMethod]
        public void ResolveTypeWithUnfulfilledDependencies()
        {
            var sut = new ContainerBuilder()
                      .WithType<TestType2>()
                      .Build()
                      .AsResolver();

            var outsideResolutions = new DependencyResolverDictionary()
            {
                {new Dependency(typeof(TestType1)), Enumerable.Empty<IProvider>()}
            };

            var result = sut.Resolve(outsideResolutions.Execute, new Dependency(typeof(TestType2))).ToArray();
            result.Should().HaveCount(1);
            result[0].ResultType.Should().Be<TestType2>();
            result[0].UnresolvedDependencies.Should().HaveCount(1);
            result[0].UnresolvedDependencies.First().ExpectedType.Should().Be<TestType1>();
            outsideResolutions.CallsFor(new Dependency(typeof(TestType1))).Should().Be(1);
        }

        [TestMethod]
        public void ResolveTypeWithDependenciesFulfilledInContainer()
        {
            var sut = new ContainerBuilder()
                      .WithType<TestType1>()
                      .WithType<TestType2>()
                      .Build()
                      .AsResolver();

            var result = sut.Resolve(FailingDependencyResolver, new Dependency(typeof(TestType2))).ToArray();
            result.Should().HaveCount(1);
            result[0].ResultType.Should().Be<TestType2>();
            result[0].UnresolvedDependencies.Should().BeEmpty();

            var instance = result[0].CreateInstance(new Dictionary<Dependency, object>());
            instance.Should().BeOfType<TestType2>();
            instance.As<TestType2>().Dependency.Should().NotBeNull();
        }

        [TestMethod]
        public void ResolveTypeWithDependenciesFulfilledOutside()
        {
            var sut = new ContainerBuilder()
                      .WithType<TestType2>()
                      .Build()
                      .AsResolver();

            var provider = new TestProvider(new TestType1());

            var outsideResolutions = new DependencyResolverDictionary()
            {
                {new Dependency(typeof(TestType1)), new[] {provider}}
            };

            var result = sut.Resolve(outsideResolutions.Execute, new Dependency(typeof(TestType2))).ToArray();
            result.Should().HaveCount(1);
            result[0].ResultType.Should().Be<TestType2>();
            result[0].UnresolvedDependencies.Should().BeEmpty();

            var instance = result[0].CreateInstance(new Dictionary<Dependency, object>());
            instance.Should().BeOfType<TestType2>();
            instance.As<TestType2>().Dependency.Should().NotBeNull();
        }
    }
}