using System.Windows;
using System.Windows.Controls;
using NUnit.Framework;
using FluentAssertions;

namespace XamlTags.Tests
{
    [TestFixture]
    public class ConvertedValues : XamlUsageContext<Button>
    {
        [Test]
        public void thickness_converter_is_used()
        {
            _xaml.Margin("1,2,3,4");
            Object.Margin.Should().Be(new Thickness(1, 2, 3, 4));
        }

        [Test]
        public void converter_used_with_combined_settings()
        {
            _xaml.WidthAndHeightAndMargin(200d, 100d, "1,2,3,4");
            Object.Margin.Should().Be(new Thickness(1, 2, 3, 4));
            Assert.AreEqual(200d, Object.Width);
            Assert.AreEqual(100d, Object.Height);
        }
    }
}