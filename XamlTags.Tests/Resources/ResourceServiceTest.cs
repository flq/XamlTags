using System.Windows;
using System.Windows.Media;
using DynamicXaml.Extensions;
using DynamicXaml.ResourcesSystem;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;

namespace XamlTags.Tests.Resources
{
    public class ResourceServiceTest : ResourceSystemTestContext
    {
        private ResourceService _resourceService;

        protected override void FixtureSetup()
        {
            _resourceService = new ResourceService(_resourceLoader);
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

        [Test]
        public void find_all_data_templates()
        {
            var dt = _resourceService.Where(kv => kv.Value is DataTemplate).ToList();
            dt.Should().HaveCount(4);
        }
    }
}