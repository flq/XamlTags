using System;
using System.Collections.Generic;
using System.Dynamic;
using DynamicXaml.Extensions;

namespace DynamicXaml
{
    internal class RootInvokeContext<T> : InvokeContext, IActionRecorder<T>
    {
        private readonly InvokeMemberBinder _binder;
        private readonly object[] _args;
        private readonly XamlBuilder _builder;
        private readonly IEnumerable<InvokeMemberHandler> _invokeMemberHandler;
        private readonly List<Action<T>> _recordedActions = new List<Action<T>>();
        private RootInvokeContext<T> _parent;

        public RootInvokeContext(InvokeMemberBinder binder, object[] args, XamlBuilder builder, IEnumerable<InvokeMemberHandler> invokeMemberHandler)
        {
            _binder = binder;
            _args = args;
            _builder = builder;
            _invokeMemberHandler = invokeMemberHandler;
            Name = _binder.Name;
        }

        public string Name
        {
            get;
            private set;
        }

        public Type XamlType
        {
            get { return typeof(T); }
        }

        public object[] Values
        {
            get { return _args; }
        }

        public XamlBuilder Builder
        {
            get { return _builder; }
        }

        public void AddSetterWith(SetterContext setterContext)
        {
            _recordedActions.Add(_builder.SetterFactory.GetSetter<T>(setterContext));
        }

        public InvokeContext ExecuteChildContext(string name = null, object[] args = null)
        {
            var context = new RootInvokeContext<T>(_binder, args ?? _args, _builder, new List<InvokeMemberHandler>(_invokeMemberHandler));
            if (name != null)
                context.Name = name;
            context._parent = this;
            return context;
        }

        public void TransferRecordedActionsInto(IActionRecorder<T> actionRecorder)
        {
            _invokeMemberHandler.MaybeFirst(h => h.CanHandle(this)).Do(h => h.Handle(this));
            _recordedActions.ForEach(actionRecorder.Add);
        }

        public void Dispose()
        {
            if (_parent != null)
                TransferRecordedActionsInto(_parent);
            _recordedActions.Clear();
        }

        void IActionRecorder<T>.Add(Action<T> action)
        {
            _recordedActions.Add(action);
        }
    }
}