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
            ctx.AddSetterWith(new SetterContext(propertyName, propertyType, ctx.Values[0]));
        }
    }
}