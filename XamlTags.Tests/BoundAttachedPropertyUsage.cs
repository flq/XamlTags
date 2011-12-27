using System.Windows.Controls;
using NUnit.Framework;
using FluentAssertions;

namespace XamlTags.Tests
{
    [TestFixture]
    public class BoundAttachedPropertyUsage : XamlUsageContext<Button>
    {
        protected override object GetDataContext()
        {
            return new ModelForBindingRow(3);
        }

        [Test]
        public void attached_properties_can_be_bound()
        {
            _xaml.Attach(Grid.RowProperty, path: "Row");
            Grid.GetRow(Object).Should().Be(3);
        }
    }
}