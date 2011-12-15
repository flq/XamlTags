using System;
using NUnit.Framework;
using XamlTag;

namespace XamlTags.Tests
{
    public class XamlUsageContext<T>
    {
        private readonly XamBuilder _builder = new XamBuilder();
        protected dynamic _xaml;

        [SetUp]
        public void Given()
        {
            _xaml = _builder.Start<T>();
        }

        public T Object { get { return _xaml.Create(); } }

        protected void IsEqual<Z>(Func<T,Z> selector, Z value)
        {
            Assert.AreEqual(value, selector(Object));
        }
    }
}