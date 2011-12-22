using System;
using System.Collections.Generic;
using System.Windows;
using DynamicXaml.Extensions;
using System.Linq;

namespace DynamicXaml
{
    public class BindSetterContext
    {
        public string Path { get; private set; }
        public DependencyProperty DependencyProperty { get; private set; }

        private readonly Dictionary<string,object> _additionalArguments = new Dictionary<string,object>();

        public BindSetterContext(string path, DependencyProperty property)
        {
            Path = path;
            DependencyProperty = property;
        }

        public void Add(string key, object value)
        {
            _additionalArguments.Add(key, value);
        }

        public Maybe<T> Get<T>() where T : class
        {
            return _additionalArguments.Values.OfType<T>().MaybeFirst();
        }

        public bool Get<T>(string key, out T value)
        {
            object val;
            var tryGetValue = _additionalArguments.TryGetValue(key, out val);
            if (tryGetValue)
              value = (T)val;
            else
              value = default(T);
            return tryGetValue;
        }
    }
}