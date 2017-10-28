using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PantherDI.Exceptions;
using PantherDI.Extensions;
using PantherDI.Registry.Catalog;
using PantherDI.Tests.Helpers;

namespace PantherDI.Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void WithoutRegistrationResolvingFails()
        {
            var sut = Container.Create(new Catalog());

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

            var sut = Container.Create(new Catalog(new Registration()
            {
                FulfilledContracts = { typeof(ICatalog), "SomeContract" },
                Factories = { new Factory(Factory) },
                RegisteredType = typeof(Catalog)
            }));

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

            string Factory(object[] p)
            {
                p.Should().BeEquivalentTo(string.Empty);

                factoryCounter++;
                return string.Empty;
            }

            var dependencyCounter = 0;

            string Dependency(object[] p)
            {
                p.Should().BeEmpty();
                dependencyCounter++;
                return string.Empty;
            }

            var sut = Container.Create(new Catalog(new Registration
            {
                RegisteredType = typeof(string),
                Factories =
                {
                    new Factory(Factory, new Dependency(typeof(int)))
                }
            }, new Registration
            {
                RegisteredType = typeof(int),
                Factories =
                {
                    new Factory(Dependency)
                }
            }));

            sut.Resolve<string>();
            factoryCounter.Should().Be(1);
            dependencyCounter.Should().Be(1);
        }

        [TestMethod]
        public void TooManySuitableFactoriesThrows()
        {
            var sut = Container.Create(new Catalog(new Registration
            {
                RegisteredType = typeof(object),
                Factories = { new Factory(_ => null) }
            }, new Registration()
            {
                RegisteredType = typeof(object),
                Factories = { new Factory(_ => null) }
            }));

            sut.Invoking(x => x.Resolve<object>()).ShouldThrow<TooManySuitableRegistrationsException>();
        }

        [TestMethod]
        public void DetectCircularDependencies()
        {
            var sut = Container.Create(new Catalog(new Registration
            {
                RegisteredType = typeof(object),
                FulfilledContracts = {"A"},
                Factories = {new Factory(_ => null, new Dependency(typeof(object), "B"))}
            }, new Registration()
            {
                RegisteredType = typeof(object),
                FulfilledContracts = {"B"},
                Factories = {new Factory(_ => null, new Dependency(typeof(object), "A"))}
            }));

            sut.Invoking(x => x.Resolve<object>("A")).ShouldThrow<CircularDependencyException>();
        }
    }
}