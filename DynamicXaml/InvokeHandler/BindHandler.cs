using System;
using System.Windows;
using DynamicXaml.Extensions;

namespace DynamicXaml
{
    public class BindHandler : InvokeMemberHandler
    {
        public bool CanHandle(InvokeContext callContext)
        {
            return callContext.Name.StartsWith("Bind");
        }

        public void Handle(InvokeContext ctx)
        {
            FailIfXamlNotAFrameworkElement(ctx);
            var propertyName = ctx.Name.Replace("Bind", "");
            var depProp = ctx.XamlType
                .FindDependencyProperty(propertyName)
                .MustHaveValue(new ArgumentException("No DependencyProperty '{0}' found on type '{1}'".Fmt(propertyName, ctx.XamlType.Name)));

            ctx.AddSetterWith(NewBindSetterContext(ctx, depProp));
        }

        private static BindSetterContext NewBindSetterContext(InvokeContext ctx, DependencyProperty depProp)
        {
            var bc = new BindSetterContext(ctx.Values[0].ToString(), depProp);
            var value = ctx.GetValueForArgumentName("converter");
            if (value != null)
                bc.Add("converter", value);
            return bc;
        }

        private static void FailIfXamlNotAFrameworkElement(InvokeContext ctx)
        {
            if (!ctx.XamlType.CanBeCastTo<FrameworkElement>())
                throw new InvalidOperationException("Type {0} is not assignable to FrameworkElement, Binding is therefore not supported".Fmt(ctx.XamlType.Name));
        }
    }
}