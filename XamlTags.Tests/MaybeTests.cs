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
    }
}