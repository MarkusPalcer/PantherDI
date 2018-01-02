using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PantherDI.Registry.Catalog;
using PantherDI.Registry.Registration.Dependency;
using PantherDI.Registry.Registration.Factory;
using PantherDI.Registry.Registration.Registration;
using PantherDI.Tests.Helpers;

namespace PantherDI.Tests
{
    [TestClass]
    public class MergedCatalogTests
    {
        [TestMethod]
        public void EmptyCatalog()
        {
            var sut = new MergedCatalog();

            sut.Registrations.Should().BeEmpty();
        }

        [TestMethod]
        public void SingleCatalog()
        {
            var catalog1 = new Catalog(
                new ManualRegistration
                {
                    RegisteredType = typeof(string),
                    FulfilledContracts = {typeof(string), "A"},
                    Factories = {new DelegateFactory(_ => string.Empty, Enumerable.Empty<object>(), Enumerable.Empty<IDependency>())}
                });

            var sut = new MergedCatalog(catalog1);
            var registrations = sut.Registrations.ToArray();

            registrations.Should().HaveCount(1);
            registrations[0].FulfilledContracts.Should().BeEquivalentTo(typeof(string), "A");
            registrations[0].Factories.Should().BeEquivalentTo(catalog1.Registrations.First().Factories);
            registrations[0].RegisteredType.Should().Be(typeof(string));
        }

        [TestMethod]
        public void DifferentTypes()
        {
            var catalog1 = new Catalog(
                new ManualRegistration
                {
                    RegisteredType = typeof(string),
                    FulfilledContracts = { typeof(string), "A" },
                    Factories = { new DelegateFactory(_ => string.Empty, Enumerable.Empty<object>(), Enumerable.Empty<IDependency>()) }
                });

            var catalog2 = new Catalog(
                new ManualRegistration
                {
                    RegisteredType = typeof(int),
                    FulfilledContracts = { typeof(int), "B" },
                    Factories = { new DelegateFactory(_ => 2, Enumerable.Empty<object>(), Enumerable.Empty<IDependency>()) }
                });

            var sut = new MergedCatalog(catalog1, catalog2);
            var registrations = sut.Registrations.ToArray();

            registrations.Should().HaveCount(2);
            registrations[0].FulfilledContracts.Should().BeEquivalentTo(typeof(string), "A");
            registrations[0].Factories.Should().BeEquivalentTo(catalog1.Registrations.First().Factories);
            registrations[0].RegisteredType.Should().Be(typeof(string));

            registrations[1].FulfilledContracts.Should().BeEquivalentTo(typeof(int), "B");
            registrations[1].Factories.Should().BeEquivalentTo(catalog2.Registrations.First().Factories);
            registrations[1].RegisteredType.Should().Be(typeof(int));
        }

        [TestMethod]
        public void MergeType()
        {
            var catalog1 = new Catalog(
                new ManualRegistration
                {
                    RegisteredType = typeof(string),
                    FulfilledContracts = { typeof(string), "A" },
                    Factories = { new DelegateFactory(_ => string.Empty, Enumerable.Empty<object>(), Enumerable.Empty<IDependency>()) }
                });

            var catalog2 = new Catalog(
                new ManualRegistration
                {
                    RegisteredType = typeof(string),
                    FulfilledContracts = { typeof(string), "C" },
                    Factories = { new DelegateFactory(_ => "123", Enumerable.Empty<object>(), new[]{ new Dependency(typeof(int))}) }
                });

            var sut = new MergedCatalog(catalog1, catalog2);
            var registrations = sut.Registrations.ToArray();

            registrations.Should().HaveCount(1);
            registrations[0].FulfilledContracts.Should().BeEquivalentTo(typeof(string), "A", "C");
            registrations[0].Factories.Should().BeEquivalentTo(catalog1.Registrations.First().Factories.First(), catalog2.Registrations.First().Factories.First());
            registrations[0].RegisteredType.Should().Be(typeof(string));
        }

        [TestMethod]
        public void MergeConstructorFactory()
        {
            var catalog1 = new Catalog(
                new ManualRegistration
                {
                    RegisteredType = typeof(string),
                    FulfilledContracts = { typeof(string), "A" },
                    Factories = { new DelegateFactory(_ => string.Empty, Enumerable.Empty<object>(), Enumerable.Empty<IDependency>()) }
                });

            var catalog2 = new Catalog(
                new ManualRegistration
                {
                    RegisteredType = typeof(string),
                    FulfilledContracts = { typeof(string), "C" },
                    Factories = { new DelegateFactory(_ => "123", Enumerable.Empty<object>(), Enumerable.Empty<IDependency>()) }
                });

            var sut = new MergedCatalog(catalog1, catalog2);
            var registrations = sut.Registrations.ToArray();

            registrations.Should().HaveCount(1);
            registrations[0].FulfilledContracts.Should().BeEquivalentTo(typeof(string), "A", "C");
            registrations[0].Factories.Should().BeEquivalentTo(catalog1.Registrations.First().Factories, "Factories should be merged based on their 'signature'");
            registrations[0].RegisteredType.Should().Be(typeof(string));
        }
    }
}