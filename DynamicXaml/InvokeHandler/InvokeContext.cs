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
        void AddSetterWith<T>(Action<T> directSetter);
        InvokeContext ExecuteChildContext(string name = null, object[] args=null);
        object GetValueForArgumentName(string name);
    }
}