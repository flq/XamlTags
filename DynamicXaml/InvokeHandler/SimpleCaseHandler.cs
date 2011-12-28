using System;
using DynamicXaml.Extensions;

namespace DynamicXaml
{
    /// <summary>
    /// End of the handling pipeline, anything that arrives here is treated as simple case
    /// </summary>
    public class SimpleCaseHandler : InvokeMemberHandler
    {
        public bool CanHandle(InvokeContext callContext)
        {
            return true;
        }

        public void Handle(InvokeContext ctx)
        {
            var propertyName = ctx.Name;
            var propertyType = ctx.XamlType.GetPropertyType(propertyName);
            var values = ctx.NormalizeToBuiltXaml(c => c.Values[0]).MustHaveValue();
            ctx.AddSetterWith(new SetterContext(propertyName, propertyType, values.Length > 1 ? values : values[0]));
        }
    }
}