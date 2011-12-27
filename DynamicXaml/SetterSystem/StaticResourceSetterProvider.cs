using System;
using System.Windows;
using DynamicXaml.Extensions;

namespace DynamicXaml
{
    public class StaticResourceSetterProvider : SetterProvider
    {
        public bool Match(SetterContext ctx)
        {
            return ctx.Value.CanBeCastTo<StaticResource>();
        }

        public Action<T> Setter<T>(SetterContext ctx)
        {
            var v = (StaticResource)ctx.Value;
            return xaml => (xaml as FrameworkElement)
                .Maybe(m => m.Get(f => f.TryFindResource(v.Key))
                             .Do(obj => xaml.SetValue(ctx.PropertyName, obj)),
                       m => m.Get(f => ctx.Builder.ResourceService.GetResource<object>(v.Key))
                             .Do(obj => xaml.SetValue(ctx.PropertyName, obj)));
        }
    }
}