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

        public void Handle(InvokeContext callContext)
        {
            callContext.Values[0].ToMaybe()
                .Cast<Func<XamlBuilder, Xaml>>()
                .Get(func => func(callContext.Builder).Create())
                .Do(xaml => RunChildContext(callContext, xaml));

            callContext.Values[0].ToMaybe()
                .Cast<Func<XamlBuilder, Xaml[]>>()
                .Get(func => func(callContext.Builder))
                .Do(xamls => RunChildContext(callContext, xamls.Select(x => x.Create()).ToArray()));
        }

        private static void RunChildContext(InvokeContext parent, object xaml)
        {
            parent.ExecuteChildContext(args: new[] { xaml }).Dispose();
        }
    }
}