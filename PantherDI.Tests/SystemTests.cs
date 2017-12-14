using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PantherDI.ContainerCreation;
using PantherDI.Exceptions;
using PantherDI.Registry.Catalog;
using PantherDI.Registry.Registration.Dependency;
using PantherDI.Registry.Registration.Registration;
using PantherDI.Tests.Helpers;

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
                    Factories = { new Factory(Factory) },
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

            string Factory(object[] p)
            {
                p.Should().HaveCount(1);
                p[0].Should().Be(dependencyMock);

                factoryCounter++;
                return string.Empty;
            }

            var dependencyCounter = 0;

            ICatalog Dependency(object[] p)
            {
                p.Should().BeEmpty();
                dependencyCounter++;
                return dependencyMock;
            }

            var builder = new ContainerBuilder();
            builder.Register<string>()
                .WithFactory(new Factory(Factory, new Dependency(typeof(ICatalog))));
            builder.Register<Catalog>()
                .As<ICatalog>()
                .WithFactory(new Factory(Dependency));
            var sut = builder.Build();

            sut.Resolve<string>();
            factoryCounter.Should().Be(1);
            dependencyCounter.Should().Be(1);
        }

        [TestMethod]
        public void TooManySuitableFactoriesThrows()
        {
            var sut = new ContainerBuilder{new ManualRegistration
            {
                RegisteredType = typeof(object),
                Factories = {new Factory(_ => null)}
            }, new ManualRegistration
            {
                RegisteredType = typeof(string),
                FulfilledContracts = { typeof(object) },
                Factories = { new Factory(_ => null) }
            }}.Build();

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
    }
}