using System.Windows.Controls;
using NUnit.Framework;
using XamlTag;
using FluentAssertions;

namespace XamlTags.Tests
{
    [TestFixture]
    public class NestedUsage : XamlUsageContext<Button>
    {
        [Test]
        public void provided_nested_turns_up_in_object()
        {
            _xaml.Content(X.Nest(
                b => b.Start<Image>().Width(123d))
            );

            Object.Content.Should().BeAssignableTo<Image>();
            var img = (Image)Object.Content;
            Assert.AreEqual(123d, img.Width);
        }

        [Test]
        public void button_image_and_text()
        {
            var stackPanelContents = X.NestMany(b => new XamlCreator[]
                                                 {
                                                     b.Start<Image>().Width(124d),
                                                     b.Start<TextBlock>().Text("Hello")
                                                 });

            _xaml.Content(X.Nest(b => b.Start<StackPanel>().Children(stackPanelContents)));
        }
    }
}