using System;
using System.Linq;
using DynamicXaml.Extensions;

namespace DynamicXaml
{
    public class MultiCaseHandler : InvokeMemberHandler
    {
        public bool CanHandle(InvokeContext callContext)
        {
            return callContext.Name.Contains("And");
        }

        public void Handle(InvokeContext callContext)
        {
            var names = callContext.Name.Split(new string[] { "And" }, StringSplitOptions.RemoveEmptyEntries);

            var propType = callContext.XamlType.GetPropertyTypeProvider();

            names.Zip(callContext.Values, (s, o) => new { PropName = s, Value = o })
                .Select(a => new SetterContext(a.PropName, propType(a.PropName), a.Value))
                .ForEach(callContext.AddSetterWith);
        }
    }
}