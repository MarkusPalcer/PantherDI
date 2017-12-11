using System.Linq;
using System.Reflection;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PantherDI.Attributes;
using PantherDI.Registry.Registration.Factory;

namespace PantherDI.Tests.Reflection
{
    [TestClass]
    public class ConstructorFactoryTests
    {
        private class TestClass1 { }

        [TestMethod]
        public void EmptyConstructor()
        {
            var sut = new ConstructorFactory(typeof(TestClass1).GetTypeInfo().DeclaredConstructors.Single());

            sut.Dependencies.Should().BeEmpty();
            var result = sut.Execute(new object[] { });
            result.Should().BeOfType<TestClass1>();
        }

        private class TestClass2
        {
            public TestClass2(TestClass1 d) { }
        }

        [TestMethod]
        public void ConstructorWithParameters()
        {
            var sut = new ConstructorFactory(typeof(TestClass2).GetTypeInfo().DeclaredConstructors.Single());

            var dependencies = sut.Dependencies.ToArray();
            dependencies.Should().HaveCount(1);
            dependencies[0].ExpectedType.Should().Be(typeof(TestClass1));
            dependencies[0].RequiredContracts.Should().BeEquivalentTo(typeof(TestClass1));
            var result = sut.Execute(new object[] { new TestClass1() });
            result.Should().BeOfType<TestClass2>();
        }

        private class TestClass3
        {
            public TestClass3([Contract("A")] int d) { }
        }

        [TestMethod]
        public void ConstructorWithContractAttributeOnParameters()
        {
            var sut = new ConstructorFactory(typeof(TestClass3).GetTypeInfo().DeclaredConstructors.Single());

            var dependencies = sut.Dependencies.ToArray();
            dependencies.Should().HaveCount(1);
            dependencies[0].ExpectedType.Should().Be(typeof(int));
            dependencies[0].RequiredContracts.Should().BeEquivalentTo((object)"A");
            var result = sut.Execute(new object[] { 2 });
            result.Should().BeOfType<TestClass3>();
        }

        private class TestClass4
        {
            public TestClass4([Contract("A"), Contract] int d) { }
        }

        [TestMethod]
        public void ConstructorWithEmptyContractAttributeOnParameter()
        {
            var sut = new ConstructorFactory(typeof(TestClass4).GetTypeInfo().DeclaredConstructors.Single());

            var dependencies = sut.Dependencies.ToArray();
            dependencies.Should().HaveCount(1);
            dependencies[0].ExpectedType.Should().Be(typeof(int));
            dependencies[0].RequiredContracts.Should().BeEquivalentTo("A", typeof(int));
            var result = sut.Execute(new object[] { 2 });
            result.Should().BeOfType<TestClass4>();
        }
    }
}