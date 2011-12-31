using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Linq;
using DynamicXaml.Extensions;

namespace DynamicXaml.ResourcesSystem
{
    public class DataTemplateService
    {
        private readonly Dictionary<object, DataTemplate> _templates;
        private readonly TypeTree _tree;

        public DataTemplateService(ResourceService service)
        {
            _templates = service.Where(kv => kv.Value is DataTemplate)
                .Distinct(new EqlComparer())
                .ToDictionary(kv => kv.Key, kv => (DataTemplate)kv.Value);
            _tree = new TypeTree(_templates.Values);

        }

        public Maybe<DataTemplate> Get(object key)
        {
            return _templates.Get(key);
        }

        public Maybe<DataTemplate> GetForObject(Type modelType)
        {
            return _tree.FindForType(modelType);
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

    internal class TypeTree
    {
        private readonly Dictionary<Type,DataTemplate> _interfaceTemplates = new Dictionary<Type, DataTemplate>();
        private readonly TypeNode<DataTemplate> _root = new TypeNode<DataTemplate>(typeof(object));

        public TypeTree(IEnumerable<DataTemplate> dataTemplates)
        {
            dataTemplates
                .Where(dt => dt.DataType is Type)
                .Select(dt => new { Type = (Type)dt.DataType, Template = dt })
                .Pipeline(dt => { if (dt.Type.IsInterface) _interfaceTemplates.Add(dt.Type, dt.Template); })
                .Pipeline(dt => { if (!dt.Type.IsInterface) _root.SortIn(dt.Type, dt.Template); })
                .Execute();
        }

        public Maybe<DataTemplate> FindForType(Type type)
        {
            return _interfaceTemplates.Get(type).Or(_root.FindFor(type));
        }
    }

    [DebuggerDisplay("Type: {_type.Name}, Value: {_item}")]
    internal class TypeNode<T> where T : class
    {
        private readonly Type _type;
        private T _item;
        private readonly List<TypeNode<T>> _childs = new List<TypeNode<T>>();

        public TypeNode(Type type, T item = default(T))
        {
            _type = type;
            _item = item;
        }

        public TypeNode(Type type, IEnumerable<TypeNode<T>> childs)
        {
            _type = type;
            _childs.AddRange(childs);
        }

        private TypeNode<T> Parent { get; set; }

        public void SortIn(Type type, T item)
        {
            if (_type.Equals(type))
            {
                _item = item;
            }
            else
            {
                var node = CreateBottomUpBranch(_type, new TypeNode<T>(type, item));
                Merge(node);
            }
        }

        public Maybe<T> FindFor(Type type)
        {
            if (_type.Equals(type))
                return _item.ToMaybe();
            var child = _childs.FirstOrDefault(tn => type.CanBeCastTo(tn._type));
            if (child != null)
                return child.FindFor(type);
            if (type.CanBeCastTo(_type))
                return _item.ToMaybe();
            return Maybe<T>.None;
        }

        private void Merge(TypeNode<T> node)
        {
            var typenode = _childs.FirstOrDefault(tn => tn._type.Equals(node._type));
            if (typenode == null)
                _childs.Add(node);
            else
            {
                typenode._item = node._item;
                foreach (var childTypeNode in node._childs)
                {
                    typenode.Merge(childTypeNode);
                }
            }
        }

        private static TypeNode<T> CreateBottomUpBranch(Type rootType, TypeNode<T> typeNode)
        {
            var baseType = typeNode._type.BaseType;
            while (baseType != null && baseType != rootType)
            {
                typeNode.Parent = new TypeNode<T>(baseType, new [] { typeNode });
                typeNode = typeNode.Parent;
                baseType = typeNode._type.BaseType;
            }
            return typeNode;
        }
    }
}