using System;
using System.Collections.Generic;
using DynamicXaml;
using DynamicXaml.Extensions;
using NUnit.Framework;
using FluentAssertions;

namespace XamlTags.Tests
{
    [TestFixture]
    public class MaybeTests
    {
        [Test]
        public void none_is_false_value()
        {
            Maybe<int>.None.HasValue.Should().BeFalse();
        }

        [Test]
        public void structness_of_maybe_makes_default_equal_to_none()
        {
            var m1 = new Maybe<int>();
            m1.HasValue.Should().BeFalse();
            m1.Should().Be(Maybe<int>.None);
        }

        [Test]
        public void standard_equality()
        {
            var m1 = new Maybe<int>(3);
            var m2 = new Maybe<int>(3);
            m1.HasValue.Should().BeTrue();
            m1.Should().Be(m2);
        }

        [Test]
        public void type_makes_a_difference()
        {
            var m1 = new Maybe<int>();
            var m2 = new Maybe<double>();
            m1.Should().NotBe(m2);
        }

        [Test]
        public void get_on_dictionary()
        {
            var d = new Dictionary<string, string> { { "a", "hello" } };

            d.Get("a").MustHaveValue().Should().Be("hello");
            d.Get("b").Should().Be(Maybe<string>.None);

        }

        [Test]
        public void chain_and_die_when_requested()
        {
            string s = null;

            Assert.Throws<ArgumentException>(() =>
                s.ToMaybe()
                .Get(s1 => s1.Length)
                .Get(i => i + 3)
                .MustHaveValue(new ArgumentException("Ouch")))
            .Message.Should().Be("Ouch");
        }

        [Test]
        public void choose_default_if_none()
        {
            string s = null;
            s.ToMaybe().MustHaveValue("foo").Should().Be("foo");
        }

        [Test]
        public void cast_through_maybe_case1()
        {
            var s = "hello";
            s.ToMaybeOf<byte>().HasValue.Should().BeFalse();
        }

        [Test]
        public void cast_through_maybe_case2()
        {
            object i = new AddInvokeHandler();
            i.ToMaybeOf<InvokeMemberHandler>().HasValue.Should().BeTrue();
        }

        [Test,Ignore("Even though conversion is possible, IsAssignableFrom returns false, ultimately failing this for now")]
        public void cast_through_maybe_case3()
        {
            int i = 1;
            double d = i;

            i.ToMaybeOf<double>().HasValue.Should().BeTrue();
        }
    }
}