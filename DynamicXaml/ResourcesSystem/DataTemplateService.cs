using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using DynamicXaml.Extensions;

namespace DynamicXaml.ResourcesSystem
{
    public class DataTemplateService
    {
        private readonly Dictionary<object, DataTemplate> _templates;

        public DataTemplateService(ResourceService service)
        {
            _templates = service.Where(kv => kv.Value is DataTemplate)
                .Distinct(new EqlComparer())
                .ToDictionary(kv => kv.Key, kv => (DataTemplate)kv.Value);
        }

        public Maybe<DataTemplate> Get(object key)
        {
            return _templates.Get(key);
        }

        public Maybe<DataTemplate> GetForObject(Type modelType)
        {
            return Maybe<DataTemplate>.None;
        }

        public Maybe<DataTemplate> GetForObject(object model)
        {
            if (model == null) throw new ArgumentNullException("model");
            return GetForObject(model.GetType());
        }

        public Maybe<DataTemplate> GetForObject<T>()
        {
            return GetForObject(typeof(T));
        }


        private class EqlComparer : IEqualityComparer<KeyValuePair<object,object>>
        {
            public bool Equals(KeyValuePair<object, object> x, KeyValuePair<object, object> y)
            {
                return Equals(x.Key, y.Key);
            }

            public int GetHashCode(KeyValuePair<object, object> obj)
            {
                return obj.Key != null ? obj.Key.GetHashCode() : -1;
            }
        }
    }
}