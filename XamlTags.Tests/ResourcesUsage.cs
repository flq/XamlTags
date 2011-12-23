using System;
using System.Collections;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using DynamicXaml;
using NUnit.Framework;
using FluentAssertions;

namespace XamlTags.Tests
{
    [TestFixture]
    public class ResourcesUsage : XamlUsageContext<Button>
    {
        [Test]
        public void adding_a_resource_to_not_fwelement_is_not_allowed()
        {
            var x = Assert.Throws<NotSupportedException>(()=> _builder.Start<Storyboard>().AddResource("string", "Hello"));
            x.Message.Should().Contain("FrameworkElement").And.Contain("Resource");
        }

        static IEnumerable PossibleResourceValues
        {
            get
            {
                yield return X.N(b => b.Start<SolidColorBrush>().Color("Green"));
                yield return new SolidColorBrush(Colors.Green);
                var bld = new XamlBuilder();
                yield return bld.Start<SolidColorBrush>().Color("Green");
            }
        }

        [Test]
        [TestCaseSource("PossibleResourceValues")]
        public void adding_a_resource_is_possible(object value)
        {
            _xaml.AddResource("color", value);

            var thing = Object.TryFindResource("color");
            thing.Should().NotBeNull();
            ((SolidColorBrush)thing).Color.Should().Be(Colors.Green);
        }

        [Test,Ignore("Need more infrastructure to tell a good story here")]
        public void reference_a_resource()
        {
            _xaml
                .AddResource("color", new SolidColorBrush(Colors.Green))
                .Content(X.N(b => b.Start<Rectangle>().StaticFill("color")));

            var r = (Rectangle)Object.Content;
            ((SolidColorBrush)r.Fill).Color.Should().Be(Colors.Green);
        }
    }
}