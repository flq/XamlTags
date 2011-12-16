using System.Windows.Controls;
using NUnit.Framework;
using FluentAssertions;

namespace XamlTags.Tests
{
    [TestFixture]
    public class BindingUsage : XamlUsageContext<TextBlock>
    {
        protected override object GetDataContext()
        {
            return new Model { Text = "Hello World" };
        }

        [Test]
        public void the_data_context_is_set()
        {
            Object.DataContext.Should().BeAssignableTo<Model>();
        }

        [Test]
        public void the_binding_is_set()
        {
            _xaml.BindText("Text");
            Object.Text.Should().Be("Hello World");
        }
    }

    public class Model
    {
        public string Text { get; set; }
    }
}