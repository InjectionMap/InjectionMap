using System;
using System.Collections.Generic;

namespace InjectionMap.Internals
{
    internal static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration)
            {
                action(item);
            }
        }

        public static void AddFrom<T>(this IList<T> list, IEnumerable<T> enumeration)
        {
            enumeration.ForEach<T>(item => list.Add(item));
        }
    }
}
