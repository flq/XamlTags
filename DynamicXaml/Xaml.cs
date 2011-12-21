using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using DynamicXaml.Extensions;

namespace DynamicXaml
{
    public interface Xaml
    {
        object Create();
    }

    public class Xaml<T> : DynamicObject, Xaml
    {
        private readonly XamlBuilder _xamlBuilder;
        private readonly object _dataContext;
        private readonly List<InvokeMemberHandler> _invokeMemberHandler;
        private readonly CreationModel<T> _creationModel;
        private readonly Lazy<T> _created;

        public Xaml(XamlBuilder xamlBuilder,  object dataContext = null)
        {
            _xamlBuilder = xamlBuilder;
            _dataContext = dataContext;
            _creationModel = new CreationModel<T>(dataContext);
            _invokeMemberHandler = _xamlBuilder.GetInvokeMemberHandler().ToList();
            _created = new Lazy<T>(()=>_creationModel.Play(Activator.CreateInstance<T>()));
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var callContext = new RootInvokeContext<T>(binder, args, _xamlBuilder, _invokeMemberHandler);
            callContext.TransferRecordedActionsInto(_creationModel);
            result = this;
            return true;
        }

        public T Create()
        {
            return _created.Value;
        }

        object Xaml.Create()
        {
            return Create();
        }
    }
}