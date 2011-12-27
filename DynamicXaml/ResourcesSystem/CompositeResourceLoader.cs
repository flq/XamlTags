using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Linq;
using DynamicXaml.Extensions;

namespace DynamicXaml.ResourcesSystem
{
    public class CompositeResourceLoader : IResourceLoader
    {
        private readonly IResourceLoader[] _loader;

        public CompositeResourceLoader(params IResourceLoader[] loader)
        {
            _loader = loader;
        }

        public CompositeResourceLoader(params Assembly[] assemblies) : this(assemblies.Select(a => new ResourceLoader(a)).ToArray())
        {
        }

        public bool HandlesAssembly(string assemblyName)
        {
            return _loader.Any(l => l.HandlesAssembly(assemblyName));
        }

        public IEnumerable<string> GetResourceNames()
        {
            return _loader.SelectMany(l => l.GetResourceNames());
        }

        public Maybe<ResourceDictionary> GetDictionary(string path)
        {
            return _loader.Select(l => l.GetDictionary(path)).MaybeFirst();
        }

        public IEnumerable<ResourceDictionary> GetDictionaries()
        {
            return _loader.SelectMany(l => l.GetDictionaries());
        }
    }
}