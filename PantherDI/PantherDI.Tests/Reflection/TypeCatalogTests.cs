using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PantherDI.Attributes;
using PantherDI.Registry.Catalog;

namespace PantherDI.Tests.Reflection
{
    [TestClass]
    public class TypeCatalogTests
    {
        [TestMethod]
        public void EmptyCatalog()
        {
            var sut = new TypeCatalog();

            sut.Registrations.Should().BeEmpty();
        }

        private class TestClass1 { }


        [Contract, Contract("A")]
        private class TestClass2 { }

        [Contract]
        private interface TestInterface1 { }

        private class TestClass3 : TestClass2, TestInterface1 { }

        [Singleton]
        private class TestClass4 { }

        private class TestClass5
        {
            public TestClass5() { }

            public TestClass5(int i) { }
        }

        [TestMethod]
        public void ContainsRegistrationsForEachType()
        {
            var sut = new TypeCatalog
            {
                typeof(TestClass1),
                typeof(TestClass2),
                typeof(TestClass3),
                typeof(TestClass4),
                typeof(TestClass5)
            };

            var registrations = sut.Registrations.ToArray();

            var registration = registrations[0];
            registration.FulfilledContracts.Should().BeEquivalentTo(typeof(TestClass1));
            registration.Factories.Should().HaveCount(1);
            registration.RegisteredType.Should().Be(typeof(TestClass1));
            registration.Singleton.Should().BeFalse();

            registration = registrations[1];
            registration.FulfilledContracts.Should().BeEquivalentTo(typeof(TestClass2), "A");
            registration.Factories.Should().HaveCount(1);
            registration.RegisteredType.Should().Be(typeof(TestClass2));
            registration.Singleton.Should().BeFalse();

            registration = registrations[2];
            registration.FulfilledContracts.Should().BeEquivalentTo(typeof(TestClass2), "A", typeof(TestInterface1));
            registration.Factories.Should().HaveCount(1);
            registration.RegisteredType.Should().Be(typeof(TestClass3));
            registration.Singleton.Should().BeFalse();

            registration = registrations[3];
            registration.FulfilledContracts.Should().BeEquivalentTo(typeof(TestClass4));
            registration.Factories.Should().HaveCount(1);
            registration.RegisteredType.Should().Be(typeof(TestClass4));
            registration.Singleton.Should().BeTrue();

            registration = registrations[4];
            registration.FulfilledContracts.Should().BeEquivalentTo(typeof(TestClass5));
            registration.Factories.Should().HaveCount(2);
            registration.RegisteredType.Should().Be(typeof(TestClass5));
            registration.Singleton.Should().BeFalse();
        }
    }
}