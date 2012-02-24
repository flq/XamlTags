using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using DynamicXaml.Extensions;

namespace DynamicXaml.MarkupSystem
{
    public interface IHandleFocus
    {
        void HandleFocus();
    }

    public static class FocusBehavior
    {
        /// <summary>
        /// Dependency property to focus a frmework element. Will honor an implementation of <see cref="IHandleFocus"/>
        /// </summary>
        public static readonly DependencyProperty IsFocusedProperty =
        DependencyProperty.RegisterAttached("IsFocused", typeof(bool?),
                typeof(FocusBehavior), new FrameworkPropertyMetadata(IsFocusedChanged));

        public static bool? GetIsFocused(DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            return (bool?)element.GetValue(IsFocusedProperty);
        }

        public static void SetIsFocused(DependencyObject element, bool? value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            element.SetValue(IsFocusedProperty, value);
        }

        #region Event Handlers
        /// <summary>
        /// Determines whether the value of the dependency property <c>IsFocused</c> has change.
        /// </summary>
        /// <param name="d">The dependency object.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void IsFocusedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Ensure it is a FrameworkElement instance.
            var fe = d as FrameworkElement;
            if (fe != null && e.OldValue == null && e.NewValue != null && (bool)e.NewValue)
            {
                // Attach to the Loaded event to set the focus there. If we do it here it will
                // be overridden by the view rendering the framework element.
                fe.Loaded += FrameworkElementLoaded;
            }
        }
        /// <summary>
        /// Sets the focus when the framework element is loaded and ready to receive input.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private static void FrameworkElementLoaded(object sender, RoutedEventArgs e)
        {
            var fe = sender as FrameworkElement;
            if (fe == null) return;
            fe.Loaded -= FrameworkElementLoaded;
            FocusHelper.Focus(fe);
        }
        #endregion Event Handlers
    }

    static class FocusHelper
    {
        public static void Focus(UIElement uiElement)
        {
            uiElement.Dispatcher.Queue(uiElement, ui =>
            {
                if (ui is IHandleFocus)
                {
                    // Thing implements I handle focus
                    ((IHandleFocus)ui).HandleFocus();
                    return;
                }

                ui.Focus();
                Keyboard.Focus(ui);
                ui.ToMaybeOf<TextBoxBase>().Do(tb => tb.SelectAll());
            });
        }
    }
}
