using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using DynamicXaml;
using NUnit.Framework;
using FluentAssertions;

namespace XamlTags.Tests
{
    [TestFixture]
    public class AddUsage : XamlUsageContext<TextBlock>
    {
        [Test]
        public void add_not_supported_on_objects_without_IAddChild()
        {
            Assert
                .Throws<NotSupportedException>(() => _builder.Start<FrameworkElement>().Add(new Rectangle()))
                .Message.Should().Contain("IAddChild").And.Contain("FrameworkElement");
        }

        [Test]
        public void correct_invocation_of_add_child()
        {
            _xaml.Add("Hello World");
            Object.Text.Should().Be("Hello World");
        }

        [Test]
        public void correct_invocation_of_add_object()
        {
            Button b = _builder.Start<Button>().Add(new Rectangle()).Create();
            b.Content.Should().BeAssignableTo<Rectangle>();
        }

        [Test]
        public void multiple_arguments_are_fed_to_the_add()
        {
            StackPanel sp = _builder.Start<StackPanel>().Add(new Rectangle(), new Button()).Create();
            sp.Children.Should().HaveCount(2);
            sp.Children[0].Should().BeAssignableTo<Rectangle>();
            sp.Children[1].Should().BeAssignableTo<Button>();
        }

        [Test]
        public void nested_syntax_works_on_add()
        {
            Button button = _builder.Start<Button>().Add(X.N(b => b.Start<TextBlock>().Add("Hello"))).Create();
            button.Content.Should().BeAssignableTo<TextBlock>();
            ((TextBlock)button.Content).Text.Should().Be("Hello");
        }

        [Test]
        public void many_itemed_arguments_are_flattened()
        {
            StackPanel sp = _builder.Start<StackPanel>()
                .Add(X.NM(b => new Xaml[] {
                          b.Start<TextBlock>().Add("Hello"),
                          b.Start<Image>()
                     }))
                .Create();
            sp.Children.Should().HaveCount(2);
            sp.Children[0].Should().BeAssignableTo<TextBlock>();
            sp.Children[1].Should().BeAssignableTo<Image>();
        }
    }
}