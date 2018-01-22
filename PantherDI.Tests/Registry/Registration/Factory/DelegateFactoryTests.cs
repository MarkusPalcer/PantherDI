using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PantherDI.Registry.Registration.Factory;

namespace PantherDI.Tests.Registry.Registration.Factory
{
    [TestClass]
    public class DelegateFactoryTests
    {
        [TestMethod]
        public void ParametersBecomeDependencies()
        {
            string Delegate1(int dependency)
            {
                return string.Empty;
            }

            string Delegate2(int d1, string d2)
            {
                return null;
            }

            var sut = DelegateFactory.Create<int, string>(Delegate1);
            
            sut.Dependencies.Should().HaveCount(1);
            sut.Dependencies.First().ExpectedType.Should().Be(typeof(int));
            sut.Dependencies.First().Ignored.Should().Be(false);
            sut.Dependencies.First().RequiredContracts.Should().BeEquivalentTo(typeof(int));

            sut = DelegateFactory.Create<int, string, string>(Delegate2);

            var deps = sut.Dependencies.ToArray();

            deps.Should().HaveCount(2);
            deps[0].ExpectedType.Should().Be(typeof(int));
            deps[0].Ignored.Should().Be(false);
            deps[0].RequiredContracts.Should().BeEquivalentTo(typeof(int));
            deps[1].ExpectedType.Should().Be(typeof(string));
            deps[1].Ignored.Should().Be(false);
            deps[1].RequiredContracts.Should().BeEquivalentTo(typeof(string));
        }

        [TestMethod]
        public void DelegateIsInvokedWithParametersInGivenOrder()
        {
            bool called = false;
            string Delegate2(int d1, string d2)
            {
                d1.Should().Be(42);
                d2.Should().Be("FourtyTwo");
                called = true;
                return "Result";
            }

            var sut = DelegateFactory.Create<int, string, string>(Delegate2);
            sut.Invoking(x => x.Execute(new object[] { })).ShouldThrow<IndexOutOfRangeException>();
            sut.Invoking(x => x.Execute(new object[] {"FourtyTwo", 42})).ShouldThrow<InvalidCastException>();
            called.Should().BeFalse();
            sut.Execute(new object[] {42, "FourtyTwo"}).Should().Be("Result");
            called.Should().BeTrue();
        }

        [TestMethod]
        public void NoContractPreFilling()
        {
            var sut = DelegateFactory.Create(() => string.Empty);
            sut.FulfilledContracts.Should().BeEmpty();
        }

        [TestMethod]
        public void ContractsAreCopied()
        {
            var sut = DelegateFactory.Create(() => string.Empty, 1, 2, 3);
            sut.FulfilledContracts.Should().BeEquivalentTo(1, 2, 3);
        }
    }
}