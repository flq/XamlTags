using System.Windows.Controls;
using NUnit.Framework;
using FluentAssertions;

namespace XamlTags.Tests
{
    [TestFixture]
    public class BindingDirectly : XamlUsageContext<TextBlock>
    {
        protected override object GetDataContext()
        {
            return "Hello World";
        }

        [Test]
        public void the_data_context_is_set()
        {
            Object.DataContext.Should().BeAssignableTo<string>();
        }

        [Test]
        public void binding_without_parameter_binds_to_datacontext()
        {
            _xaml.BindText();
            Object.Text.Should().Be("Hello World");
        }
    }
}