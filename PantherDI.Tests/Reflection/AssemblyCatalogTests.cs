using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PantherDI.Attributes;
using PantherDI.Registry.Catalog;

namespace PantherDI.Tests.Reflection
{
    [TestClass]
    public class AssemblyCatalogTests
    {
        [Contract]
        [Attributes.Ignore]
        public class TestClass1 { }

        [TestMethod]
        public void TestAssembly()
        {
            var sut = new AssemblyCatalog(typeof(AssemblyCatalogTests).Assembly);
            sut.Registrations.Should().HaveCount(11);
        }
    }
}