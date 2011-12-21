using System;
using DynamicXaml.Extensions;
using System.Linq;

namespace DynamicXaml
{
    public class NestedInvokeHandler : InvokeMemberHandler
    {
        public bool CanHandle(InvokeContext callContext)
        {
            var value = callContext.Values[0];
            return value.CanBeCastTo<Func<XamlBuilder, Xaml>>() || value.CanBeCastTo<Func<XamlBuilder, Xaml[]>>();
        }

        public void Handle(InvokeContext ctx)
        {
            ctx.Values[0].ToMaybe()
                .Cast<Func<XamlBuilder, Xaml>>()
                .Get(func => func(ctx.Builder).Create())
                .Do(xaml => RunChildContext(ctx, xaml));

            ctx.Values[0].ToMaybe()
                .Cast<Func<XamlBuilder, Xaml[]>>()
                .Get(func => func(ctx.Builder))
                .Do(xamls => RunChildContext(ctx, xamls.Select(x => x.Create()).ToArray()));
        }

        private static void RunChildContext(InvokeContext parent, object xaml)
        {
            parent.ExecuteChildContext(args: new[] { xaml }).Dispose();
        }
    }
}