using System;

namespace DynamicXaml.Extensions
{
    public interface Maybe
    {
        bool HasValue { get; }
        object Value { get; }
    }

    public class Maybe<T> : Maybe where T : class
    {
        private readonly T _o;

        public Maybe()
        {
            _o = null;
        }

        public Maybe(T o)
        {
            _o = o;
        }

        private static readonly Lazy<Maybe<T>> _none = new Lazy<Maybe<T>>(()=> new Maybe<T>());
        public static Maybe<T> None
        {
            get { return _none.Value;}
        }

        public bool HasValue { get { return _o != null; } }
        public T Value { get { return _o; } }

        object Maybe.Value
        {
            get { return Value; }
        }

        public static implicit operator bool(Maybe<T> maybeValue)
        {
            return maybeValue.HasValue;
        }
    }
}