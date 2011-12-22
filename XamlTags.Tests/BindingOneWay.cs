using System.Windows.Controls;
using NUnit.Framework;
using FluentAssertions;

namespace XamlTags.Tests
{
    [TestFixture]
    public class BindingOneWay : XamlUsageContext<TextBox>
    {
        protected override object GetDataContext()
        {
            return new ModelForBindingReadOnly("Hello World");
        }

        [Test]
        public void one_way_binding_can_be_specified()
        {
            _xaml.IsEnabled(false).OneWayBindText("Text");
            Object.Text.Should().Be("Hello World");
                
        }
    }
}