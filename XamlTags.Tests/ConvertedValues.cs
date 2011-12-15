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
    }
}