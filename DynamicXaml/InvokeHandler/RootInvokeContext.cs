using System;
using System.Dynamic;

namespace DynamicXaml
{
    internal class RootInvokeContext<T> : InvokeContext
    {
        private readonly InvokeMemberBinder _binder;
        private readonly object[] _args;
        private readonly CreationModel<T> _creationModel;
        private readonly SetterFactory _setterFactory;

        public RootInvokeContext(InvokeMemberBinder binder, object[] args, CreationModel<T> creationModel, SetterFactory setterFactory)
        {
            _binder = binder;
            _args = args;
            _creationModel = creationModel;
            _setterFactory = setterFactory;
        }

        public string Name
        {
            get { return _binder.Name; }
        }

        public Type XamlType
        {
            get { return typeof(T); }
        }

        public object[] Values
        {
            get { return _args; }
        }

        public void AddSetterWith(SetterContext setterContext)
        {
            _creationModel.AddSetter(_setterFactory.GetSetter<T>(setterContext));
        }
    }
}