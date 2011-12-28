using System;
using System.Collections.Generic;
using DynamicXaml.Extensions;
using System.Linq;

namespace DynamicXaml.ResourcesSystem
{
    public class ResourceService
    {
        private readonly IResourceLoader _loader;

        public ResourceService(IResourceLoader loader, ResourceCacheMode mode = ResourceCacheMode.None)
        {
            _loader = loader;
        }

        public Maybe<T> GetResource<T>(string key)
        {
            return _loader
                .GetDictionaries()
                .MaybeFirst(rd => rd.Contains(key))
                .Get(rd => rd[key])
                .Cast<T>();
        }

        public IEnumerable<KeyValuePair<object, object>> Where(Func<KeyValuePair<object, object>, bool> predicate)
        {
            return _loader.GetDictionaries()
                .SelectMany(rd => rd.Keys.OfType<object>().Select(key => new KeyValuePair<object, object>(key, rd[key])))
                .Where(predicate).AsEnumerable();
        }
    }

    public enum ResourceCacheMode
    {
        None,
        Cached
    }
}