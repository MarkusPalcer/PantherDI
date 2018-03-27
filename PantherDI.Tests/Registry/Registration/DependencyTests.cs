using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PantherDI.Registry.Registration;

namespace PantherDI.Tests.Registry.Registration
{
    [TestClass]
    public class DependencyTests
    {
        [TestMethod]
        public void DependencyIsDesignedSoItIsUsableAsKeyWithoutEqualityComparer()
        {
            var dep1 = new Dependency(typeof(string));
            var dep2 = new Dependency(typeof(string));

            dep1.ExpectedType.GetHashCode().Should().Be(dep2.ExpectedType.GetHashCode());
            dep1.RequiredContracts.Single().GetHashCode().Should().Be(dep2.RequiredContracts.Single().GetHashCode());
            dep1.Ignored.GetHashCode().Should().Be(dep2.Ignored.GetHashCode());
            dep1.GetHashCode().Should().Be(dep2.GetHashCode());

            var tester = new Dictionary<Dependency, string>
            {
                [dep1] = "123",
                [dep2] = "456"
            };


            tester.Should().HaveCount(1);

            new HashSet<Dependency>(new[] {dep1, dep2}).Should().HaveCount(1);
        }
    }
}