using System.Linq;
using PantherDI.Resolved;
using PantherDI.Resolvers.Aggregation;

namespace PantherDI.Tests.Helpers
{
    public static class ContainerExtensions
    {
        public static KnowledgeBase KnowledgeBase(this Container container)
        {
            return ((FirstMatchResolver)container.RootResolver).OfType<KnowledgeBase>().Single();
        }
    }
}