using System;

namespace DynamicXaml
{
    public interface SetterProvider
    {
        bool Match(SetterContext ctx);
        Action<T> Setter<T>(SetterContext ctx);
    }
}