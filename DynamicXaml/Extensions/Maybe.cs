namespace DynamicXaml.Extensions
{
    public interface Maybe
    {
        bool HasValue { get; }
        object Value { get; }
    }

    public struct Maybe<T> : Maybe
    {
        private readonly T _o;
        private readonly bool _hasValue;

        public Maybe(T o) : this(o, DetermineIfHasValue(o)) { }

        /// <summary>
        /// Use this to ensure on value types that the maybe acts correctly
        /// </summary>
        public Maybe(T o, bool hasValue)
        {
            _o = o;
            _hasValue = hasValue;
        }

        public static Maybe<T> None
        {
            get { return new Maybe<T>();}
        }

        public bool HasValue { get { return _hasValue; } }

        public T Value { get { return _o; } }

        object Maybe.Value
        {
            get { return Value; }
        }

        public static implicit operator bool(Maybe<T> maybeValue)
        {
            return maybeValue.HasValue;
        }

        private static bool DetermineIfHasValue(T t)
        {
            return !Equals(default(T), t);
        }

        public override string ToString()
        {
            if (!HasValue)
                return "Maybe<{0}>.None".Fmt(typeof(T).Name);
            return "Maybe<{0}>".Fmt(Value);
        }
    }
}