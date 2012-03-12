using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicXaml.Extensions
{
    public static class MaybeExtensions
    {
        public static Maybe<T> ToMaybe<T>(this T @object) where T : class
        {
            return new Maybe<T>(@object, @object != null);
        }

        public static Maybe<T> ToMaybe<T>(this T? @object) where T : struct
        {
            return @object.HasValue ? new Maybe<T>(@object.Value) : Maybe<T>.None;
        }

        [Obsolete("Please use GetValue for getting the value or a provided default")]
        public static T MustHaveValue<T>(this Maybe<T> @object, T defaultValue)
        {
            return @object.GetValue(defaultValue);
        }

        public static T GetValue<T>(this Maybe<T> @object, T defaultValue)
        {
            return !@object ? defaultValue : @object.Value;
        }

        public static T MustHaveValue<T>(this Maybe<T> @object, Exception raiseIfNoValue = null)
        {
            if (!@object.HasValue)
                throw raiseIfNoValue ?? new InvalidOperationException("Maybe<{0}> has no value and hence a value is not obtainable".Fmt(typeof(T).Name));
            return @object.Value;
        }

        public static Maybe<Z> Maybe<T,Z>(this T @object, params Func<Maybe<T>,Maybe<Z>>[] maybes) where T : class where Z : class
        {
            var v = new Maybe<T>(@object);
            return maybes.Select(maybe => maybe(v)).FirstOrDefault(maybeZ => maybeZ.HasValue);
        }

        // On collections...

        public static Maybe<T> MaybeFirst<T>(this IEnumerable<T> items, Func<T,bool> predicate) where T : class
        {
            return items.FirstOrDefault(predicate).ToMaybe();
        }

        public static Maybe<T> MaybeFirst<T>(this IEnumerable<T> items) where T : class
        {
            return items.FirstOrDefault(_ => true).ToMaybe();
        }

        public static Maybe<T> MaybeFirst<T>(this IEnumerable<Maybe<T>> items) where T : class
        {
            return items.FirstOrDefault(i => i.HasValue); // ?? Maybe<T>.None;
        }

        public static Maybe<U> Get<T,U>(this IDictionary<T,U> dictionary, T key)
        {
            U value;
            var success = dictionary.TryGetValue(key, out value);
            return new Maybe<U>(value, success);
        }

        // Bind, etc. ...

        public static Maybe<T> Do<T>(this Maybe<T> value, Action<T> action)
        {
            if (value)
                action(value.Value);
            return value;
        }

        public static Maybe<U> Get<T,U>(this Maybe<T> value, Func<T,U> map)
        {
            return value ? new Maybe<U>(map(value.Value)) : Maybe<U>.None;
        }

        public static Maybe<U> Get<T, U>(this Maybe<T> value, Func<T, Maybe<U>> map)
        {
            return value ? map(value.Value) : Maybe<U>.None;
        }

        /// <summary>
        /// Returns false if either the Maybe has no value (None), or
        /// if the condition returns false;
        /// </summary>
        public static bool Is<T>(this Maybe<T> value, Func<T,bool> condition)
        {
            return value.HasValue && condition(value.Value);
        }

        public static Maybe<T> Or<T>(this Maybe<T> value, Maybe<T> orValue)
        {
            if (value.HasValue)
                return value;
            return orValue;
        }

        public static Maybe<T> ToMaybeOf<T>(this object value)
        {
            if (ReferenceEquals(value, null))
                return Maybe<T>.None;
            if (value.CanBeCastTo<T>())
                return new Maybe<T>((T)value);
            return Maybe<T>.None;
        }

        public static Maybe<T> Cast<T>(this Maybe maybeValue)
        {
            if (!maybeValue.HasValue || !maybeValue.Value.CanBeCastTo<T>())
                return Maybe<T>.None;
            return new Maybe<T>((T)maybeValue.Value);
        }
    }
}