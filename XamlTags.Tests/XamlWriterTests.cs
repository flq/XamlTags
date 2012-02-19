using System.Diagnostics;
using System.IO;
using System.Text;
using DynamicXaml.MarkupSystem;
using FluentAssertions;
using NUnit.Framework;

namespace XamlTags.Tests
{
    [TestFixture]
    public class XamlWriterTests
    {
        private StringBuilder _builder;
        private StringWriter _sw;

        public string GeneratedOutput
        {
            get
            {
                return _builder.ToString();
            }
        }

        [SetUp]
        public void Given()
        {
            _builder = new StringBuilder();
            _sw = new StringWriter(_builder);
        }

        [TearDown]
        public void AfterEachTest()
        {
            Debug.WriteLine(GeneratedOutput);
        }

        [Test]
        public void default_generates_empty_content_control()
        {
            var writer = new XamlWriter(_sw);
            writer.Dispose();
            GeneratedOutput.Should().StartWith("<ContentControl");
        }

        [Test]
        public void root_control_can_be_overridden()
        {
            var writer = new XamlWriter(_sw, new XamlWriterSettings(cfg => cfg.SetRootElement("StackPanel")));
            writer.Dispose();
            GeneratedOutput.Should().StartWith("<StackPanel");
        }

        [Test]
        public void root_control_can_be_custom_control()
        {
            var writer = new XamlWriter(_sw, new XamlWriterSettings(cfg => cfg.AddReference("foo", "FooNameSpace").SetRootElement("foo:FooPanel")));
            writer.Dispose();
            GeneratedOutput.Should().StartWith("<foo:FooPanel");
        }

        [Test]
        public void additional_namespaces_can_be_specified()
        {
            var writer = new XamlWriter(_sw, new XamlWriterSettings(cfg => cfg.AddReference("foo", "Acme.SuperNameSpace", "Acme.SuperAssembly")));
            writer.Dispose();
            GeneratedOutput.Should().Contain(@"xmlns:foo=""clr-namespace:Acme.SuperNameSpace;assembly=Acme.SuperAssembly""");
        }

        [Test]
        public void add_nested_element()
        {
            var writer = new XamlWriter(_sw);
            writer.RegisterReference("query", "abc");
            writer.StartElement("query:Viewer", w => w.AddAttribute("Value", "{Binding}"));
            writer.Dispose();
            GeneratedOutput.Should().Contain("  <query:Viewer");
        }
    }
}