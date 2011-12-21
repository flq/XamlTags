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
        InvokeContext ExecuteChildContext(string name = null, object[] args=null);
    }
}