﻿using System;
using DynamicXaml;
using NUnit.Framework;

namespace XamlTags.Tests
{
    public class XamlUsageContext<T>
    {
        protected readonly XamlBuilder _builder = new XamlBuilder();
        protected dynamic _xaml;

        [SetUp]
        public void Given()
        {
            var o = GetDataContext();
            _xaml = o != null ? _builder.Start<T>(o) : _builder.Start<T>();
            MoreGivens();
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

        protected virtual void MoreGivens()
        {
            
        }
    }
}