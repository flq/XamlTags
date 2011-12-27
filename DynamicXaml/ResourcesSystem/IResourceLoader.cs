using System.Collections.Generic;
using System.Windows;
using DynamicXaml.Extensions;

namespace DynamicXaml.ResourcesSystem
{
    public interface IResourceLoader
    {
        bool HandlesAssembly(string assemblyName);

        /// <summary>
        /// Enumerate the names of all resources contained in the provided assembly
        /// </summary>
        IEnumerable<string> GetResourceNames();

        Maybe<ResourceDictionary> GetDictionary(string path);

        IEnumerable<ResourceDictionary> GetDictionaries();
    }
}