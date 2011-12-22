using System;
using System.Collections.Generic;

namespace DynamicXaml.Extensions
{
    public static class StandardExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var i in items)
                action(i);
        }
    }
}