using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace DynamicXaml.MarkupSystem
{
    /// <summary>
    /// Functionality to recursively search through a visual tree to find some item.
    /// </summary>
    public class VisualScanner
    {
        private readonly DependencyObject _root;

        /// <summary>
        /// Root of the visual tree to be searched
        /// </summary>
        /// <param name="root"></param>
        public VisualScanner(DependencyObject root)
        {
            _root = root;
        }

        /// <summary>
        /// Returns first element assignable to T
        /// </summary>
        public T First<T>()  where T : DependencyObject
        {
            return First<T>(_ => true);
        }

        /// <summary>
        /// Returns first element of type T that satisfies the condition
        /// </summary>
        public T First<T>(Func<T,bool> predicate) where T : DependencyObject
        {
            return Where(predicate).FirstOrDefault();
        }

        /// <summary>
        /// Return all elements assignable to T
        /// </summary>
        public IEnumerable<T> All<T>() where T : DependencyObject
        {
            return Where<T>(_=>true);
        }

        /// <summary>
        /// Return all elements assignable to T that satisfy the condition
        /// </summary>
        public IEnumerable<T> Where<T>(Func<T, bool> predicate) where T : DependencyObject
        {
            return Search<T>().Where(predicate);
        }


        private IEnumerable<T> Search<T>() where T : DependencyObject
        {
            var workStack = GetWorkStack();
            while (workStack.Count > 0)
            {
                var next = workStack.Pop();
                if (next is T)
                    yield return (T)next;
                GetAllChildsOf(next, workStack);
            }
        }

        private static IEnumerable<DependencyObject> GetAllChildsOf(DependencyObject element)
        {
            var c = VisualTreeHelper.GetChildrenCount(element);
            for (int i = 0; i < c; i++)
                yield return VisualTreeHelper.GetChild(element, i);
        }

        private Stack<DependencyObject> GetWorkStack()
        {
            var workStack = new Stack<DependencyObject>();
            workStack.Push(_root);
            return workStack;
        }

        private static void GetAllChildsOf(DependencyObject element, Stack<DependencyObject> workStack)
        {
            foreach (var child in GetAllChildsOf(element))
                workStack.Push(child);
        }
    }
}