using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace DynamicXaml.MarkupSystem
{
    /// <summary>
    /// A true XAML writer based on an XmlReader
    /// </summary>
    public class XamlWriter : IDisposable
    {
        private readonly Dictionary<string, string> _references = new Dictionary<string, string>();

        readonly XmlWriter _writer;
        readonly Stack<bool> _isElementOpen = new Stack<bool>(10);


        public XamlWriter(TextWriter output, XamlWriterSettings settings = null, bool closeOutputStream = true)
        {
            _writer = XmlWriter.Create(output, 
                new XmlWriterSettings { CloseOutput = closeOutputStream, Indent = true, IndentChars = "  ", OmitXmlDeclaration = true });

            if (settings == null)
            {
                StartRoot("ContentControl");
                return;
            }

            settings.RegisterReferences(this);
            StartRoot(settings.RootElement ?? "ContentControl");
            _writer.WriteAttributeString("xmlns", "x", null, "http://schemas.microsoft.com/winfx/2006/xaml");
            WriteAllReferences();
            settings.ApplyCustomizations(this);
        }

        public void Dispose()
        {
            while (_isElementOpen.Count > 0)
            {
                _writer.WriteEndElement();
                _isElementOpen.Pop();
            }
            _writer.Close();
        }


        public XamlWriter CloseCurrentElement()
        {
            if (_isElementOpen.Count > 0)
            {
                _writer.WriteEndElement();
                _isElementOpen.Pop();
            }
            return this;
        }

        private void StartRoot(string name)
        {
            var parts = name.Split(':');
            if (parts.Length == 1)
                _writer.WriteStartElement(parts[0], "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            if (parts.Length == 2)
            {
                _writer.WriteStartElement(parts[0], parts[1], _references[parts[0]]);
                _writer.WriteAttributeString("xmlns", null, null, "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            }
            _isElementOpen.Push(true);
        }

        public XamlWriter StartElement(string name)
        {
            var parts = name.Split(':');
            if (parts.Length == 1)
                _writer.WriteStartElement(parts[0]);
            if (parts.Length == 2)
                _writer.WriteStartElement(parts[0], parts[1], _references[parts[0]]);
            _isElementOpen.Push(true);
            return this;
        }

        public XamlWriter StartElement(string name, Action<XamlWriter> action)
        {
            StartElement(name);
            action(this);
            CloseCurrentElement();
            return this;
        }

        public XamlWriter AddAttribute(string attribute, object content)
        {
            return AddAttribute(attribute, content.ToString());
        }

        public XamlWriter AddAttribute(string attribute, string content)
        {
            var parts = attribute.Split(':');
            if (parts.Length == 1)
                _writer.WriteAttributeString(parts[0], content);
            if (parts.Length == 2)
                _writer.WriteAttributeString(parts[0], parts[1], null, content);
            return this;
        }

        public XamlWriter AddAttribute(bool condition, string attribute, string content)
        {
            return condition ? AddAttribute(attribute, content) : this;
        }

        public void RegisterReference(string ns, string clrNamespace, string assembly = null)
        {
            var text = "clr-namespace:" + clrNamespace;
            if (assembly != null)
                text += ";assembly=" + assembly;
            _references.Add(ns, text);
        }

        private void ElementSwitch(string text, Action<XmlWriter,string> withoutNameSpace, Action<XmlWriter,string> withNameSpace)
        {
            
        }

        private void WriteAllReferences()
        {
            foreach (var kv in _references)
            {
                _writer.WriteAttributeString("xmlns", kv.Key, null, kv.Value);
            }
        }

        public bool IsElementOpen()
        {
            return _writer.WriteState == WriteState.Element;
        }
    }

    public interface IXamlWriterSettings
    {
        IXamlWriterSettings AddReference(string ns, string clrNamespace, string assembly = null);
        IXamlWriterSettings CustomizeRootElement(Action<XamlWriter> writerCustomization);
        IXamlWriterSettings SetRootElement(string rootElement);
    }

    public class XamlWriterSettings : IXamlWriterSettings
    {
        private readonly List<Action<XamlWriter>> _references = new List<Action<XamlWriter>>();
        private readonly List<Action<XamlWriter>> _actions = new List<Action<XamlWriter>>();

        public XamlWriterSettings(Action<IXamlWriterSettings> config)
        {
            config(this);
        }

        internal string RootElement { get; private set; }

        internal void RegisterReferences(XamlWriter xamlWriter)
        {
            _references.ForEach(a => a(xamlWriter));
        }

        internal void ApplyCustomizations(XamlWriter xamlWriter)
        {
            _actions.ForEach(a => a(xamlWriter));
        }

        IXamlWriterSettings IXamlWriterSettings.AddReference(string ns, string clrNamespace, string assembly)
        {
            _references.Add(w => w.RegisterReference(ns, clrNamespace, assembly));
            return this;
        }

        IXamlWriterSettings IXamlWriterSettings.CustomizeRootElement(Action<XamlWriter> writerCustomization)
        {
            _actions.Add(writerCustomization);
            return this;
        }

        IXamlWriterSettings IXamlWriterSettings.SetRootElement(string rootElement)
        {
            RootElement = rootElement;
            return this;
        }
    }
}