using System.Collections.Generic;

namespace DynamicXaml
{
    public class XamlBuilder
    {
        private readonly SetterFactory _setterFactory = new SetterFactory();

        private readonly List<InvokeMemberHandler> _knownInvokeMemberHandlers = new List<InvokeMemberHandler>();

        public XamlBuilder()
        {
            _knownInvokeMemberHandlers.Add(new MultiCaseHandler());
            _knownInvokeMemberHandlers.Add(new NestedInvokeHandler());
            _knownInvokeMemberHandlers.Add(new BindHandler());
            _knownInvokeMemberHandlers.Add(new AddResourceHandler());
            _knownInvokeMemberHandlers.Add(new StaticResourceHandler());
            _knownInvokeMemberHandlers.Add(new SimpleCaseHandler());
        }

        internal SetterFactory SetterFactory
        {
            get { return _setterFactory; }
        }

        public dynamic Start<T>()
        {
            return new Xaml<T>(this);
        }

        public dynamic Start<T>(object dataContext)
        {
            return new Xaml<T>(this, dataContext);
        }

        public IEnumerable<InvokeMemberHandler> GetInvokeMemberHandler()
        {
            return _knownInvokeMemberHandlers;
        }
    }
}