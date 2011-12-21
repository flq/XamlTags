using System;
using System.Collections;
using DynamicXaml.Extensions;

namespace DynamicXaml
{
    public class ListSetterProvider : SetterProvider
    {
        public bool Match(SetterContext ctx)
        {
            return ctx.PropertyType.CanBeCastTo<IList>() && ctx.Value.CanBeCastTo<object[]>();
        }

        public Action<T> Setter<T>(SetterContext ctx)
        {
            return xaml =>
                       {
                           var list = xaml.GetValue<IList>(ctx.PropertyName);
                           var values = (object[])ctx.Value;
                           values.ForEach(v => list.Add(v));
                       };
        }
    }
}