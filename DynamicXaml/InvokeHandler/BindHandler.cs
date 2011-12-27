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

            ctx.AddSetterWith(ctx.NewBindSetterContext(depProp));
        }

        private static void FailIfXamlNotAFrameworkElement(InvokeContext ctx)
        {
            if (!ctx.XamlType.CanBeCastTo<FrameworkElement>())
                throw new NotSupportedException("Type {0} is not assignable to FrameworkElement, Binding is therefore not supported".Fmt(ctx.XamlType.Name));
        }
    }
}