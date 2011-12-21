using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicXaml.Extensions
{
    public static class MaybeExtensions
    {
        public static Maybe<T> MaybeFirst<T>(this IEnumerable<T> items, Func<T,bool> predicate) where T : class
        {
            return items.FirstOrDefault(predicate).ToMaybe();
        }

        public static Maybe<T> ToMaybe<T>(this T @object) where T : class
        {
            return new Maybe<T>(@object);
        }

        public static Maybe<T> Do<T>(this Maybe<T> value, Action<T> action) where T : class
        {
            if (value.HasValue)
                action(value.Value);
            return value;
        }

        public static Maybe<U> Get<T,U>(this Maybe<T> value, Func<T,U> map) where T : class where U : class
        {
            if (value.HasValue)
                return new Maybe<U>(map(value.Value));
            return Maybe<U>.None;
        }

        public static Maybe<T> Cast<T>(this Maybe maybeValue) where T : class
        {
            if (!maybeValue.HasValue || !maybeValue.Value.CanBeCastTo<T>())
                return Maybe<T>.None;
            return new Maybe<T>((T)maybeValue.Value);
        }
    }
}