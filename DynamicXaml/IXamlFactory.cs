using System;
using System.Windows;
using DynamicXaml.Extensions;

namespace DynamicXaml
{
    public interface XamlFactory<T>
    {
        T Create(object dataContext);
    }

    class DefaultXamlFactory<T> : XamlFactory<T>
    {
        private readonly Func<T, T> _modifier;

        public DefaultXamlFactory(Func<T,T> modifier)
        {
            _modifier = modifier;
        }

        public T Create(object dataContext)
        {
            var obj = Activator.CreateInstance<T>();
            (obj as FrameworkElement).ToMaybe().Do(fw => fw.DataContext = dataContext);
            return _modifier(obj);
        }
    }
}