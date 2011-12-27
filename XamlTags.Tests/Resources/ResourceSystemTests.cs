using System.Windows;
using System.Windows.Media;
using DynamicXaml.ResourcesSystem;
using NUnit.Framework;
using XamlAppForTesting;
using System.Linq;
using FluentAssertions;
using DynamicXaml.Extensions;

namespace XamlTags.Tests.Resources
{
    [TestFixture]
    public class ResourceSystemTests
    {
        private ResourceLoader _resourceLoader;
        private ResourceService _resourceService;

        [TestFixtureSetUp]
        public void Given()
        {
            if (Application.Current == null) new App(); //Awesome, require to make this subsystem work
            _resourceLoader = new ResourceLoader(typeof(MainWindow).Assembly);
            _resourceService = new ResourceService(_resourceLoader);
        }

        [Test]
        public void finds_all_known_resources()
        {
            var names = _resourceLoader.GetResourceNames().ToList();
            names.Should().HaveCount(5);
        }

        [Test]
        public void loader_provides_all_known_dictionaries()
        {
            var rds = _resourceLoader.GetDictionaries();
            rds.Should().HaveCount(4);
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

        [Test]
        public void res_service_existing_is_found()
        {
            var v = _resourceService.GetResource<SolidColorBrush>("red");
            v.HasValue.Should().BeTrue();
        }

        [Test]
        public void res_service_non_existing_is_none()
        {
            var v = _resourceService.GetResource<SolidColorBrush>("violet");
            v.Should().Be(Maybe<SolidColorBrush>.None);
        }

        [Test]
        public void res_service_wrong_cast_is_none()
        {
            var v = _resourceService.GetResource<int>("red");
            v.Should().Be(Maybe<int>.None);
        }
    }
}