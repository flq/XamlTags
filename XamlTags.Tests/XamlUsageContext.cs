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
            var o = GetDataContext();
            _xaml = o != null ? _builder.Start<T>(o) : _builder.Start<T>();
        }

        public T Object { get { return _xaml.Create(); } }

        protected void IsEqual<Z>(Func<T,Z> selector, Z value)
        {
            Assert.AreEqual(value, selector(Object));
        }

        protected virtual object GetDataContext()
        {
            return null;
        }
    }
}