using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DynamicXaml.ResourcesSystem
{
    /// <summary>
    /// Adapted from the StylesExplorer project on codeplex.
    /// The original class is able to fully parse the BAML. This class is reduced to read the root element
    /// to identify whether it is a ResourceDictionary or not. That way we can skip instantiating things
    /// that we shouldn't instantiate.
    /// </summary>
    internal class BamlRootElementCheck
    {
        private readonly BamlBinaryReader _reader;
        private bool _initialized;
        private BamlRecordType _currentType;
        private int _bytesToSkip;
        private readonly Dictionary<short,string> _assemblyTable = new Dictionary<short, string>();
        private readonly Dictionary<short, string> _stringTable = new Dictionary<short, string>();
        private readonly Dictionary<short, string> _attribTable = new Dictionary<short, string>();
        private readonly Dictionary<short, string> _typeTable = new Dictionary<short, string>();
        private bool _firstElementStartMet;

        public BamlRootElementCheck(BamlBinaryReader reader)
        {
            _reader = reader;
        }

        public string RootElement()
        {
            EnsureInit();
            var root = "unknown";
            do
            {
                _currentType = (BamlRecordType)_reader.ReadByte();
                if (_currentType == BamlRecordType.DocumentEnd) break;
                if (_currentType == BamlRecordType.ElementStart && !_firstElementStartMet)
                {
                    _firstElementStartMet = true;
                    root = ReadElementStart();
                    break;
                }
                var position = _reader.BaseStream.Position;
                ComputeBytesToSkip();
                ProcessNext();

                if (_bytesToSkip > 0)
                {
                    _reader.BaseStream.Position = position + _bytesToSkip;
                }
            } while (true);

            return root;
        }

        private void EnsureInit()
        {
            if (_initialized) return;

            var startChars = _reader.ReadInt32();
            var type = new String(new BinaryReader(_reader.BaseStream, Encoding.Unicode).ReadChars(startChars >> 1));
            if (type != "MSBAML")
                throw new NotSupportedException("Not a MS BAML");

            int r = _reader.ReadInt32();
            int s = _reader.ReadInt32();
            int t = _reader.ReadInt32();
            if (((r != 0x600000) || (s != 0x600000)) || (t != 0x600000))
                throw new NotSupportedException("Apparently corrupt BAML?!");

            _initialized = true;
        }

        private void ComputeBytesToSkip()
        {
            _bytesToSkip = 0;
            switch (_currentType)
            {
                case BamlRecordType.PropertyWithConverter:
                case BamlRecordType.DefAttributeKeyString:
                case BamlRecordType.PresentationOptionsAttribute:
                case BamlRecordType.Property:
                case BamlRecordType.PropertyCustom:
                case BamlRecordType.Text:
                case BamlRecordType.TextWithConverter:
                case BamlRecordType.XmlnsProperty:
                case BamlRecordType.DefAttribute:
                case BamlRecordType.PiMapping:
                case BamlRecordType.AssemblyInfo:
                case BamlRecordType.TypeInfo:
                case BamlRecordType.AttributeInfo:
                case BamlRecordType.StringInfo:
                    _bytesToSkip = _reader.ReadCompressedInt32();
                    break;
            }
        }

        private void ProcessNext()
        {
            switch (_currentType)
            {
                case BamlRecordType.DocumentStart:
                    ReadDocumentStart();
                    break;
                case BamlRecordType.AssemblyInfo:
                    ReadAssemblyInfo();
                    break;
                case BamlRecordType.DocumentEnd:
                    break;
                case BamlRecordType.ElementStart:
                    ReadElementStart();
                    break;
                case BamlRecordType.StringInfo:
                    ReadStringInfo();
                    break;
                case BamlRecordType.LineNumberAndPosition:
                    _reader.ReadInt32();
                    _reader.ReadInt32();
                    break;
                case BamlRecordType.LinePosition:
                    _reader.ReadInt32();
                    break;
                case BamlRecordType.ConnectionId:
                    _reader.ReadInt32();
                    break;
                case BamlRecordType.DeferableContentStart:
                    _reader.ReadInt32();
                    break;
                case BamlRecordType.AttributeInfo:
                    ReadAttributeInfo();
                    break;
                case BamlRecordType.PiMapping:
                    ReadPiMapping();
                    break;
                case BamlRecordType.TypeInfo:
                    ReadTypeInfo();
                    break;
                    /*
                case BamlRecordType.ElementEnd:
                case BamlRecordType.XmlnsProperty:
                case BamlRecordType.DefAttribute:
                case BamlRecordType.DefAttributeKeyType:
                case BamlRecordType.DefAttributeKeyString:
                case BamlRecordType.PropertyListStart:
                case BamlRecordType.PropertyListEnd:
                case BamlRecordType.Property:
                case BamlRecordType.PropertyWithConverter:
                case BamlRecordType.PropertyWithExtension:
                case BamlRecordType.PropertyDictionaryStart:
                case BamlRecordType.PropertyCustom:
                case BamlRecordType.PropertyDictionaryEnd:
                case BamlRecordType.PropertyComplexStart:
                case BamlRecordType.PropertyComplexEnd:
                case BamlRecordType.ContentProperty:
                case BamlRecordType.ConstructorParametersStart:
                case BamlRecordType.ConstructorParametersEnd:
                case BamlRecordType.ConstructorParameterType:
                case BamlRecordType.Text:
                case BamlRecordType.TextWithConverter:
                case BamlRecordType.PropertyWithStaticResourceId:
                case BamlRecordType.OptimizedStaticResource:
                case BamlRecordType.KeyElementStart:
                case BamlRecordType.KeyElementEnd:
                case BamlRecordType.PropertyTypeReference:
                case BamlRecordType.StaticResourceStart:
                case BamlRecordType.StaticResourceEnd:
                case BamlRecordType.StaticResourceId:
                case BamlRecordType.PresentationOptionsAttribute: */
                default:
                    throw new NotImplementedException("Unexpected BAML Token " + _currentType);
            }
        }

        private void ReadTypeInfo()
        {
            var typeId = _reader.ReadInt16(); // typeid;
            _reader.ReadInt16(); // assemblyid
            var fullName = _reader.ReadString();
            _typeTable.Add(typeId, fullName);
        }

        private void ReadDocumentStart()
        {
            _reader.ReadBoolean();
            _reader.ReadInt32();
            _reader.ReadBoolean();
        }

        private void ReadAssemblyInfo()
        {
            var key = _reader.ReadInt16();
            var text = _reader.ReadString();
            _assemblyTable.Add(key, text);
        }

        private void ReadPiMapping()
        {
            _reader.ReadString(); // xmlNamespace
            _reader.ReadString(); // clrNamespace
            _reader.ReadInt16(); // assemblyId
        }

        private void ReadAttributeInfo()
        {
            var key = _reader.ReadInt16();
            _reader.ReadInt16(); //identifier
            _reader.ReadByte();
            var name = _reader.ReadString();
            _attribTable.Add(key, name);
        }

        private void ReadStringInfo()
        {
            var key = _reader.ReadInt16();
            var text = _reader.ReadString();
            _stringTable.Add(key, text);
        }

        private string ReadElementStart()
        {
            var identifier = _reader.ReadInt16();
            _reader.ReadByte();

            if (_typeTable.ContainsKey(identifier))
                return _typeTable[identifier];

            if (-identifier == 0x20c)
                return "ResourceDictionary";

            return "other";
        }
    }

        internal enum BamlRecordType : byte
    {
        AssemblyInfo = 0x1c,
        AttributeInfo = 0x1f,
        ClrEvent = 0x13,
        Comment = 0x17,
        ConnectionId = 0x2d,
        ConstructorParametersEnd = 0x2b,
        ConstructorParametersStart = 0x2a,
        ConstructorParameterType = 0x2c,
        ContentProperty = 0x2e,
        DefAttribute = 0x19,
        DefAttributeKeyString = 0x26,
        DefAttributeKeyType = 0x27,
        DeferableContentStart = 0x25,
        DefTag = 0x18,
        DocumentEnd = 2,
        DocumentStart = 1,
        ElementEnd = 4,
        ElementStart = 3,
        EndAttributes = 0x1a,
        KeyElementEnd = 0x29,
        KeyElementStart = 40,
        LastRecordType = 0x39,
        LineNumberAndPosition = 0x35,
        LinePosition = 0x36,
        LiteralContent = 15,
        NamedElementStart = 0x2f,
        OptimizedStaticResource = 0x37,
        PiMapping = 0x1b,
        PresentationOptionsAttribute = 0x34,
        ProcessingInstruction = 0x16,
        Property = 5,
        PropertyArrayEnd = 10,
        PropertyArrayStart = 9,
        PropertyComplexEnd = 8,
        PropertyComplexStart = 7,
        PropertyCustom = 6,
        PropertyDictionaryEnd = 14,
        PropertyDictionaryStart = 13,
        PropertyListEnd = 12,
        PropertyListStart = 11,
        PropertyStringReference = 0x21,
        PropertyTypeReference = 0x22,
        PropertyWithConverter = 0x24,
        PropertyWithExtension = 0x23,
        PropertyWithStaticResourceId = 0x38,
        RoutedEvent = 0x12,
        StaticResourceEnd = 0x31,
        StaticResourceId = 50,
        StaticResourceStart = 0x30,
        StringInfo = 0x20,
        Text = 0x10,
        TextWithConverter = 0x11,
        TextWithId = 0x33,
        TypeInfo = 0x1d,
        TypeSerializerInfo = 30,
        Unknown = 0,
        XmlAttribute = 0x15,
        XmlnsProperty = 20
    }
}