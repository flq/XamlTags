using System;
using System.Windows;
using DynamicXaml.Extensions;

namespace DynamicXaml
{
    public class StaticResourceHandler : InvokeMemberHandler
    {
        public bool CanHandle(InvokeContext ctx)
        {
            return ctx.Name.StartsWith("Static");
        }

        public void Handle(InvokeContext ctx)
        {
            throw new NotImplementedException("Setting value StaticResource-Style is currently not supported");

            var propertyName = ctx.Name.Replace("Static", "");
            var propertyType = ctx.XamlType.GetPropertyType(propertyName);
            ctx.AddSetterWith(new SetterContext(propertyName, propertyType, null));
        }
    }
}