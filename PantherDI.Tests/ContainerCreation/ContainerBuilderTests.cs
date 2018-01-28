    using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PantherDI.ContainerCreation;
using PantherDI.Exceptions;
using PantherDI.Registry.Catalog;
using PantherDI.Registry.Registration.Dependency;
    using PantherDI.Registry.Registration.Factory;
    using PantherDI.Registry.Registration.Registration;
using PantherDI.Resolved;
    using PantherDI.Resolvers;
    using PantherDI.Resolvers.Aggregation;
    using PantherDI.Tests.Helpers;

namespace PantherDI.Tests.ContainerCreation
{
    [TestClass]
    public class ContainerBuilderTests
    {
        [TestMethod]
        public void NoRegistrationYieldsEmptyKnowledgeBase()
        {
            var sut = (Container)new ContainerBuilder
            {
                new Catalog()
            }
            .Build();

            ((KnowledgeBase)sut.KnowledgeBase()).KnownProviders.Should().BeEmpty();
        }

        [TestMethod]
        public void SimpleRegistration()
        {
            var catalog = new Catalog(new ManualRegistration()
            {
                FulfilledContracts = {typeof(ICatalog), "SomeContract"},
                Factories = {DelegateFactory.Create<Catalog>(() => null)},
                RegisteredType = typeof(Catalog)
            });

            var sut = (Container)new ContainerBuilder()
            {
                catalog
            }
            .Build();

            var knowledgeBase = sut.KnowledgeBase();
            knowledgeBase.KnownProviders.Keys.Should().BeEquivalentTo(typeof(ICatalog), "SomeContract", typeof(Catalog));

            var results = knowledgeBase[typeof(ICatalog)].ToArray();
            results.Should().HaveCount(1);
            var result = results[0];
            result.FulfilledContracts.Should().BeEquivalentTo(typeof(ICatalog), "SomeContract", typeof(Catalog));
            result.ResultType.Should().Be(typeof(Catalog));
            result.UnresolvedDependencies.Should().BeEmpty();
            result.Singleton.Should().BeFalse();

            results = knowledgeBase["SomeContract"].ToArray();
            results.Should().HaveCount(1);
            result = results[0];
            result.FulfilledContracts.Should().BeEquivalentTo(typeof(ICatalog), "SomeContract", typeof(Catalog));
            result.ResultType.Should().Be(typeof(Catalog));
            result.UnresolvedDependencies.Should().BeEmpty();
            result.Singleton.Should().BeFalse();
        }

        [TestMethod]
        public void DependencyInjection()
        {
            var catalog = new Catalog(new ManualRegistration
            {
                RegisteredType = typeof(string),
                Factories =
                {
                    DelegateFactory.Create<int, string>(i => string.Empty)
                }
            }, new ManualRegistration
            {
                RegisteredType = typeof(int),
                Factories =
                {
                    DelegateFactory.Create(() => 42)
                },
                Singleton = true
            });

            var sut = (Container)new ContainerBuilder {catalog}.Build();
            var knowledgeBase = (KnowledgeBase)sut.KnowledgeBase();

            knowledgeBase.KnownProviders.Keys.Should().BeEquivalentTo(typeof(string), typeof(int));

            var results = knowledgeBase[typeof(string)].ToArray();
            results.Should().HaveCount(1);
            var result = results[0];
            result.FulfilledContracts.Should().BeEquivalentTo(typeof(string));
            result.ResultType.Should().Be(typeof(string));
            result.UnresolvedDependencies.Should().BeEmpty();
            result.Singleton.Should().BeFalse();

            results = knowledgeBase[typeof(int)].ToArray();
            results.Should().HaveCount(1);
            result = results[0];
            result.FulfilledContracts.Should().BeEquivalentTo(typeof(int));
            result.ResultType.Should().Be(typeof(int));
            result.UnresolvedDependencies.Should().BeEmpty();
            result.Singleton.Should().BeTrue();
        }

        [TestMethod]
        public void DetectCircularDependencies()
        {
            var sut = new ContainerBuilder{new Catalog(new ManualRegistration
            {
                RegisteredType = typeof(object),
                FulfilledContracts = { "A" },
                Factories = { new DelegateFactory(_ => null, Enumerable.Empty<object>(), new []{ new Dependency(typeof(object), "B")}) }
            }, new ManualRegistration()
            {
                RegisteredType = typeof(object),
                FulfilledContracts = { "B" },
                Factories = { new DelegateFactory(_ => null, Enumerable.Empty<object>(), new[] { new Dependency(typeof(object), "A")}) }
            })};

            sut.Invoking(x => x.Build()).ShouldThrow<CircularDependencyException>();
        }

        [TestMethod]
        public void RegisteringWithMetadata()
        {
            var sut = new ContainerBuilder();
            sut.Register<ContainerBuilderTests>()
                .WithConstructors()
               .WithMetadata("Test", 123);

            var cnt = (Container)sut.Build();

            var kb = ((FirstMatchResolver) cnt._rootResolver).OfType<KnowledgeBase>().Single();

            var providers = kb[typeof(ContainerBuilderTests)].ToArray();
            var provider = providers.Single();
            var entry = provider.Metadata.Single();
            entry.Key.Should().Be("Test");
            entry.Value.Should().Be(123);
        }
    }
}