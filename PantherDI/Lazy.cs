using System;
using System.Threading;

namespace PantherDI
{
    public class Lazy<T, TMetadata> : Lazy<T>
        where TMetadata:new()
    {
        public Lazy(Func<T> valueFactory) : base(valueFactory)
        {
        }

        public TMetadata Metadata { get; } = new TMetadata();
    }
}