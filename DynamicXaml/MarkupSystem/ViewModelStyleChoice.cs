using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace DynamicXaml.MarkupSystem
{
    [ContentProperty("Styles")]
    public class ViewModelStyleChoice : StyleSelector
    {
        public ViewModelStyleChoice()
        {
            Styles = new Styles();
        }

        public Binding ViewModelAccess { get; set; }

        public Styles Styles { get; private set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            var style = Styles.GetMatchFor(item.GetType());
            return style ?? base.SelectStyle(item, container);
        }
    }

    public class Styles : List<StyleHolder>
    {
        internal Style GetMatchFor(Type viewModelType)
        {
            var style = this.FirstOrDefault(t => viewModelType.Equals(t.MatchViewModelType));
            return style.Style;
        }  
    }

    [ContentProperty("Style")]
    public class StyleHolder
    {
        public Type MatchViewModelType { get; set; }
        public Style Style { get; set; }
    }
}