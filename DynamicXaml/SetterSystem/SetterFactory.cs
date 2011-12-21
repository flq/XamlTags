using System;
using System.Collections.Generic;
using System.Linq;
using DynamicXaml.Extensions;

namespace DynamicXaml
{
    internal class SetterFactory
    {
        private readonly List<SetterProvider> _knownSetterProvider = new List<SetterProvider>();

        public SetterFactory()
        {
            _knownSetterProvider.Add(new StandardSetterProvider());
            _knownSetterProvider.Add(new SetterProviderFromConverter());
            _knownSetterProvider.Add(new ListSetterProvider());
        }

        public Action<T> GetSetter<T>(SetterContext ctx)
        {
            var setter = _knownSetterProvider.FirstOrDefault(p => p.Match(ctx));
            if (setter == null)
                throw new NotSupportedException("Could not resolve a way to set {0} to target ({1}:{2})".Fmt(ctx.Value.GetType().Name, ctx.PropertyName, ctx.PropertyType.Name));
            return setter.Setter<T>(ctx);
        }
    }
}