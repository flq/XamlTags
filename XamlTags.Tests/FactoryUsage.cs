using System.Windows.Controls;
using DynamicXaml;
using NUnit.Framework;
using FluentAssertions;

namespace XamlTags.Tests
{
    [TestFixture]
    public class FactoryUsage
    {
        private Button _produce1;
        private Button _produce2;

        [TestFixtureSetUp]
        public void Given()
        {
            var b = new XamlBuilder();
            XamlFactory<Button> f =
                b.Start<Button>()
                .MinWidth(100d)
                .BindContent("Text")
                .CreateFactory();

            _produce1 = f.Create(new ModelForBinding { Text = "A" });
            _produce2 = f.Create(new ModelForBinding { Text = "B" });
        }

        [Test]
        public void produces_are_not_null()
        {
            _produce1.Should().NotBeNull();
            _produce2.Should().NotBeNull();
        }

        [Test]
        public void produces_are_not_the_same()
        {
            _produce1.Should().NotBeSameAs(_produce2);
        }

        [Test]
        public void buttons_have_the_same_width()
        {
            Assert.AreEqual(100d, _produce1.MinWidth);
            Assert.AreEqual(100d, _produce2.MinWidth);
        }

        [Test]
        public void produces_are_bound_to_correct_object()
        {
            _produce1.Content.Should().Be("A");
            _produce2.Content.Should().Be("B");
        }
    }
}