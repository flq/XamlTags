using System;

namespace DynamicXaml
{
    internal interface ActionRecorder<T>
    {
        void Add(Action<T> action);
    }
}