using System;
using System.Linq;
using System.Windows;
using DynamicXaml.Extensions;

namespace DynamicXaml
{
    public static class InvokeContextExtensions
    {
        public static BindSetterContext NewBindSetterContext(this InvokeContext ctx, DependencyProperty depProp)
        {
            var bc = new BindSetterContext(ctx.Values[0] != null ? ctx.Values[0].ToString() : ".", depProp);
            var value = ctx.GetValueForArgumentName("converter");
            if (value != null)
                bc.Add("converter", value);
            if (ctx.Name.StartsWith("OneWay"))
                bc.Add("oneway", true);

            value = ctx.GetValueForArgumentName("oneway");
            if (value != null)
                bc.Add("oneway", (bool)value);

            return bc;
        }

        public static Maybe<object[]> NormalizeToBuiltXaml(this InvokeContext ctx, Func<InvokeContext,object> rootObjectSelector)
        {
            var value = rootObjectSelector(ctx);

            return value.Maybe(
                v => v.Cast<object[]>(),
                v => v.Cast<Func<XamlBuilder, Xaml>>()
                      .Get(func => func(ctx.Builder).Create())
                      .Get(obj => new[] { obj }),
                v => v.Cast<Func<XamlBuilder, Xaml[]>>()
                      .Get(func => func(ctx.Builder))
                      .Get(xamls => xamls.Select(x => x.Create()).ToArray()),
                v => v.Get(obj => new [] { obj })
            );
        }
    }
}