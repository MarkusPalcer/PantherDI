using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PantherDI.Extensions
{
    static class EnumerableExtensions
    {
        public static IEnumerable<TOut> SelectManyForPairs<TIn, TPairWith, TOut>(this IEnumerable<TIn> source,
                                                                                 Func<TIn, IEnumerable<TPairWith>> pairSelector,
                                                                                 Func<TIn, TPairWith, TOut> selectFromPair)
        {
            return source.SelectMany(x => pairSelector(x).Select(y => Tuple.Create(x, y))).Select(x => selectFromPair(x.Item1, x.Item2));
        }
    }
}

