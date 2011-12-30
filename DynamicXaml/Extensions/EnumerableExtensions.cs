using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicXaml.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var i in items)
                action(i);
        }

        public static void Execute<T>(this IEnumerable<T> items)
        {
            items.ForEach(i => {});
        }

        public static IEnumerable<T> Pipeline<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var i in items)
            {
                action(i);
                yield return i;
            }
        }

        public static IEnumerable<T> Multiply<T>(this IEnumerable<T> items, int multiplier, Func<T,T> copyFunction)
        {
            if (multiplier == 0)
                yield break;
            if (multiplier == 1)
                foreach (var i in items)
                    yield return i;

            var q = new Queue<T>(items);
            var count = q.Count;
            multiplier -= 1; //It Enqueue new values at least once, so with m = 2, only run once

            while (multiplier > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    var value = q.Dequeue();
                    yield return value;
                    q.Enqueue(copyFunction(value));
                }
                multiplier--;
            }

            // Empty the queue
            while (q.Count > 0)
                yield return q.Dequeue();
        }

        public static IEnumerable<T> Multiply<T>(this IEnumerable<T> items, int multiplier)
        {
            return items.Multiply(multiplier, v => v);
        }

        public static IEnumerable<T> Concat<T>(params IEnumerable<T>[] bits)
        {
            if (bits.Length == 1)
                return bits[0];

            var ret = bits[0];

            for (var i = 1; i < bits.Length; i++)
                ret = ret.Concat(bits[i]);

            return ret;
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