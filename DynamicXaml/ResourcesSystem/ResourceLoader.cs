using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            _resourcenames = new Lazy<IEnumerable<string>>(()=>GetRawResourceNames().ToArray());
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
            return _resourcenames.Value.Select(ConvertToXaml);
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

        private IEnumerable<string> GetRawResourceNames()
        {
            var asm = _assembly;
            var resName = asm.GetName().Name + ".g.resources";
            using (var stream = asm.GetManifestResourceStream(resName))
            {
                if (stream == null)
                    yield break;

                using (var reader = new System.Resources.ResourceReader(stream))
                {
                    foreach (DictionaryEntry entry in reader)
                    {
                        var rawResourceName = (string) entry.Key;
                        var binReader = new BamlBinaryReader((Stream) entry.Value);
                        var r = new BamlRootElementCheck(binReader);
                        var element = r.RootElement();
                        if (element == "ResourceDictionary")
                          yield return rawResourceName;
                    }
                }
            }
        }

        private static string ConvertToXaml(string bamlResource)
        {
            return bamlResource.Replace(".baml", "") + ".xaml";
        }
    }
}