using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using NUnit.Framework;
using FluentAssertions;

namespace XamlTags.Tests
{
    [TestFixture]
    public class BindingUsage : XamlUsageContext<TextBlock>
    {
        protected override object GetDataContext()
        {
            return new ModelForBinding { Text = "Hello World" };
        }

        [Test]
        public void the_data_context_is_set()
        {
            Object.DataContext.Should().BeAssignableTo<ModelForBinding>();
        }

        [Test]
        public void binding_unacceptable_if_element_not_framework_element()
        {
            var x = Assert.Throws<NotSupportedException>(()=> _builder.Start<Storyboard>().BindSlipBehavior(SlipBehavior.Slip).Create());
            x.Message.Should()
                .Contain("FrameworkElement").And
                .Contain("Storyboard").And
                .Contain("Binding");
        }

        [Test]
        public void unknown_property_fails()
        {
            var x = Assert.Throws<ArgumentException>(() => _xaml.BindFoo("Text"));
            x.Message.Should()
                .Contain("TextBlock").And
                .Contain("Foo");
        }

        [Test]
        public void the_binding_is_set()
        {
            _xaml.BindText("Text");
            Object.Text.Should().Be("Hello World");
        }

        [Test]
        public void binding_with_a_converter()
        {
            _xaml.BindVisibility("Visible", converter: new BooleanToVisibilityConverter());
            Object.Visibility.Should().Be(Visibility.Collapsed);
        }
    }
}