using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PantherDI.Registry.Catalog;

namespace PantherDI.Tests.Reflection
{
    [TestClass]
    public class AssemblyCatalogTests
    {
        [TestMethod]
        public void TestAssembly()
        {
            var sut = new AssemblyCatalog(typeof(AssemblyCatalogTests).Assembly);
            sut.Registrations.Select(x => x.RegisteredType).Should().HaveCount(10);
        }
    }
}