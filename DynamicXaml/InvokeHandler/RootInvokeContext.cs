﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using DynamicXaml.Extensions;
using System.Linq;

namespace DynamicXaml
{
    internal class RootInvokeContext<T> : InvokeContext, ActionRecorder<T>
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
            get { return _args.Length > 0 ? _args : new object[1]; }
        }

        public XamlBuilder Builder
        {
            get { return _builder; }
        }

        public void AddSetterWith(SetterContext setterContext)
        {
            setterContext.Builder = _builder;
            _recordedActions.Add(_builder.SetterFactory.GetSetter<T>(setterContext));
        }

        public void AddSetterWith(BindSetterContext setterContext)
        {
            _recordedActions.Add(_builder.SetterFactory.GetSetter<T>(setterContext));
        }

        public void AddSetterWith<T1>(Action<T1> directSetter)
        {
            if (typeof(T).CanBeCastTo<T1>())
            {
                _recordedActions.Add(t => directSetter(t.Cast<T1>()));
            }
        }

        public InvokeContext ExecuteChildContext(string name = null, object[] args = null)
        {
            var context = new RootInvokeContext<T>(_binder, args ?? _args, _builder, new List<InvokeMemberHandler>(_invokeMemberHandler));
            if (name != null)
                context.Name = name;
            context._parent = this;
            return context;
        }

        public object GetValueForArgumentName(string key)
        {
            // Unnamed values must appear in the beginning
            var offset = Values.Length - _binder.CallInfo.ArgumentNames.Count;
            var index = _binder.CallInfo.ArgumentNames.IndexOf(key);
            if (index > -1)
                return _args[index + offset]; // First is always assumed to be unnamed
            return null;
        }

        public bool IsArgumentNameSpecified(string name)
        {
            return _binder.CallInfo.ArgumentNames.IndexOf(name) != -1;
        }

        public void TransferRecordedActionsInto(ActionRecorder<T> actionRecorder)
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

        void ActionRecorder<T>.Add(Action<T> action)
        {
            _recordedActions.Add(action);
        }
    }
}