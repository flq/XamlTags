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

        public void Handle(InvokeContext callContext)
        {
            var propertyName = callContext.Name;
            var propertyType = callContext.XamlType.GetPropertyType(propertyName);
            callContext.AddSetterWith(new SetterContext(propertyName, propertyType, callContext.Values[0]));
        }
    }
}