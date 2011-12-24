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

            var propertyName = ctx.Name.Replace("Static", "");
            var propertyType = ctx.XamlType.GetPropertyType(propertyName);
            ctx.AddSetterWith(new SetterContext(propertyName, propertyType, new StaticResource(ctx.Values[0])));
        }
    }

    public class StaticResource
    {
        private readonly string _key;

        public StaticResource(object o)
        {
            _key = o.ToString();
        }

        public static implicit operator string(StaticResource r)
        {
            return r.Key;
        }

        public string Key
        {
            get { return _key; }
        }
    }
}