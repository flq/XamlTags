using System;
using DynamicXaml.Extensions;

namespace DynamicXaml
{
    internal class StandardSetterProvider : SetterProvider
    {
        public bool Match(SetterContext ctx)
        {
            return ctx.PropertyType.IsAssignableFrom(ctx.Value.GetType());
        }

        public Action<T> Setter<T>(SetterContext ctx)
        {
            return xaml => xaml.SetValue(ctx.PropertyName, ctx.Value);
        }
    }
}