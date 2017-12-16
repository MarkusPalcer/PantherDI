using System.Linq;
using PantherDI.Resolved;
using PantherDI.Resolvers;

namespace PantherDI.Tests.Helpers
{
    public static class ContainerExtensions
    {
        public static KnowledgeBase KnowledgeBase(this Container container)
        {
            return ((MergedResolver)container._rootResolver).OfType<KnowledgeBase>().Single();
        }
    }
}