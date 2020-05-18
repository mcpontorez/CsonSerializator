using System;
using System.Collections.Generic;

namespace SerializatorApp.Serialization.Utils
{
    public static class HashSetExtensions
    {
        public static HashSet<TItem> AddRange<TItem>(this HashSet<TItem> target, IEnumerable<TItem> range)
        {
            foreach (var item in range)
                target.Add(item);
            return target;
        }
    }
}
