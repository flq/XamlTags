using System;
using System.Collections.Generic;
using System.Windows;
using DynamicXaml.Extensions;

namespace DynamicXaml
{
    public class CreationModel<T> : IActionRecorder<T>
    {
        private readonly List<Action<T>> _actions = new List<Action<T>>();

        public CreationModel(object dataContext = null)
        {
            if (dataContext != null)
                _actions.Add(xaml => SetContextAction(xaml, dataContext));
        }

        public void AddSetter(Action<T> setter)
        {
            _actions.Add(setter);
        }

        public T Play(T @object)
        {
            foreach (var a in _actions)
                a(@object);
            return @object;
        }

        void IActionRecorder<T>.Add(Action<T> action)
        {
            AddSetter(action);
        }

        private static void SetContextAction(T xaml, object dataContext)
        {
            (xaml as FrameworkElement)
                .ToMaybe()
                .Do(fe => fe.DataContext = dataContext);
        }
    }
}