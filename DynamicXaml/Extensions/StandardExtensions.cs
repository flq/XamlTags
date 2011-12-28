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

        public static IEnumerable<object> Flatten(this IEnumerable<object> enumerable)
        {
            foreach(var thing in enumerable)
            {
                if (thing.CanBeCastTo<IEnumerable<object>>())
                {
                    foreach (var innerThing in ((IEnumerable<object>)thing).Flatten())
                        yield return innerThing;
                }
                else
                    yield return thing;
            }
        }
    }
}