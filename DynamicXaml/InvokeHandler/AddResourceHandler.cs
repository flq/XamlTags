using System;
using System.Windows;
using DynamicXaml.Extensions;

namespace DynamicXaml
{
    public class AddResourceHandler : InvokeMemberHandler
    {
        public bool CanHandle(InvokeContext callContext)
        {
            return callContext.Name.Equals("AddResource");
        }

        public void Handle(InvokeContext ctx)
        {
            FailIfXamlNotAFrameworkElement(ctx);

            object value = null;
            ctx.Values[1].Maybe(
                v => v.Cast<Func<XamlBuilder, Xaml>>()
                    .Get(func => func(ctx.Builder).Create())
                    .Do(xaml => value = xaml),
                v => v.Cast<Xaml>()
                    .Get(x => x.Create())
                    .Do(xaml => value = xaml)
            );

            ctx.AddSetterWith<FrameworkElement>(fw => fw.Resources.Add(ctx.Values[0], value ?? ctx.Values[1]));
        }

        private static void FailIfXamlNotAFrameworkElement(InvokeContext ctx)
        {
            if (!ctx.XamlType.CanBeCastTo<FrameworkElement>())
                throw new NotSupportedException("Type {0} is not assignable to FrameworkElement, Add Resource is therefore not supported".Fmt(ctx.XamlType.Name));
        }
    }
}