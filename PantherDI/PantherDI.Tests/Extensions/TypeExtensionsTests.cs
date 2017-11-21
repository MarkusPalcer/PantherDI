using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PantherDI.Attributes;
using PantherDI.Extensions;

namespace PantherDI.Tests.Extensions
{
    [TestClass]
    public class TypeExtensionsTests
    {
        private class TestClass1 { }

        [TestMethod]
        public void TypeWithoutContracts()
        {
            typeof(TestClass1).GetFulfilledContracts().Should().BeEquivalentTo(typeof(TestClass1));
        }

        [Contract, Contract("A")]
        private class TestClass2 { }

        [TestMethod]
        public void DirectlyDecorated()
        {
            typeof(TestClass2).GetFulfilledContracts().Should().BeEquivalentTo(typeof(TestClass2), "A");
        }

        private class TestClass3 : TestClass1 { }

        [TestMethod]
        public void InheritingNonContracts()
        {
            typeof(TestClass3).GetFulfilledContracts().Should().BeEquivalentTo(typeof(TestClass3));
        }

        private class TestClass4 : TestClass2 { }

        [TestMethod]
        public void InheritingContracts()
        {
            typeof(TestClass4).GetFulfilledContracts().Should().BeEquivalentTo(typeof(TestClass2), "A");
        }

        [Contract, Contract("B")]
        private class TestClass5 : TestClass2 { }

        [TestMethod]
        public void MergingInheritingContracts()
        {
            typeof(TestClass5).GetFulfilledContracts().Should().BeEquivalentTo(typeof(TestClass2), typeof(TestClass5), "A", "B");
        }

        [Contract, Contract("C")]
        private interface TestInterface1 { }

        private class TestClass6 : TestInterface1 { }

        [TestMethod]
        public void ImplementingContracts()
        {
            typeof(TestClass6).GetFulfilledContracts().Should().BeEquivalentTo(typeof(TestInterface1), "C");
        }

        [Contract("D")]
        private interface TestInterface2 { }

        private class TestClass7 : TestInterface1, TestInterface2 { }

        [TestMethod]
        public void ImplementingMultipleContracts()
        {
            typeof(TestClass7).GetFulfilledContracts().Should().BeEquivalentTo(typeof(TestInterface1), "C", "D");
        }

        [Contract]
        private interface TestInterface3 :TestInterface2 { }

        private class TestClass8 : TestInterface3 { }

        [TestMethod]
        public void ImplementingContractsIndirectly()
        {
            typeof(TestClass8).GetFulfilledContracts().Should().BeEquivalentTo(typeof(TestInterface3), "D");
        }
    }

}