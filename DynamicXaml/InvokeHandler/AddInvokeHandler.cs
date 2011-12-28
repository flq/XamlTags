using System;
using System.Windows.Markup;
using DynamicXaml.Extensions;

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

            ctx.AddSetterWith<IAddChild>(ac =>
                                             {
                                                 var v = ctx.Values[0];
                                                 if (v.CanBeCastTo<string>())
                                                     ac.AddText((string)v);
                                                 else
                                                     ac.AddChild(v);
                                             });
        }

        private static void FailIfIAddChildIsMissing(InvokeContext ctx)
        {
            if (!ctx.XamlType.CanBeCastTo<IAddChild>())
                throw new NotSupportedException("Type {0} does not support IAddChild interface, hence Add is not supported.".Fmt(ctx.XamlType.Name));
        }
    }
}