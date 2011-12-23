using System;
using System.Windows;
using DynamicXaml.Extensions;

namespace DynamicXaml
{
    public class BindHandler : InvokeMemberHandler
    {
        public bool CanHandle(InvokeContext callContext)
        {
            return callContext.Name.StartsWithAnyOf("Bind", "OneWayBind");
        }

        public void Handle(InvokeContext ctx)
        {
            FailIfXamlNotAFrameworkElement(ctx);
            var propertyName = ctx.Name.Replace("OneWayBind", "").Replace("Bind", "");
            var depProp = ctx.XamlType
                .FindDependencyProperty(propertyName)
                .MustHaveValue(new ArgumentException("No DependencyProperty '{0}' found on type '{1}'".Fmt(propertyName, ctx.XamlType.Name)));

            ctx.AddSetterWith(NewBindSetterContext(ctx, depProp));
        }

        private static BindSetterContext NewBindSetterContext(InvokeContext ctx, DependencyProperty depProp)
        {
            var bc = new BindSetterContext(ctx.Values[0] != null ? ctx.Values[0].ToString() : ".", depProp);
            var value = ctx.GetValueForArgumentName("converter");
            if (value != null)
                bc.Add("converter", value);
            if (ctx.Name.StartsWith("OneWay"))
                bc.Add("oneway", true);
            return bc;
        }

        private static void FailIfXamlNotAFrameworkElement(InvokeContext ctx)
        {
            if (!ctx.XamlType.CanBeCastTo<FrameworkElement>())
                throw new NotSupportedException("Type {0} is not assignable to FrameworkElement, Binding is therefore not supported".Fmt(ctx.XamlType.Name));
        }
    }
}