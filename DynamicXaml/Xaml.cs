using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using DynamicXaml.Extensions;

namespace DynamicXaml
{
    public class Xaml<T> : DynamicObject
    {
        private readonly XamlBuilder _xamlBuilder;
        private readonly List<InvokeMemberHandler> _invokeMemberHandler;
        private readonly CreationModel<T> _creationModel;
        private readonly Lazy<T> _created;

        public Xaml(XamlBuilder xamlBuilder)
        {
            _xamlBuilder = xamlBuilder;
            _creationModel = new CreationModel<T>();
            _invokeMemberHandler = _xamlBuilder.GetInvokeMemberHandler().ToList();
            _created = new Lazy<T>(()=>_creationModel.Play(Activator.CreateInstance<T>()));
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var callContext = new RootInvokeContext<T>(binder, args, _creationModel, _xamlBuilder.SetterFactory);
            _invokeMemberHandler.MaybeFirst(h => h.CanHandle(callContext)).Do(h => h.Handle(callContext));
            result = this;
            return true;
        }

        public T Create()
        {
            return _created.Value;
        }
    }
}