using System;
using System.Collections.Generic;

namespace CommonLibrary.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (T item in items)
                action.Invoke(item);
        }
    }
}