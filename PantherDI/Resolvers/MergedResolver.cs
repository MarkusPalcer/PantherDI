using System;
using System.Collections.Generic;
using PantherDI.Resolvers.Aggregation;

namespace PantherDI.Resolvers
{
    /// <summary>
    /// Resolves the given dependency by selecting the first resolver that produces an actual result
    /// </summary>
    [Obsolete("Use PantherDI.Resolvers.Aggregation.FirstMatchResolver instead.")]
    public class MergedResolver : FirstMatchResolver
    {
        public MergedResolver()
        {
        }

        public MergedResolver(IEnumerable<IResolver> resolvers)
        {
            AddRange(resolvers);
        }
    }
}