using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace DynamicXaml.MarkupSystem
{
	public class DataTemplates : List<DataTemplate>
	{
		internal DataTemplate GetMatchFor(Type objectType)
		{
			var dataTemplate = this.FirstOrDefault(t => MatchViaDataType(t, objectType));
			return dataTemplate;
		}

		private static bool MatchViaDataType(DataTemplate arg, Type objectType)
		{
			var type = arg.DataType as Type;
			return type != null && type.IsAssignableFrom(objectType);
		}
	}
}
