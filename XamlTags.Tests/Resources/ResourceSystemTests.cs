using System;
using System.Windows;
using NUnit.Framework;
using System.Linq;
using FluentAssertions;
using DynamicXaml.Extensions;
using XamlAppForTesting;

namespace XamlTags.Tests.Resources
{
    [TestFixture]
    public class ResourceSystemTests : ResourceSystemTestContext
    {
        [Test]
        public void finds_all_known_resources()
        {
            var names = _resourceLoader.GetResourceNames().ToList();
            names.Should().HaveCount(3);
        }

        [Test]
        public void loader_provides_all_known_dictionaries()
        {
            var rds = _resourceLoader.GetDictionaries();
            rds.Should().HaveCount(3);
        }

        [Test]
        public void resourceloader_does_not_instantiate_a_window()
        {
            MainWindow.BlowUpOnConstruction = true;
            try
            {
                var rds = _resourceLoader.GetDictionaries().ToList();
            }
            catch (Exception)
            {
                Assert.Fail("The loader instantiated the window");
            }
            finally
            {
                MainWindow.BlowUpOnConstruction = false;
            }
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