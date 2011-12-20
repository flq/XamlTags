using System;
using System.ComponentModel;
using DynamicXaml.Extensions;

namespace DynamicXaml
{
    public class SetterProviderFromConverter : SetterProvider
    {

        public bool Match(SetterContext ctx)
        {
            return ctx.PropertyType.MayHaveConverter();
        }

        public Action<T> Setter<T>(SetterContext ctx)
        {
            var converter = ctx.PropertyType
                .MayHaveConverter()
                .Get(cA => cA.ConverterTypeName)
                .Get(Type.GetType)
                .Get(Activator.CreateInstance)
                .Cast<TypeConverter>();
            return xaml => xaml.SetValue(ctx.PropertyName, converter.Value.ConvertFrom(null, System.Globalization.CultureInfo.InvariantCulture, ctx.Value));
        }
    }
}