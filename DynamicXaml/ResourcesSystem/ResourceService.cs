using DynamicXaml.Extensions;

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
    }

    public enum ResourceCacheMode
    {
        None,
        Cached
    }
}