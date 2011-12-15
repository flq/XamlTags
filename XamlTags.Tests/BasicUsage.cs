using System;
using System.Windows.Controls;
using NUnit.Framework;
using FluentAssertions;

namespace XamlTags.Tests
{
    [TestFixture]
    public class BasicUsage : XamlUsageContext<Image>
    {
        [Test]
        public void created_object_is_image()
        {
            Object.Should().BeAssignableTo<Image>();
        }

        [Test]
        public void set_prop_with_primitive()
        {
            _xaml.MinWidth(100d);
            IsEqual(o => o.MinWidth, 100d);
        }

        [Test]
        public void set_two_props_with_primitive()
        {
            _xaml.Width(200d).Height(300d);
            IsEqual(o => o.Width, 200d);
            IsEqual(o => o.Height, 300d);
        }

        [Test]
        public void set_two_props_together()
        {
            _xaml
                .WidthAndHeight(200d, 300d)
                .StretchDirection(StretchDirection.DownOnly);
            IsEqual(o => o.StretchDirection, StretchDirection.DownOnly);
            IsEqual(o => o.Width, 200d);
            IsEqual(o => o.Height, 300d);
        }

        [Test]
        public void understandable_exception_with_non_existing_property()
        {
            var ex = Assert.Throws<ArgumentException>(() => _xaml.FooLatl("free!"));
            ex.Message.Contains("Image").Should().BeTrue();
            ex.Message.Contains("FooLatl").Should().BeTrue();
        }
    }
}