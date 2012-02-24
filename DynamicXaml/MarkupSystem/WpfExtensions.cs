using System;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using DynamicXaml.Extensions;

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
    }
}