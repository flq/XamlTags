using System;
using System.Windows.Markup;
using DynamicXaml.Extensions;
using System.Linq;

namespace DynamicXaml
{
    public class AddInvokeHandler : InvokeMemberHandler
    {
        public bool CanHandle(InvokeContext callContext)
        {
            return callContext.Name.InvariantEquals("Add");
        }

        public void Handle(InvokeContext ctx)
        {
            FailIfIAddChildIsMissing(ctx);

            var values = ctx.Values.Select(v => ctx.NormalizeToBuiltXaml(c => v).MustHaveValue()).Flatten();

            foreach (var value in values)
            {
                var v = value;
                ctx.AddSetterWith<IAddChild>(ac =>
                                                 {
                                                     if (v.CanBeCastTo<string>())
                                                         ac.AddText((string) v);
                                                     else
                                                         ac.AddChild(v);
                                                 });
            }
        }

        private static void FailIfIAddChildIsMissing(InvokeContext ctx)
        {
            if (!ctx.XamlType.CanBeCastTo<IAddChild>())
                throw new NotSupportedException("Type {0} does not support IAddChild interface, hence Add is not supported.".Fmt(ctx.XamlType.Name));
        }
    }
}