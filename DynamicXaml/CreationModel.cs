using System;
using System.Collections.Generic;

namespace DynamicXaml
{
    public class CreationModel<T> : IActionRecorder<T>
    {
        private readonly List<Action<T>> _actions = new List<Action<T>>();

        public void AddSetter(Action<T> setter)
        {
            _actions.Add(setter);
        }

        public T Play(T @object)
        {
            foreach (var a in _actions)
                a(@object);
            return @object;
        }

        void IActionRecorder<T>.Add(Action<T> action)
        {
            AddSetter(action);
        }
    }
}