using System;
using System.Collections.Generic;
using System.Linq;

namespace Whitebox.Profiler.Util
{
    static class Traverse
    {
        public static IEnumerable<T> PreOrder<T>(T root, Func<T, IEnumerable<T>> childMapping)
        {
            yield return root;
            foreach (var child in childMapping(root).SelectMany(c => PreOrder(c, childMapping)))
            {
                yield return child;
            }
        }
    }
}
