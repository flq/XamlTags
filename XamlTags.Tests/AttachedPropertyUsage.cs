using System;
using System.Windows.Controls;
using NUnit.Framework;
using FluentAssertions;

namespace XamlTags.Tests
{
    [TestFixture]
    public class AttachedPropertyUsage : XamlUsageContext<Image>
    {
        [Test]
        public void attached_properties_are_applied()
        {
            _xaml
                .Attach(Grid.RowProperty, 2)
                .Attach(Grid.ColumnProperty, 3);

            Grid.GetRow(Object).Should().Be(2);
            Grid.GetColumn(Object).Should().Be(3);
        }

        [Test]
        public void Fails_if_incorrect_count()
        {
            AssertException(() => _xaml.Attach(Grid.RowProperty));
        }

        [Test]
        public void fails_if_wrong_signature()
        {
            AssertException(() => _xaml.Attach("Foo", 3));
        }

        [Test]
        public void fails_if_unacceptable_value()
        {
            AssertException(() => _xaml.Attach(Grid.RowProperty, new StackPanel()));
        }

        private static void AssertException(TestDelegate action)
        {
            Assert.Throws<InvalidOperationException>(action)
                .Message.Should().Contain("Signature").And.Contain("Attach(DependencyProperty, T value)");
        }
    }
}