using System;
namespace DynamicXaml
{
    public class X
    {
        /// <summary>
        /// Nested builder
        /// </summary>
        public static Func<XamlBuilder, Xaml> N(Func<XamlBuilder, Xaml> map)
        {
            return map;
        }

        /// <summary>
        /// Many-Nested builder
        /// </summary>
        public static Func<XamlBuilder, Xaml[]> NM(Func<XamlBuilder, Xaml[]> map)
        {
            return map;
        }
    }
}