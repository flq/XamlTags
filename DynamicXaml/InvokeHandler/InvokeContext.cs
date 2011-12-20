using System;

namespace DynamicXaml
{
    public interface InvokeContext
    {
        string Name { get; }
        Type XamlType { get; }
        object[] Values { get; }
        void AddSetterWith(SetterContext setterContext);
    }
}