using System.Windows.Markup;
using System.Windows.Controls;
using System.Windows;

namespace DynamicXaml.MarkupSystem
{
	[ContentProperty("Templates")]
	public class DataTemplateChoice : DataTemplateSelector
	{
		public DataTemplateChoice()
		{
			Templates = new DataTemplates();
		}

		public DataTemplates Templates { get; private set; }

		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			var dt = Templates.GetMatchFor(item.GetType()) ?? base.SelectTemplate(item, container);
			return dt;
		}
	}
}
