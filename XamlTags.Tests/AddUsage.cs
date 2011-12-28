using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
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
    }
}