using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PantherDI.Attributes;
using PantherDI.ContainerCreation;
using PantherDI.Exceptions;
using PantherDI.Extensions.TypeRegistrationHelper;
using PantherDI.Registry.Catalog;
using PantherDI.Registry.Registration;
using PantherDI.Registry.Registration.Factory;
using PantherDI.Registry.Registration.Registration;
using PantherDI.Resolvers;
using PantherDI.Tests.Helpers;

// ReSharper disable InconsistentNaming
// ReSharper disable ClassNeverInstantiated.Global

// ReSharper disable UnusedMember.Local
// ReSharper disable ClassNeverInstantiated.Local
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace PantherDI.Tests
{
    [TestClass]
    public class SystemTests
    {
        [TestMethod]
        public void WithoutRegistrationResolvingFails()
        {
            var sut = new ContainerBuilder().Build();

            sut.Invoking(cnt => cnt.Resolve<string>()).ShouldThrow<NoSuitableRegistrationException>();
            sut.Invoking(cnt => cnt.Resolve<object>(typeof(string))).ShouldThrow<NoSuitableRegistrationException>();
        }

        [TestMethod]
        public void BasicResolution()
        {
            var instance = new Catalog();
            var invokeCounter = 0;

            ICatalog Factory(object[] _)
            {
                invokeCounter++;
                return instance;
            }

            var sut = new ContainerBuilder()
                      .WithRegistration(
                                        new ManualRegistration()
                                        {
                                            FulfilledContracts = { typeof(ICatalog), "SomeContract" },
                                            Factories = { new DelegateFactory(Factory, Enumerable.Empty<object>(), Enumerable.Empty<Dependency>()) },
                                            RegisteredType = typeof(Catalog)
                                        })
                      .Build();

            object resolved = sut.Resolve<ICatalog>();
            invokeCounter.Should().Be(1);
            resolved.Should().BeSameAs(instance);

            resolved = sut.Resolve<object>(typeof(ICatalog));
            invokeCounter.Should().Be(2);
            resolved.Should().BeSameAs(instance);

            resolved = sut.Resolve<object>("SomeContract");
            invokeCounter.Should().Be(3);
            resolved.Should().BeSameAs(instance);

            resolved = sut.Resolve<Catalog>();
            invokeCounter.Should().Be(4);
            resolved.Should().BeSameAs(instance);

            sut.Invoking(cnt => cnt.Resolve<int>()).ShouldThrow<NoSuitableRegistrationException>();
            sut.Invoking(cnt => cnt.Resolve<int>("SomeContract")).ShouldThrow<NoSuitableRegistrationException>();
        }

        [TestMethod]
        public void DependencyInjection()
        {
            var factoryCounter = 0;
            var dependencyMock = new Mock<ICatalog>().Object;

            string Factory(ICatalog p)
            {
                p.Should().Be(dependencyMock);

                factoryCounter++;
                return string.Empty;
            }

            var dependencyCounter = 0;

            ICatalog Dependency()
            {
                dependencyCounter++;
                return dependencyMock;
            }

            var cb = new ContainerBuilder();
            cb.Register<ICatalog>().WithFactory(Dependency);
            cb.Register<string>().WithFactory<ICatalog, string>(Factory);

            var sut = cb.Build();

            sut.Resolve<string>();
            factoryCounter.Should().Be(1);
            dependencyCounter.Should().Be(1);
        }

        [TestMethod]
        public void TooManySuitableFactoriesThrows()
        {
            var sut = new ContainerBuilder()
                .WithRegistration(new ManualRegistration
                {
                    RegisteredType = typeof(object),
                    Factories = { DelegateFactory.Create<object>(() => null) }
                })
                .WithRegistration(new ManualRegistration
                {
                    RegisteredType = typeof(string),
                    FulfilledContracts = { typeof(object) },
                    Factories = { DelegateFactory.Create<string>(() => null) }
                })
                .Build();

            sut.Invoking(x => x.Resolve<object>()).ShouldThrow<TooManySuitableRegistrationsException>();
        }

        [TestMethod]
        public void InstanceRegistration()
        {
            var instance = new Catalog();

            var sut = new ContainerBuilder()
                      .WithInstance<ICatalog>(instance)
                      .Build();

            sut.Resolve<ICatalog>().Should().BeSameAs(instance);
            sut.Resolve<ICatalog>().Should().BeSameAs(instance);
        }

        private class TestClass1
        {
        }

        private class TestClass2
        {
            public TestClass1 ResolvedDependency { get; }

            public TestClass2(TestClass1 resolvedDependency)
            {
                ResolvedDependency = resolvedDependency;
            }
        }

        [TestMethod]
        public void ResolveFunction()
        {
            var sut = new ContainerBuilder()
                      .WithType<TestClass1>()
                      .WithGenericResolvers()
                      .Build();

            var resolvedFunction = sut.Resolve<Func<TestClass1>>();
            resolvedFunction.Invoking(x => x()).ShouldNotThrow();
        }

        [TestMethod]
        public void ResolveFunctionWithParameter()
        {
            var sut = new ContainerBuilder()
                      .WithType<TestClass2>()
                      .WithGenericResolvers()
                      .Build();

            sut.Invoking(x => x.Resolve<TestClass2>()).ShouldThrow<NoSuitableRegistrationException>();
            var resolvedFunction = sut.Resolve<Func<TestClass1, TestClass2>>();
            var dependency = new TestClass1();
            var resolvedInstance = resolvedFunction(dependency);
            resolvedInstance.ResolvedDependency.Should().BeSameAs(dependency);
        }

        private class TestClass3
        {
            public TestClass1 ResolvedDependency { get; }

            public TestClass3([Attributes.Ignore] TestClass1 resolvedDependency)
            {
                ResolvedDependency = resolvedDependency;
            }
        }

        [TestMethod]
        public void IgnoreConstructorParameters()
        {
            var sut = new ContainerBuilder()
                      .WithType<TestClass1>()
                      .WithType<TestClass3>()
                      .WithGenericResolvers()
                      .Build();

            sut.Invoking(x => x.Resolve<TestClass1>()).ShouldNotThrow();
            sut.Invoking(x => x.Resolve<TestClass3>()).ShouldThrow<NoSuitableRegistrationException>();
            var resolvedFunction = sut.Resolve<Func<TestClass1, TestClass3>>();
            var dependency = new TestClass1();
            var resolvedInstance = resolvedFunction(dependency);
            resolvedInstance.ResolvedDependency.Should().BeSameAs(dependency);
        }

        private class TestClass4
        {
            public TestClass1 ResolvedDependency { get; }

            public TestClass4(TestClass1 resolvedDependency)
            {
                ResolvedDependency = resolvedDependency;
            }

            [Attributes.Ignore]
            public TestClass4()
            {
            }
        }

        [TestMethod]
        public void IgnoreConstructors()
        {
            var sut = new ContainerBuilder()
                      .WithType<TestClass4>()
                      .WithGenericResolvers()
                      .Build();

            // Empty constructor should be ignored
            sut.Invoking(x => x.Resolve<TestClass4>()).ShouldThrow<NoSuitableRegistrationException>();

            var resolvedFunction = sut.Resolve<Func<TestClass1, TestClass4>>();
            var dependency = new TestClass1();
            var resolvedInstance = resolvedFunction(dependency);
            resolvedInstance.ResolvedDependency.Should().BeSameAs(dependency);
        }

        private class TestClass5
        {
            public TestClass5(TestClass6 d)
            {
            }
        }

        private class TestClass6
        {
            public TestClass6(TestClass5 d)
            {
            }
        }

        [TestMethod]
        public void CircularDependency()
        {
            var sut = new ContainerBuilder()
                      .WithType<TestClass1>()
                      .WithType<TestClass5>()
                      .WithType<TestClass6>();

            sut.Invoking(x => x.Build()).ShouldThrow<CircularDependencyException>();
        }

        [TestMethod]
        public void LateProcessing()
        {
            /* 
             * If TestClass5 or TestClass6 were processed
             * a circular dependency would be found
             */

            var sut = new ContainerBuilder()
                      .WithType<TestClass1>()
                      .WithType<TestClass5>()
                      .WithType<TestClass6>()
                      .WithLateProcessing()
                      .Build();

            sut.Invoking(x => x.Resolve<TestClass1>()).ShouldNotThrow();
            sut.Invoking(x => x.Resolve<TestClass5>()).ShouldThrow<CircularDependencyException>();
            sut.Invoking(x => x.Resolve<TestClass6>()).ShouldThrow<CircularDependencyException>();
        }

        [TestMethod]
        public void RegisterTypeWithRegistrationHelper()
        {
            var cb = new ContainerBuilder();
            cb.Register<TestClass1>()
              .WithConstructors();

            var sut = cb.Build();

            sut.Invoking(x => x.Resolve<TestClass1>()).ShouldNotThrow();
        }

        private class MyMetadataAttribute : MetadataAttribute
        {
            public MyMetadataAttribute(Type value) : base("Entry3", value)
            {
            }

            public MyMetadataAttribute() : base("Entry3")
            {
            }
        }

        private class Metadata1
        {
            public string Entry1 { get; set; }

            [Metadata("Entry2")]
            public int Something { get; set; }

            [MyMetadata]
            public Type AnotherThing { get; set; }
        }

        [TestMethod]
        public void ManualRegisteredMetadata()
        {
            var cb = new ContainerBuilder()
                .WithGenericResolvers();
            cb.Register<TestClass1>()
              .WithConstructors()
              .WithMetadata("Entry1", "StringEntry")
              .WithMetadata("Entry2", 42)
              .WithMetadata("Entry3", typeof(string));
            var sut = cb.Build();

            var resolved = sut.Resolve<Lazy<TestClass1, Metadata1>>();

            resolved.Metadata.Entry1.Should().Be("StringEntry");
            resolved.Metadata.Something.Should().Be(42);
            resolved.Metadata.AnotherThing.Should().Be(typeof(string));
        }

        [Metadata("Entry1", "Value")]
        [Metadata("Entry2", 42)]
        [MyMetadata(typeof(string))]
        private class TestClass7
        {
        }

        [TestMethod]
        public void AutoRegisteredMetadata()
        {
            var cb = new ContainerBuilder()
                .WithGenericResolvers();
            cb.Register<TestClass7>()
              .WithConstructors()
              .WithMetadataViaReflection();

            var sut = cb.Build();

            var resolved = sut.Resolve<Lazy<TestClass7, Metadata1>>();

            resolved.Metadata.Entry1.Should().Be("Value");
            resolved.Metadata.Something.Should().Be(42);
            resolved.Metadata.AnotherThing.Should().Be(typeof(string));
        }

        // ReSharper disable once UnusedMember.Global
        public class TestClass8
        {
            [Contract]
            // ReSharper disable once UnusedMember.Global
            public static string Registration1 => "Test";

            [Contract("Test")]
            // ReSharper disable once UnusedMember.Global
            public static string Registration2 = "Value";
        }

        [TestMethod]
        public void RegisteredInstancesViaPropertyAndField()
        {
            var sut = new ContainerBuilder()
                      .WithAssemblyOf<SystemTests>()
                      .WithGenericResolvers()
                      .Build();

            sut.Resolve<string>("Registration1").Should().Be("Test");
            sut.Resolve<string>("Test").Should().Be("Value");
        }

        private class TestClass9
        {
        }

        [Factory]
        [Contract("Test")]
        private static TestClass9 TestFactory()
        {
            return new TestClass9();
        }

        [TestMethod]
        public void FactoryRegisteredViaReflection()
        {
            var sut = new ContainerBuilder().WithAssemblyOf<SystemTests>().Build();
            sut.Invoking(x => x.Resolve<TestClass9>()).ShouldNotThrow();
            sut.Invoking(x => x.Resolve<TestClass9>("Test")).ShouldNotThrow();
        }

        [TestMethod]
        public void FactoryRegisteredManually()
        {
            var cb = new ContainerBuilder();
            cb.Register<TestClass9>().WithFactory(TestFactory);
            var sut = cb.Build();

            sut.Invoking(x => x.Resolve<TestClass9>()).ShouldNotThrow();
            sut.Invoking(x => x.Resolve<TestClass9>("Test")).ShouldThrow<NoSuitableRegistrationException>();
        }

        private class TestClass12
        {
            public TestClass12(TestClass9 dependency)
            {
            }
        }

        [TestMethod]
        // Tests that issue #31 doesn't appear again
        public void FactoryWithContractCanBeUsedForDependenciesDuringContainerCreation()
        {
            var cb = new ContainerBuilder();

            cb.WithType<TestClass12>();
            cb.Register<TestClass9>()
              .WithFactory(() => new TestClass9(), "Test");

            var sut = cb.Build();
            sut.Resolve<TestClass12>();
        }

        [TestMethod]
        public void FactoryWithContractIsAddedWithAdditionalContract()
        {
            var counter = 0;

            TestClass9 Factory()
            {
                counter++;
                return new TestClass9();
            }

            var cb = new ContainerBuilder();
            cb.Register<TestClass9>()
              .WithConstructors()
              .WithFactory(Factory, "Test");

            var sut = cb.Build();
            sut.Invoking(x => x.Resolve<TestClass9>()).ShouldThrow<TooManySuitableRegistrationsException>();
            counter.Should().Be(0);
            sut.Resolve<TestClass9>("Test");
            counter.Should().Be(1);
        }

        private class TestClass13
        {
        }

        private class TestClass14
        {
            public TestClass13 Dependency { get; }

            public TestClass14(TestClass13 dependency)
            {
                Dependency = dependency;
            }
        }

        private class TestClass15
        {
            public TestClass14 Dependency { get; }

            public TestClass15(TestClass14 dependency)
            {
                Dependency = dependency;
            }
        }

        [TestMethod]
        // Regression test against issue #35
        public void DependenciesAreTransitive()
        {
            var sut = new ContainerBuilder()
                      .WithType<TestClass14>()
                      .WithType<TestClass15>()
                      .WithFuncResolvers()
                      .Build();

            sut.Invoking(x => x.Resolve<TestClass15>()).ShouldThrow<NoSuitableRegistrationException>();

            var factory = sut.Resolve<Func<TestClass13, TestClass15>>();
            var given = new TestClass13();

            var result = factory(given);

            result.Dependency.Dependency.Should().BeSameAs(given);
        }

        [TestMethod]
        public void LooseModeResolvesUnregisteredTypes()
        {
            var sut = new ContainerBuilder()
                      .WithSupportForUnregisteredTypes()
                      .Build();

            var result = sut.Resolve<TestClass15>();
            result.Dependency.Should().NotBeNull();
            result.Dependency.Dependency.Should().NotBeNull();
        }

        [TestMethod]
        public void RemovalOfResolversByType()
        {
            var sut = new ContainerBuilder()
                      .WithFuncResolvers()
                      .WithoutResolver<Func0Resolver>()
                      .WithType<TestClass13>()
                      .WithType<TestClass15>()
                      .Build();

            sut.Invoking(x => x.Resolve<TestClass13>()).ShouldNotThrow("TestClass13 has no dependency.");
            sut.Invoking(x => x.Resolve<Func<TestClass13>>()).ShouldThrow<NoSuitableRegistrationException>("the Func0Resolver has been removed.");
            sut.Invoking(x => x.Resolve<TestClass15>()).ShouldThrow<NoSuitableRegistrationException>("TestClass15 has a dependency which is not registered.");
            sut.Invoking(x => x.Resolve<Func<TestClass14, TestClass15>>()).ShouldNotThrow("Func1Resolver is still present.");
        }

        [TestMethod]
        public void StrictModeCanBeReenabled()
        {
            var sut = new ContainerBuilder()
                      .WithSupportForUnregisteredTypes()
                      .WithStrictRegistrationHandling()
                      .Build();

            sut.Invoking(x => x.Resolve<TestClass13>()).ShouldThrow<NoSuitableRegistrationException>();
        }

        [TestMethod]
        public void ContainerCanBeResolved()
        {
            var sut = new ContainerBuilder().Build();

            sut.Resolve<IContainer>().Should().BeSameAs(sut);
            sut.Resolve<Container>().Should().BeSameAs(sut);
        }

        [TestMethod]
        public void ContainerCanBeUsedAsCatalog()
        {
            // src is missing TestClass 13
            var src = new ContainerBuilder()
                      .WithType<TestClass15>()
                      .WithType<TestClass14>()
                      .Build();

            // Sut only has TestClass 13
            var sut = new ContainerBuilder()
                      .WithType<TestClass13>()
                      .WithCatalog(src.AsCatalog())
                      .Build();

            sut.Invoking(x => x.Resolve<TestClass13>()).ShouldNotThrow();
            sut.Invoking(x => x.Resolve<TestClass14>()).ShouldNotThrow();
            sut.Invoking(x => x.Resolve<TestClass15>()).ShouldNotThrow();
        }

        [TestMethod]
        public void ChildContainersCanAccessParentRegistrations()
        {
            var parentBuilder = new ContainerBuilder()
                                    .WithType<TestClass14>()
                                    .WithFuncResolvers();
            parentBuilder.WithMultipleGenerations();

            var parent = parentBuilder.Build();

            var child = parent.Resolve<IContainerBuilder>()
                              .WithType<TestClass13>()
                              .WithType<TestClass15>()
                              .Build();

            // Child can access the parent
            child.Invoking(x => x.Resolve<TestClass13>()).ShouldNotThrow();
            child.Invoking(x => x.Resolve<TestClass14>()).ShouldNotThrow();
            child.Invoking(x => x.Resolve<TestClass15>()).ShouldNotThrow();

            // Parent can't access the child. 
            parent.Invoking(x => x.Resolve<TestClass13>()).ShouldThrow<NoSuitableRegistrationException>();
            parent.Invoking(x => x.Resolve<TestClass14>()).ShouldThrow<NoSuitableRegistrationException>();
            parent.Invoking(x => x.Resolve<TestClass15>()).ShouldThrow<NoSuitableRegistrationException>();
            parent.Invoking(x => x.Resolve<Func<TestClass13, TestClass14>>()).ShouldNotThrow();
        }


        [TestMethod]
        public void ChildContainersCanAccessParentResolvers()
        {
            var parentBuilder = new ContainerBuilder()
                .WithSupportForUnregisteredTypes();
            parentBuilder.WithMultipleGenerations();

            var parent = parentBuilder
                         .Build();

            var child = parent.Resolve<IContainerBuilder>()
                              .WithType<TestClass14>()
                              .WithType<TestClass15>()
                              .WithResolver(new EnumerableResolver())
                              .Build();

            // Child can access the parent resolvers
            child.Invoking(x => x.Resolve<TestClass13>()).ShouldNotThrow("the child should have access to the parents support for unregistered types");
            child.Invoking(x => x.Resolve<IEnumerable<TestClass13>>()).ShouldNotThrow();

            // Parent can't access the child resolvers. 
            parent.Invoking(x => x.Resolve<TestClass13>()).ShouldNotThrow("the parent should have support for unregistered types");
            parent.Invoking(x => x.Resolve<IEnumerable<TestClass13>>()).ShouldThrow<NoSuitableRegistrationException>("the parent should not have access to the childs EnumerableResolver");
        }

        [TestMethod]
        public void GenerationsAreCapped()
        {
            var parentBuilder = new ContainerBuilder()
                .WithSupportForUnregisteredTypes();
            parentBuilder.WithMultipleGenerations()
                         .WithMaxiumNumberOfChildGenerations(3);

            var cnt = parentBuilder
                .Build();

            // Three child generations are allowed
            cnt = cnt.Resolve<IContainerBuilder>().Build();
            cnt = cnt.Resolve<IContainerBuilder>().Build();
            cnt = cnt.Resolve<IContainerBuilder>().Build();

            cnt.Invoking(x => x.Resolve<IContainerBuilder>().Build()).ShouldThrow<MaximumNumberOfGenerationsExceededException>();
        }
    }
}