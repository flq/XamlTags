using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Xps.Serialization;
using DynamicXaml.MarkupSystem;
using NUnit.Framework;
using XamlAppForTesting;
using FluentAssertions;
using DynamicXaml.Extensions;

namespace XamlTags.Tests
{
    [TestFixture]
    public class VisualScannerTests
    {
        private VisualScanner _scanner;

        [TestFixtureSetUp]
        public void Given()
        {
            var wdw = new WindowContents();
            wdw.CreateVisualTree();
            _scanner = new VisualScanner(wdw);
        }

        [Test]
        public void Can_enumerate_twice()
        {
            var brdr = _scanner.All<Border>();
            brdr.Should().HaveCount(2);
            brdr.Should().HaveCount(2, "Because method must be idempotent");
        }

        [Test]
        public void Finds_the_root()
        {
            var w = _scanner.First<UserControl>();
            w.Should().NotBeNull();
            w.Should().BeAssignableTo<WindowContents>();
        }

        [Test]
        public void Find_all_borders()
        {
            var brdr = _scanner.All<Border>();
            brdr.Should().NotBeNull();
            brdr.Should().HaveCount(2);
        }

        [Test]
        public void Find_red_border()
        {
            var brdr = _scanner.First<Border>(b => b.Background.ToMaybeOf<SolidColorBrush>().Get(brush => brush.Color == Colors.Red));
            brdr.Should().NotBeNull();
            brdr.Name.Should().Be("Red");
        }

        [Test]
        public void Not_finding_anything_returns_null()
        {
            var thing = _scanner.First<CheckBox>();
            thing.Should().BeNull();
        }

        [Test]
        public void Not_finding_anything_returns_empty_enumeration()
        {
            var thing = _scanner.All<CheckBox>();
            thing.Should().NotBeNull();
            thing.Should().HaveCount(0);
        }

    }
}