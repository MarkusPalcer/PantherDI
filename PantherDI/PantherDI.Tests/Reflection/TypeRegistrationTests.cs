using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PantherDI.Attributes;
using PantherDI.Registry.Registration.Registration;

namespace PantherDI.Tests.Reflection
{
    [TestClass]
    public class TypeRegistrationTests
    {
        private class TestClass1 { }

        [TestMethod]
        public void NoAttributes()
        {
            var sut = TypeRegistration.Create<TestClass1>();
            sut.FulfilledContracts.Should().BeEquivalentTo(typeof(TestClass1));
            sut.Factories.Should().HaveCount(1);
            sut.RegisteredType.Should().Be(typeof(TestClass1));
            sut.Singleton.Should().BeFalse();
        }

        [Contract, Contract("A")]
        private class TestClass2 { }

        [TestMethod]
        public void ContractAttributes()
        {
            var sut = TypeRegistration.Create<TestClass2>();
            sut.FulfilledContracts.Should().BeEquivalentTo(typeof(TestClass2), "A");
            sut.Factories.Should().HaveCount(1);
            sut.RegisteredType.Should().Be(typeof(TestClass2));
            sut.Singleton.Should().BeFalse();
        }

        [Contract]
        private interface TestInterface1 { }

        private class TestClass3 : TestClass2, TestInterface1 { }

        [TestMethod]
        public void InheritedContractAttributes()
        {
            var sut = TypeRegistration.Create<TestClass3>();
            sut.FulfilledContracts.Should().BeEquivalentTo(typeof(TestClass2), "A", typeof(TestInterface1));
            sut.Factories.Should().HaveCount(1);
            sut.RegisteredType.Should().Be(typeof(TestClass3));
            sut.Singleton.Should().BeFalse();
        }

        [Singleton]
        private class TestClass4 { }

        [TestMethod]
        public void SingletonAttribute()
        {
            var sut = TypeRegistration.Create<TestClass4>();
            sut.FulfilledContracts.Should().BeEquivalentTo(typeof(TestClass4));
            sut.Factories.Should().HaveCount(1);
            sut.RegisteredType.Should().Be(typeof(TestClass4));
            sut.Singleton.Should().BeTrue();
        }

        private class TestClass5
        {
            public TestClass5() { }

            public TestClass5(int i) { }
        }

        [TestMethod]
        public void MultipleConstructors()
        {
            var sut = TypeRegistration.Create<TestClass5>();
            sut.FulfilledContracts.Should().BeEquivalentTo(typeof(TestClass5));
            sut.Factories.Should().HaveCount(2);
            sut.RegisteredType.Should().Be(typeof(TestClass5));
            sut.Singleton.Should().BeFalse();
        }
    }
}