using System;

namespace DynamicXaml.Extensions
{
    public interface Maybe
    {
        bool HasValue { get; }
        object Value { get; }
    }

    public class Maybe<T> : Maybe
    {
        private readonly T _o;
        private readonly bool? _hasValue;

        public Maybe() : this(default(T), false) { }
        public Maybe(T o) : this(o, null) { }

        /// <summary>
        /// Use this to ensure on value types that the maybe acts correctly
        /// </summary>
        public Maybe(T o, bool? hasValue)
        {
            _o = o;
            _hasValue = hasValue;
        }

        private static readonly Lazy<Maybe<T>> _none = new Lazy<Maybe<T>>(()=> new Maybe<T>());
        public static Maybe<T> None
        {
            get { return _none.Value;}
        }

        public bool HasValue { get { return _hasValue.HasValue ? _hasValue.Value :  _o != null; } }
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