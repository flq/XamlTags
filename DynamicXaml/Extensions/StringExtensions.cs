using System;
using System.Linq;

namespace DynamicXaml.Extensions
{
    public static class StringExtensions
    {
        public static string Fmt(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        public static bool StartsWithAnyOf(this string str, params string[] starters)
        {
            return starters.Any(str.StartsWith);
        }

        public static bool InvariantEquals(this string s, string other)
        {
            return s.Equals(other, StringComparison.InvariantCultureIgnoreCase);
        }

        public static Maybe<int> GetInt(this string s)
        {
            int value;
            if (int.TryParse(s, out value))
                return new Maybe<int>(value);
            return Maybe<int>.None;
        }
    }
}