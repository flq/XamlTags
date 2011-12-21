using System.Windows.Controls;
using DynamicXaml;
using NUnit.Framework;
using FluentAssertions;

namespace XamlTags.Tests
{
    [TestFixture]
    public class NestedUsage : XamlUsageContext<Button>
    {
        [Test]
        public void provided_nested_turns_up_in_object()
        {
            _xaml.Content(X.N(b => b.Start<Image>().Width(123d)));

            Object.Content.Should().BeAssignableTo<Image>();
            var img = (Image)Object.Content;
            Assert.AreEqual(123d, img.Width);
        }

        [Test]
        public void button_image_and_text()
        {
            var stackPanelContents = X.NM(b => new Xaml[]
                                                 {
                                                     b.Start<Image>().Width(124d),
                                                     b.Start<TextBlock>().Text("Hello")
                                                 });
            _xaml.Content(X.N(b => b.Start<StackPanel>().Children(stackPanelContents)));

            var sp = (StackPanel)Object.Content;
            sp.Children.Should().HaveCount(2);
            Assert.AreEqual(124d, ((Image)sp.Children[0]).Width);
            Assert.AreEqual("Hello", ((TextBlock)sp.Children[1]).Text);
        }
    }
}