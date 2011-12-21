using System;
using System.Collections.Generic;

namespace DynamicXaml
{
    public class XamlBuilder
    {
        private readonly SetterFactory _setterFactory = new SetterFactory();
        private object _dataContext;

        private readonly List<InvokeMemberHandler> _knownInvokeMemberHandlers = new List<InvokeMemberHandler>();

        public XamlBuilder()
        {
            _knownInvokeMemberHandlers.Add(new MultiCaseHandler());
            _knownInvokeMemberHandlers.Add(new NestedInvokeHandler());
            _knownInvokeMemberHandlers.Add(new SimpleCaseHandler());
        }

        internal SetterFactory SetterFactory
        {
            get { return _setterFactory; }
        }

        internal object DataContext
        {
            get { return _dataContext; }
        }

        public dynamic Start<T>()
        {
            return new Xaml<T>(this);
        }

        public dynamic Start<T>(object dataContext)
        {
            _dataContext = dataContext;
            return new Xaml<T>(this);
        }

        public IEnumerable<InvokeMemberHandler> GetInvokeMemberHandler()
        {
            return _knownInvokeMemberHandlers;
        }
    }
}