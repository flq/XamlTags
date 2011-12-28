using System.Collections.Generic;
using System.Reflection;
using DynamicXaml.ResourcesSystem;

namespace DynamicXaml
{
    public class XamlBuilder
    {
        private readonly SetterFactory _setterFactory = new SetterFactory();

        private readonly List<InvokeMemberHandler> _knownInvokeMemberHandlers = new List<InvokeMemberHandler>();
        private ResourceService _resourceService;

        public XamlBuilder()
        {
            _knownInvokeMemberHandlers.Add(new MultiCaseHandler());
            //_knownInvokeMemberHandlers.Add(new NestedInvokeHandler());
            _knownInvokeMemberHandlers.Add(new BindHandler());
            _knownInvokeMemberHandlers.Add(new AddResourceHandler());
            _knownInvokeMemberHandlers.Add(new StaticResourceHandler());
            _knownInvokeMemberHandlers.Add(new AddInvokeHandler());
            _knownInvokeMemberHandlers.Add(new AttachedPropertyHandler());
            _knownInvokeMemberHandlers.Add(new SimpleCaseHandler());
        }

        internal ResourceService ResourceService
        {
            get { return _resourceService; }
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

        internal IEnumerable<InvokeMemberHandler> GetInvokeMemberHandler()
        {
            return _knownInvokeMemberHandlers;
        }

        public void GetResourcesFrom(params Assembly[] assembly)
        {
            var l = new CompositeResourceLoader(assembly);
            _resourceService = new ResourceService(l);
        }
    }
}