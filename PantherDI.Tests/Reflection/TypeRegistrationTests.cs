using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PantherDI.Attributes;
using PantherDI.Registry.Registration.Registration;

namespace PantherDI.Tests.Reflection
{
    [TestClass]
    public class TypeRegistrationTests
    {
        private class TestClass1
        {
        }

        [TestMethod]
        public void NoAttributes()
        {
            var sut = TypeRegistration.Create<TestClass1>();
            sut.FulfilledContracts.Should().BeEquivalentTo(typeof(TestClass1));
            sut.Factories.Should().HaveCount(1);
            sut.RegisteredType.Should().Be(typeof(TestClass1));
            sut.Singleton.Should().BeFalse();
            sut.Metadata.Should().BeEmpty();
        }

        [Contract, Contract("A")]
        private class TestClass2
        {
        }

        [TestMethod]
        public void ContractAttributes()
        {
            var sut = TypeRegistration.Create<TestClass2>();
            sut.FulfilledContracts.Should().BeEquivalentTo(typeof(TestClass2), "A");
            sut.Factories.Should().HaveCount(1);
            sut.RegisteredType.Should().Be(typeof(TestClass2));
            sut.Singleton.Should().BeFalse();
            sut.Metadata.Should().BeEmpty();
        }

        [Contract]
        private interface TestInterface1
        {
        }

        private class TestClass3 : TestClass2, TestInterface1
        {
        }

        [TestMethod]
        public void InheritedContractAttributes()
        {
            var sut = TypeRegistration.Create<TestClass3>();
            sut.FulfilledContracts.Should().BeEquivalentTo(typeof(TestClass2), "A", typeof(TestInterface1));
            sut.Factories.Should().HaveCount(1);
            sut.RegisteredType.Should().Be(typeof(TestClass3));
            sut.Singleton.Should().BeFalse();
            sut.Metadata.Should().BeEmpty();
        }

        [Singleton]
        private class TestClass4
        {
        }

        [TestMethod]
        public void SingletonAttribute()
        {
            var sut = TypeRegistration.Create<TestClass4>();
            sut.FulfilledContracts.Should().BeEquivalentTo(typeof(TestClass4));
            sut.Factories.Should().HaveCount(1);
            sut.RegisteredType.Should().Be(typeof(TestClass4));
            sut.Singleton.Should().BeTrue();
            sut.Metadata.Should().BeEmpty();
        }

        private class TestClass5
        {
            public TestClass5()
            {
            }

            public TestClass5(int i)
            {
            }
        }

        [TestMethod]
        public void MultipleConstructors()
        {
            var sut = TypeRegistration.Create<TestClass5>();
            sut.FulfilledContracts.Should().BeEquivalentTo(typeof(TestClass5));
            sut.Factories.Should().HaveCount(2);
            sut.RegisteredType.Should().Be(typeof(TestClass5));
            sut.Singleton.Should().BeFalse();
            sut.Metadata.Should().BeEmpty();
        }

        [Metadata("Entry1", "Value")]
        [Metadata("Entry2", 42)]
        [Metadata("Entry3", typeof(TestClass6))]
        private class TestClass6
        {
        }

        [TestMethod]
        public void ScanForMetadata()
        {
            var sut = TypeRegistration.Create<TestClass6>();
            sut.FulfilledContracts.Should().BeEquivalentTo(typeof(TestClass6));
            sut.Factories.Should().HaveCount(1);
            sut.RegisteredType.Should().Be(typeof(TestClass6));
            sut.Singleton.Should().BeFalse();
            sut.Metadata["Entry1"].Should().Be("Value");
            sut.Metadata["Entry2"].Should().Be(42);
            sut.Metadata["Entry3"].Should().Be(typeof(TestClass6));
        }

        [Metadata("Entry2", 21)]
        [Metadata("Ignored")]
        private class TestClass7 : TestClass6
        {
        }

        [TestMethod]
        public void OverrideMetadataOfParentClasses()
        {
            var sut = TypeRegistration.Create<TestClass7>();
            sut.Metadata["Entry1"].Should().Be("Value");
            sut.Metadata["Entry2"].Should().Be(21);
            sut.Metadata["Entry3"].Should().Be(typeof(TestClass6));
            sut.Metadata.ContainsKey("Ignored").Should().BeFalse();
        }

        private class TestClass8
        {
            [Metadata("Entry1")]
            public static string Something => "Value";

            [Metadata]
            public static int Entry2 => 42;
        }

        [TestMethod]
        public void MetadataProperties()
        {
            var sut = TypeRegistration.Create<TestClass8>();
            sut.Metadata.Should().HaveCount(2);
            sut.Metadata["Entry1"].Should().Be("Value");
            sut.Metadata["Entry2"].Should().Be(42);
        }

        private class TestClass9
        {
            [Metadata("Entry3")] public static Type Anything = typeof(TestClass9);

            [Metadata] public static string Entry4 = "abc";
        }

        [TestMethod]
        public void MetadataFields()
        {
            var sut = TypeRegistration.Create<TestClass9>();
            sut.Metadata.Should().HaveCount(2);
            sut.Metadata["Entry3"].Should().Be(typeof(TestClass9));
            sut.Metadata["Entry4"].Should().Be("abc");
        }

        private class TestClass10 : TestClass8
        {
        }

        [TestMethod]
        public void InhreitedMetadataProperties()
        {
            var sut = TypeRegistration.Create<TestClass10>();
            sut.Metadata.Should().HaveCount(2);
            sut.Metadata["Entry1"].Should().Be("Value");
            sut.Metadata["Entry2"].Should().Be(42);
        }

        private class TestClass11 : TestClass9
        {
        }

        [TestMethod]
        public void InheritedMetadataFields()
        {
            var sut = TypeRegistration.Create<TestClass11>();
            sut.Metadata.Should().HaveCount(2);
            sut.Metadata["Entry3"].Should().Be(typeof(TestClass9));
            sut.Metadata["Entry4"].Should().Be("abc");
        }

        [Metadata("Entry1", "Value")]
        private class TestClass12
        {
            [Metadata]
            public static string Entry1 => "Value2";
        }

        [TestMethod]
        public void PropertiesOverrideTypeMetadata()
        {
            var sut = TypeRegistration.Create<TestClass12>();
            sut.Metadata.Should().HaveCount(1);
            sut.Metadata["Entry1"].Should().Be("Value2");
        }

        [Metadata("Entry1", "Value")]
        private class TestClass13
        {
            [Metadata] public static string Entry1 = "Value2";
        }

        [TestMethod]
        public void FieldsOverrideTypeMetadata()
        {
            var sut = TypeRegistration.Create<TestClass13>();
            sut.Metadata.Should().HaveCount(1);
            sut.Metadata["Entry1"].Should().Be("Value2");
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

        [MyMetadata(typeof(TestClass14))]
        private class TestClass14
        {
        }

        [TestMethod]
        public void CustomMetadataAttribute()
        {
            var sut = TypeRegistration.Create<TestClass14>();
            sut.Metadata.Should().HaveCount(1);
            sut.Metadata["Entry3"].Should().Be(typeof(TestClass14));
        }


    }
}