﻿using System;
using System.Windows;
using DynamicXaml.Extensions;

namespace DynamicXaml
{
    public class AttachedPropertyHandler : InvokeMemberHandler
    {
        public bool CanHandle(InvokeContext callContext)
        {
            return callContext.Name.Equals("Attach");
        }

        public void Handle(InvokeContext ctx)
        {
            GuardAgainstSignatureFailures(ctx);
            var attachProp = ctx.Values[0].Cast<DependencyProperty>();
            var value = ctx.Values[1];
            ctx.AddSetterWith<DependencyObject>(xaml => xaml.SetValue(attachProp, value));
        }

        private static void GuardAgainstSignatureFailures(InvokeContext ctx)
        {
            if (ctx.Values.Length != 2 ||
                !ctx.Values[0].CanBeCastTo<DependencyProperty>() ||
                !((DependencyProperty)ctx.Values[0]).IsValidType(ctx.Values[1]))
                throw new InvalidOperationException("Signature issue with attach call: The call should be of the form Attach(DependencyProperty, T value) and value must be accepted by the dependency property.");
        }
    }
}