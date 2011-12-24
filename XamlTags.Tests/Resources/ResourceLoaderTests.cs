using System.Windows;
using DynamicXaml.ResourcesSystem;
using NUnit.Framework;
using XamlAppForTesting;
using System.Linq;
using FluentAssertions;
using DynamicXaml.Extensions;

namespace XamlTags.Tests.Resources
{
    [TestFixture]
    public class ResourceLoaderTests
    {
        private ResourceLoader _resourceLoader;

        [TestFixtureSetUp]
        public void Given()
        {
            if (Application.Current == null) new Application(); //Awesome, require to make this subsystem work
            _resourceLoader = new ResourceLoader(typeof(MainWindow).Assembly);
        }

        [Test]
        public void finds_all_known_resource_dictionaries()
        {
            var names = _resourceLoader.GetResourceNames().ToList();
            names.Should().HaveCount(5);
        }

        [Test]
        public void a_resource_dictionary_asks_its_childs()
        {
            var dict = _resourceLoader.GetDictionary("RedYellowDictionary.xaml").MustHaveValue();
            dict["red"].Should().NotBeNull();
            dict["yellow"].Should().NotBeNull();
        }

        [Test]
        public void unknown_dictionary_is_none_maybe()
        {
            var dict = _resourceLoader.GetDictionary("Sabloey.xaml");
            dict.Should().Be(Maybe<ResourceDictionary>.None);
        }

        [Test]
        public void loading_a_non_dictionary_resource_returns_none()
        {
            var dict = _resourceLoader.GetDictionary("MainWindow.xaml");
            dict.Should().Be(Maybe<ResourceDictionary>.None);
        }
    }
}