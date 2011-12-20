using System;
using System.Collections.Generic;

namespace DynamicXaml.Extensions
{
    public static class StandardExtensions
    {
        public static string Fmt(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var i in items)
                action(i);
        }
    }
}