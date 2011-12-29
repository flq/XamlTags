using System;
using DynamicXaml.ResourcesSystem;
using NUnit.Framework;
using FluentAssertions;
using XamlAppForTesting;

namespace XamlTags.Tests.Resources
{
    public class DataTemplateServiceTests : ResourceSystemTestContext
    {
        private DataTemplateService _svc;

        protected override void FixtureSetup()
        {
            _svc = new DataTemplateService(new ResourceService(_resourceLoader));
        }

        [Test]
        public void template_is_found_based_on_key()
        {
            var dt = _svc.Get("person");
            dt.HasValue.Should().BeTrue();
        }

        [Test]
        [TestCase(typeof(Person), "person")]
        [TestCase(typeof(Employee), "employee")]
        [TestCase(typeof(Slave), "person")]
        [TestCase(typeof(Boss), "employee")]
        [TestCase(typeof(ILifeform), "lifeform")]
        public void template_is_found_based_on_type(Type modelType, object key)
        {
            var dt = _svc.GetForObject(modelType);
            var dt2 = _svc.Get(key);
            dt.HasValue.Should().BeTrue("Type-based search must return known DataTemplate");
            dt.Value.Should().BeSameAs(dt2.Value);
            dt.HasValue.Should().BeTrue();
        }
    }
}