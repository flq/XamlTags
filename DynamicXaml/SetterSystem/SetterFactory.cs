using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using DynamicXaml.Extensions;

namespace DynamicXaml
{
    internal class SetterFactory
    {
        private readonly List<SetterProvider> _knownSetterProvider = new List<SetterProvider>();

        public SetterFactory()
        {
            _knownSetterProvider.Add(new StandardSetterProvider());
            _knownSetterProvider.Add(new StaticResourceSetterProvider());
            _knownSetterProvider.Add(new SetterProviderFromConverter());
            _knownSetterProvider.Add(new ListSetterProvider());
        }

        public Action<T> GetSetter<T>(SetterContext ctx)
        {
            var setter = _knownSetterProvider
                .MaybeFirst(p => p.Match(ctx))
                .MustHaveValue(new NotSupportedException("Could not resolve a way to set {0} to target ({1}:{2})".Fmt(ctx.Value.GetType().Name, ctx.PropertyName, ctx.PropertyType.Name)));
            return setter.Setter<T>(ctx);
        }

        public Action<T> GetSetter<T>(BindSetterContext ctx)
        {
            return xaml =>
                       {
                           var fw = xaml.Cast<FrameworkElement>();
                           var b = new Binding(ctx.Path);
                           ctx.Get<IValueConverter>().Do(vc => b.Converter = vc);
                           bool oneway;
                           if (ctx.Get("oneway", out oneway) && oneway)
                               b.Mode = BindingMode.OneWay;
                           fw.SetBinding(ctx.DependencyProperty, b);
                       };
        }
    }
}