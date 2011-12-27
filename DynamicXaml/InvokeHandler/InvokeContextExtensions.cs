using System.Windows;

namespace DynamicXaml
{
    public static class InvokeContextExtensions
    {
        public static BindSetterContext NewBindSetterContext(this InvokeContext ctx, DependencyProperty depProp)
        {
            var bc = new BindSetterContext(ctx.Values[0] != null ? ctx.Values[0].ToString() : ".", depProp);
            var value = ctx.GetValueForArgumentName("converter");
            if (value != null)
                bc.Add("converter", value);
            if (ctx.Name.StartsWith("OneWay"))
                bc.Add("oneway", true);

            value = ctx.GetValueForArgumentName("oneway");
            if (value != null)
                bc.Add("oneway", (bool)value);

            return bc;
        }
    }
}