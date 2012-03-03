using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Markup.Primitives;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Xps.Serialization;
using DynamicXaml.Extensions;
using System.Linq;

namespace DynamicXaml.MarkupSystem
{
    public static class WpfExtensions
    {
        public static Maybe<T> GetVisualParent<T>(this DependencyObject obj) where T : DependencyObject
        {
            while (obj != null)
            {
                if (obj is T)
                    return ((T)obj).ToMaybe();
                obj = VisualTreeHelper.GetParent(obj);
            }
            return Maybe<T>.None;
        }

        /// <summary>
        /// Put an activity to be executed by the dispatcher after the execution of the current frame
        /// </summary>
        public static void Queue<T>(this Dispatcher dispatcher, T input, Action<T> activity)
        {
            ThreadPool.QueueUserWorkItem(arg =>
            {
                var typed = (T)arg;
                dispatcher.Invoke(DispatcherPriority.Normal,(Action)(() => activity(typed)));
            }, input);
        }

        /// <summary>
        /// Render a UIElement such that the visual tree is generated, without actually displaying the UIElement
        /// anywhere
        /// </summary>
        public static void CreateVisualTree(this UIElement element)
        {
            var fixedDoc = new FixedDocument();
            var pageContent = new PageContent();
            var fixedPage = new FixedPage();
            fixedPage.Children.Add(element);
            pageContent.ToMaybeOf<IAddChild>().Do(c => c.AddChild(fixedPage));
            fixedDoc.Pages.Add(pageContent);

            var f = new XpsSerializerFactory();
            var w = f.CreateSerializerWriter(new MemoryStream());
            w.Write(fixedDoc);
        }

        /// <summary>
        /// List all dependency properties of a given dependency object
        /// </summary>
        public static IEnumerable<DependencyProperty> GetDependencyProperties(this DependencyObject element)
        {
            return MarkupWriter.GetMarkupObjectFor(element)
                .Properties.Where(p => p.DependencyProperty != null)
                .Select(p => p.DependencyProperty);
        }

        /// <summary>
        /// List all attached properties of a given dependency object
        /// </summary>
        public static IEnumerable<DependencyProperty> GetAttachedProperties(this DependencyObject element)
        {
            return MarkupWriter.GetMarkupObjectFor(element)
                .Properties.Where(p => p.IsAttached)
                .Select(p => p.DependencyProperty);
        }

        public static IEnumerable<BindingBase> GetBindings(this DependencyObject element)
        {
            var dpList = new List<DependencyProperty>(element.GetDependencyProperties().Concat(element.GetAttachedProperties()));
            return dpList.Select(dp => BindingOperations.GetBindingBase(element, dp)).Where(bb => bb != null);
        }
    }
}