using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using FluentAssertions;
using NUnit.Framework;
using DynamicXaml.MarkupSystem;
using System.Linq;
using DynamicXaml.Extensions;

namespace XamlTags.Tests
{
    [TestFixture]
    public class BindingIdentificationTests
    {
        [Test]
        public void Dep_prop_identification_performance()
        {
            var fw = new UserControl();
            var l = fw.GetDependencyProperties().ToList();
            new Action(()=>
            {
              for (int i = 0; i < 500; i++)
                l = fw.GetDependencyProperties().ToList();
            }).ExecutionTime().ShouldNotExceed(TimeSpan.FromMilliseconds(180));
            Debug.WriteLine(l);
        }

        [Test]
        public void find_all_bindings()
        {
            var dataContext = new Data();
            var tb = new TextBox { DataContext = dataContext };
            tb.SetBinding(UIElement.VisibilityProperty, "IsVisible");
            tb.SetBinding(TextBox.TextProperty, "Text");
            var bndg = tb.GetBindings();
            bndg.Should().HaveCount(2);
        }

        public class Data
        {
            public bool IsVisible { get; set; }
            public string Text { get; set; }
        }
    }
}