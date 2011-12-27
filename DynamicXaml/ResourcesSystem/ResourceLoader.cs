using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using DynamicXaml.Extensions;

namespace DynamicXaml.ResourcesSystem
{
    /// <summary>
    /// Resource access on an assembly
    /// </summary>
    public class ResourceLoader : IResourceLoader
    {
        private readonly Assembly _assembly;
        private readonly Lazy<IEnumerable<string>> _resourcenames;


        public ResourceLoader(Assembly assembly)
        {
            _assembly = assembly;
            _resourcenames = new Lazy<IEnumerable<string>>(GetResourceNamesFresh);
        }

        public bool HandlesAssembly(string assemblyName)
        {
            return _assembly.GetName().Name.Equals(assemblyName, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Enumerate the names of all resources contained in the provided assembly
        /// </summary>
        public IEnumerable<string> GetResourceNames()
        {
            return _resourcenames.Value;
        }

        public Maybe<ResourceDictionary> GetDictionary(string path)
        {
            if (!UriParser.IsKnownScheme("pack"))
                UriParser.Register(new GenericUriParser(GenericUriParserOptions.GenericAuthority), "pack", -1);

            if (!GetResourceNames().Any(s => s.Equals(path, StringComparison.InvariantCultureIgnoreCase)))
            {
                Debug.WriteLine("Unknown resource name " + path);
                return Maybe<ResourceDictionary>.None;
            }
            try
            {
                var dict = new ResourceDictionary();
                // if the resource dictionary is the app.xaml, an exception occurs if an application is already running
                // if it is running, we return the resources of the already running app.
                if (path.ToLowerInvariant().StartsWith("app") && Application.Current != null)
                {
                    return Application.Current.Resources.ToMaybe();
                }
                var uri = new Uri("/" + _assembly.GetName().Name + ";component/" + path.ToLowerInvariant(),
                                  UriKind.Relative);
                dict.Source = uri;
                return dict.ToMaybe();
            }
            catch (InvalidOperationException)
            {
                Debug.WriteLine(path + " could not be loaded as Resource");
                return Maybe<ResourceDictionary>.None;
            }
        }

        public IEnumerable<ResourceDictionary> GetDictionaries()
        {
            return GetResourceNames().Select(GetDictionary).Where(rd => rd.HasValue).Select(rd => rd.Value);
        }

        private IEnumerable<string> GetResourceNamesFresh()
        {
            var asm = _assembly;
            var resName = asm.GetName().Name + ".g.resources";
            using (var stream = asm.GetManifestResourceStream(resName))
            using (var reader = new System.Resources.ResourceReader(stream))
            {
                return reader.Cast<DictionaryEntry>().Select(entry => ((string)entry.Key).Replace(".baml", "") + ".xaml").ToArray();
            }
        }
    }
}