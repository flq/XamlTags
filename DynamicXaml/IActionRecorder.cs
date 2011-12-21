using System;

namespace DynamicXaml
{
    internal interface IActionRecorder<T>
    {
        void Add(Action<T> action);
    }
}