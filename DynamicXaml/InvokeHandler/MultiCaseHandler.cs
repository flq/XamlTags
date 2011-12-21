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

        public void Handle(InvokeContext ctx)
        {
            var names = ctx.Name.Split(new string[] { "And" }, StringSplitOptions.RemoveEmptyEntries);

            var propType = ctx.XamlType.GetPropertyTypeProvider();

            names.Zip(ctx.Values, (s, o) => new { PropName = s, Value = o })
                .Select(a => new SetterContext(a.PropName, propType(a.PropName), a.Value))
                .ForEach(ctx.AddSetterWith);
        }
    }
}