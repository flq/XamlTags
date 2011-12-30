using System;
using System.Windows;
using System.Windows.Controls;

namespace DynamicXaml.MarkupSystem
{
    public static partial class QuickGrid
    {
        public static readonly DependencyProperty ColumnsProperty = DependencyProperty.RegisterAttached(
          "Columns",
          typeof(string),
          typeof(QuickGrid),
          new PropertyMetadata(new PropertyChangedCallback(HandleChange))
        );

        public static readonly DependencyProperty RowsProperty = DependencyProperty.RegisterAttached(
          "Rows",
          typeof(string),
          typeof(QuickGrid),
          new PropertyMetadata(new PropertyChangedCallback(HandleChange))
        );

        public static readonly DependencyProperty WithProperty = DependencyProperty.RegisterAttached(
          "With",
          typeof(string),
          typeof(QuickGrid),
          new PropertyMetadata(new PropertyChangedCallback(HandleChange))
        );

        public static void SetColumns(Grid element, string value) { element.SetValue(ColumnsProperty, value); }
        public static string GetColumns(Grid element) { return (string)element.GetValue(ColumnsProperty); }

        public static void SetRows(Grid element, string value) { element.SetValue(RowsProperty, value); }
        public static string GetRows(Grid element) { return (string)element.GetValue(RowsProperty); }

        public static void SetWith(Grid element, string value) { element.SetValue(WithProperty, value); }
        public static string GetWith(Grid element) { return (string)element.GetValue(WithProperty); }

        private static void HandleChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var g = (Grid)d;
            var newValue = e.NewValue as string;
            if (string.IsNullOrWhiteSpace(newValue))
                return;

            if (g.ColumnDefinitions.Count > 0)
                g.ColumnDefinitions.Clear();
            if (g.RowDefinitions.Count > 0)
                g.RowDefinitions.Clear();

            if (e.Property.Equals(RowsProperty))
                AddRows(newValue, g);
            if (e.Property.Equals(ColumnsProperty))
                AddColumns(newValue, g);
            if (e.Property.Equals(WithProperty))
            {
                var values = newValue.Split('|');
                AddRows(values[0], g);
                AddColumns(values[1], g);
            }
        }

        private static void AddColumns(string newValue, Grid g)
        {
            foreach (var gl in new GridLengthBuilder(newValue).GetLengths())
                g.ColumnDefinitions.Add(new ColumnDefinition { Width = gl });
        }

        private static void AddRows(string newValue, Grid g)
        {
            foreach (var gl in new GridLengthBuilder(newValue).GetLengths())
                g.RowDefinitions.Add(new RowDefinition {Height = gl});
        }
    }
}