using System;

namespace DynamicXaml
{
    public interface InvokeContext : IDisposable
    {
        string Name { get; }
        Type XamlType { get; }
        object[] Values { get; }
        XamlBuilder Builder { get; }
        void AddSetterWith(SetterContext setterContext);
        void AddSetterWith(BindSetterContext setterContext);
        InvokeContext ExecuteChildContext(string name = null, object[] args=null);
        object GetValueForArgumentName(string name);
    }
}