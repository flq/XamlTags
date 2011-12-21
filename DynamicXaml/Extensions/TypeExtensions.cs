using System;
using System.ComponentModel;

namespace DynamicXaml.Extensions
{
    internal static class TypeExtensions
    {
        public static Type GetPropertyType(this Type t, string propertyName)
        {
            var p = t.GetProperty(propertyName);
            if (p == null)
                throw new ArgumentException("Type {0} does not have a property named {1}".Fmt(t.Name, propertyName));
            return p.PropertyType;
        }

        public static Func<string,Type> GetPropertyTypeProvider(this Type t)
        {
            return s => t.GetPropertyType(s);
        }

        public static void SetValue(this object o, string propertyName, object value)
        {
            o.GetType().GetProperty(propertyName).SetValue(o, value,null);
        }

        public static T GetValue<T>(this object o, string propertyName)
        {
            return (T)o.GetType().GetProperty(propertyName).GetValue(o, null);
        }



        public static bool CanBeCastTo<T>(this object o)
        {
            return o.GetType().CanBeCastTo<T>();
        }

        public static bool CanBeCastTo<T>(this Type t)
        {
            return typeof(T).IsAssignableFrom(t);
        }

        public static Maybe<TypeConverterAttribute> MayHaveConverter(this Type type)
        {
            return type.GetCustomAttributes(true).MaybeFirst(o => o is TypeConverterAttribute).Cast<TypeConverterAttribute>();
        }
    }
}